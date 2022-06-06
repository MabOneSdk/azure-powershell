function Set-AzDataProtectionMSIPermission {
    [OutputType('Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api20210701.IBackupInstanceResource')]
    [CmdletBinding(PositionalBinding=$false)]
    [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Description('Initializes Backup instance Request object for configuring backup')]

    param(
        [Parameter(Mandatory, HelpMessage='Backup instance request object which will be used to configure backup')]
        [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api20210701.IBackupInstanceResource]
        ${BackupInstance},
        
        [Parameter(Mandatory=$false, HelpMessage='ID of the keyvault')]
        [ValidatePattern("/subscriptions/([A-z0-9\-]+)/resourceGroups/(?<rg>.+)/(?<id>.+)")]
        [System.String]
        ${KeyvaultId},

        [Parameter(Mandatory, HelpMessage='Resource group of the backup vault')]
        [System.String]
        ${VaultResourceGroup},
        
        [Parameter(Mandatory, HelpMessage='Name of the backup vault')]
        [System.String]
        ${VaultName},

        [Parameter(Mandatory, HelpMessage='Scope at which the permissions need to be granted')]
        [System.String]
        [ValidateSet("Resource","ResourceGroup","Subscription")]
        ${PermissionsScope}
        
    )

    process {
          CheckResourcesModuleDependency
          CheckPostgreSqlModuleDependency         
          #CheckKeyVaultModuleDependency                  
          
          $DatasourceId = $BackupInstance.Property.DataSourceInfo.ResourceId
          $DatasourceType = $BackupInstance.Property.DataSourceInfo.ResourceType

          if($DatasourceType -eq "Microsoft.Storage/storageAccounts"){$DatasourceType = "AzureBlob"}
          elseif($DatasourceType -eq "Microsoft.Compute/disks"){$DatasourceType = "AzureDisk"}
          elseif($DatasourceType -eq "Microsoft.DBforPostgreSQL/servers/databases"){$DatasourceType = "AzureDatabaseForPostgreSQL"}

          #Validation
          if($DatasourceType -eq "AzureDatabaseForPostgreSQL" -and $KeyvaultId -eq "")
          {
              $message = "Please Provide KeyVaultId"
              throw $message
          }

          $manifest = LoadManifest -DatasourceType $DatasourceType.ToString()

          $vault = Get-AzDataProtectionBackupVault -VaultName $VaultName -ResourceGroupName $VaultResourceGroup
          $ResourceArray = $DataSourceId.Split("/")
          $ResourceRG = "/subscriptions/" + $ResourceArray[2] + "/resourceGroups/" + $ResourceArray[4]
          $SubscriptionName = "/subscriptions/" + $ResourceArray[2]
          
          $AllRoles =  Get-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId
               
          foreach($Permission in $manifest.datasourcePermissions)
          {
              $CheckPermission = $AllRoles
              | Where-Object { ($_.Scope -eq $DataSourceId -or $_.Scope -eq $ResourceRG -or  $_.Scope -eq $SubscriptionName) -and $_.RoleDefinitionName -eq $Permission}
              
              if($CheckPermission -ne $null)
              {
                  Write-Host "Required Permissions Already Assigned."
              }

              else
              {
                  Write-Debug "Assigning Required Permissions"

                  if($PermissionsScope -eq "Resource")
                  {
                      New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName $Permission -Scope $DatasourceId  | Out-Null
                  }
              
                  elseif($PermissionsScope -eq "ResourceGroup")
                  {
                      New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName $Permission -Scope $ResourceRG  | Out-Null
                  }

                  elseif($PermissionsScope -eq "Subscription")
                  {                   
                      New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName $Permission -Scope $SubscriptionName  | Out-Null
                  }

                  Write-Host "Assigned Required Permissions"
              }
          }
              

          foreach($Permission in $manifest.snapshotRGPermissions)
          {
              $SnapshotResourceGroupId = $BackupInstance.Property.PolicyInfo.PolicyParameter.DataStoreParametersList[0].ResourceGroupId

              $CheckPermission = $AllRoles
              | Where-Object { ($_.Scope -eq $SnapshotResourceGroupId -or $_.Scope -eq $SubscriptionName)  -and $_.RoleDefinitionName -eq $Permission}

              Write-Host "CheckPermission = $($CheckPermission | ConvertTo-Json -Depth 10)"

              if($CheckPermission -ne $null)
              {
                  Write-Host "Required Permissions Already Assigned."
              }

              else
              {
                  Write-Host "Assigning Required Permissions"

                  if($PermissionsScope -eq "Subscription")
                  {
                      New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName $Permission -Scope $SubscriptionName | Out-Null
                  }             
                  else
                  {
                      New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName $Permission -Scope $SnapshotResourceGroupId  | Out-Null
                  }             

                  Write-Host "Assigned Required Permissions"
              }
          }

          foreach($Permission in $manifest.keyVaultPermissions)
          {
              $KeyvaultArray = $KeyvaultId.Split("/")
              $KeyvaultRG = "/subscriptions/" + $KeyvaultArray[2] + "/resourceGroups/" + $KeyvaultArray[4]
              $KeyvaultSubscriptionName = "/subscriptions/" + $KeyvaultArray[2]
              $KeyvaultName = $KeyvaultArray[8]
              
              $ServerRG = $ResourceArray[4]
              $ServerName = $ResourceArray[8] 
              $KeyVault = Get-AzKeyVault -VaultName $KeyvaultName
              
              Update-AzKeyVaultNetworkRuleSet -VaultName $KeyvaultName -Bypass AzureServices 
              Update-AzPostgreSqlServer -ResourceGroupName $ServerRG -ServerName $ServerName -PublicNetworkAccess Enabled | Out-Null
              New-AzPostgreSqlFirewallRule -Name AllowAllAzureIps -ResourceGroupName $ServerRG -ServerName $ServerName -EndIPAddress 0.0.0.0 -StartIPAddress 0.0.0.0 | Out-Null
              
              if($KeyVault.EnableRbacAuthorization -eq $false)
              {
                  Write-Host "Assigning Vault Access Policies"
                  Set-AzKeyVaultAccessPolicy -VaultName $KeyvaultName -ObjectId $vault.IdentityPrincipalId -PermissionsToSecrets Get,List
              }

              else
              {
                  Write-Host "Assigning RBAC Policies"

                  $CheckPermission = $AllRoles
                  | Where-Object { ($_.Scope -eq $KeyvaultId -or $_.Scope -eq $KeyvaultRG -or  $_.Scope -eq $KeyvaultSubscription) -and $_.RoleDefinitionName -eq $Permission}
              
                  Write-Host "CheckPermission = $($CheckPermission | ConvertTo-Json -Depth 10)"
              
                  if($CheckPermission -ne $null)
                  {
                      Write-Host "Required Permissions Already Assigned."
                  }

                  else
                  {
                      Write-Host "Assigning Required Permissions"

                      if($PermissionsScope -eq "Resource")
                      {
                          New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName $Permission -Scope $KeyvaultId  | Out-Null
                      }
              
                      elseif($PermissionsScope -eq "ResourceGroup")
                      {
                          New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName $Permission -Scope $KeyvaultRG  | Out-Null
                      }

                      elseif($PermissionsScope -eq "Subscription")
                      {                   
                          New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName $Permission -Scope $KeyvaultSubscriptionName  | Out-Null
                      }

                      Write-Host "Assigned Required Permissions"
                  }
              }
          }      
          
    }
}