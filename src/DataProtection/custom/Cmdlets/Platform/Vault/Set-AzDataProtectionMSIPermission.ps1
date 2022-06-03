


function Set-AzDataProtectionMSIPermission {
    [OutputType('Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api20210701.IBackupInstanceResource')]
    [CmdletBinding(PositionalBinding=$false)]
    [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Description('Initializes Backup instance Request object for configuring backup')]

    param(
        [Parameter(Mandatory, HelpMessage='Backup instance request object which will be used to configure backup')]
        [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api20210701.IBackupInstanceResource]
        ${BackupInstance},

        <#[Parameter(Mandatory, HelpMessage='ID of the datasource to be protected')]
        [System.String]
        [ValidatePattern("/subscriptions/([A-z0-9\-]+)/resourceGroups/(?<rg>.+)/(?<id>.+)")]
        ${DatasourceId},

        [Parameter(Mandatory, HelpMessage='Datasource Type')]
        [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Support.DatasourceTypes]
        ${DatasourceType},
              

        [Parameter(Mandatory=$false, HelpMessage=' Operation for which the required permissions should be granted')]
        [System.String]
        ${Operation},
  
        [Parameter(Mandatory=$false, HelpMessage='Resource group which will contain the disk snapshots')]
        [System.String]
        [ValidatePattern("/subscriptions/([A-z0-9\-]+)/resourcegroups/(?<rg>.+)")]
        ${SnapshotResourceGroupId},
       
        [Parameter(Mandatory=$false, HelpMessage='Resource group in which the disk should be restored')]
        [System.String]
        ${TargetResourceGroupForRestore},
        #>
        [Parameter(Mandatory=$false, HelpMessage='Resource group of the backup vault')]
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
                  
          #Write-Host "instance = $($BackupInstance | ConvertTo-Json -Depth 10)"

          $DatasourceId = $BackupInstance.Property.DataSourceInfo.ResourceId
          $DatasourceType = $BackupInstance.Property.DataSourceInfo.ResourceType

          if($DatasourceType -eq "Microsoft.Storage/storageAccounts"){$DatasourceType = "AzureBlob"}
          elseif($DatasourceType -eq "Microsoft.Compute/disks"){$DatasourceType = "AzureDisk"}
          elseif($DatasourceType -eq "Microsoft.DBforPostgreSQL/servers/databases"){$DatasourceType = "AzureDatabaseForPostgreSQL"}


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
              
              #Write-Host "CheckPermission = $($CheckPermission | ConvertTo-Json -Depth 10)"

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
              Write-Host "($SnapshotResourceGroupId | ConvertTo-Json -Depth 10)"

              $CheckPermission = $AllRoles
              | Where-Object { ($_.Scope -eq $SnapshotResourceGroupId -or $_.Scope -eq $SubscriptionName)  -and $_.RoleDefinitionName -eq $Permission}

              Write-Host "CheckPermission = $($CheckPermission | ConvertTo-Json -Depth 10)"

              if($CheckPermission -ne $null)
              {
                  Write-Host "Required Permissions Already Assigned."
              }

              else
              {
                  Write-Debug "Assigning Required Permissions"

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
              
              $ServerRG = $ResourceArray[4]
              $ServerName = $ResourceArray[8] 

              Write-Host "ServerRG = $($ServerRG)"
              Write-Host "ServerName = $($ServerName)"


              $CheckPermission = $AllRoles
              | Where-Object { ($_.Scope -eq $KeyvaultId -or $_.Scope -eq $KeyvaultRG -or  $_.Scope -eq $KeyvaultSubscription) -and $_.RoleDefinitionName -eq $Permission}
              
              Write-Host "CheckPermission = $($CheckPermission | ConvertTo-Json -Depth 10)"

              if($CheckPermission -ne $null)
              {
                  Write-Host "Required Permissions Already Assigned."
              }

              else
              {
                  Write-Debug "Assigning Required Permissions"

                  Update-AzPostgreSqlServer -ResourceGroupName $ServerRG -ServerName $ServerName -PublicNetworkAccess Enabled
                  New-AzPostgreSqlFirewallRule -Name AllowAllAzureIps -ResourceGroupName $ServerRG -ServerName $ServerName -EndIPAddress 0.0.0.0 -StartIPAddress 0.0.0.0

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
                      New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName $Permission -Scope $KeyvaultSubscription  | Out-Null
                  }

                  Write-Host "Assigned Required Permissions"
              }
          }
          

          
          
          

          <#
          $DiskBackupReader=  $AllRoles
          | Where-Object { ($_.Scope -eq $DataSourceId -or $_.Scope -eq $ResourceRG -or  $_.Scope -eq $SubscriptionName) -and $_.RoleDefinitionName -eq "Disk Backup Reader"}
 
          Write-Host "DiskBackupreader = $($DiskBackupReader | ConvertTo-Json -Depth 10)"

          if($DiskBackupReader -ne $null)
          {
              Write-Host "Disk Backup Reader role already assigned on Disk"
          }

          elseif ($DiskBackupReader -eq $null)
          {
              Write-Host "Assigning Disk Backup Reader role on Disk"

              if($PermissionsScope -eq "Resource")
              {New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName "Disk Backup Reader" -Scope $DatasourceId  | Out-Null}
              
              elseif($PermissionsScope -eq "ResourceGroup")
              {
                  New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName "Disk Backup Reader" -Scope $ResourceRG  | Out-Null
              }

              elseif($PermissionsScope -eq "Subscription")
              {                   
                  New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName "Disk Backup Reader" -Scope $SubscriptionName  | Out-Null
              }

              Write-Host "Assigned Disk Backup Reader role on Disk"
          }
          $DiskSnapshotContributor =  $AllRoles
          | Where-Object { ($_.Scope -eq $SnapshotResourceGroupId -or $_.Scope -eq $SubscriptionName)  -and $_.RoleDefinitionName -eq "Disk Snapshot Contributor"}
          
          Write-Host "DiskSnapShotContributor = $($DiskSnapshotContributor | ConvertTo-Json -Depth 10)"
          if($DiskSnapshotContributor -ne $null)
          {
              Write-Host "Disk Snapshot Contributor already assigned on SnapshotRG" 
          }

          elseif ($DiskSnapshotContributor -eq $null)
          {
              Write-Host "Assigning Disk Snapshot Contributor on SnapshotRG"
              
              if($PermissionsScope -eq "Subscription")
              {New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName "Disk Snapshot Contributor" -Scope $SubscriptionName | Out-Null}             
              else
              {New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName "Disk Snapshot Contributor" -Scope $SnapshotResourceGroupId  | Out-Null}             
             
              Write-Host "Assigned Disk Snapshot Contributor on SnapshotRG"
          }
          #>
          


          #if($DatasourceType -eq "Blobs")
          #{
              <#$StorageAccountBackupContributor =  $AllRoles
              | Where-Object { ($_.Scope -eq $DataSourceId -or $_.Scope -eq $ResourceRG -or  $_.Scope -eq $SubscriptionName) -and $_.RoleDefinitionName -eq "Storage Account Backup Contributor"}
  
              if($StorageAccountBackupContributor -ne $null)
              { 
                  Write-Host "StorageAccountBackupContributor already assigned on Storage Account"
              }
                            
              else
              {
                  if($PermissionsScope -eq "Resource")
                  {New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName "Storage Account Backup Contributor" -Scope $DatasourceId | Out-Null}
              
                  elseif($PermissionsScope -eq "ResourceGroup")
                  {New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName "Storage Account Backup Contributor" -Scope $ResourceRG | Out-Null}

                  elseif($PermissionsScope -eq "Subscription")
                  {New-AzRoleAssignment -ObjectId $vault.IdentityPrincipalId -RoleDefinitionName "Storage Account Backup Contributor" -Scope $SubscriptionName | Out-Null}
              }
              #>
              
           #}
          
          
    }
}