@{
  GUID = 'ba5feff5-66db-4569-bb06-1aa44aeb1d8b'
  RootModule = './Az.DataProtection.psm1'
  ModuleVersion = '0.1.0'
  CompatiblePSEditions = 'Core', 'Desktop'
  Author = 'Microsoft Corporation'
  CompanyName = 'Microsoft Corporation'
  Copyright = 'Microsoft Corporation. All rights reserved.'
  Description = 'Microsoft Azure PowerShell: DataProtection cmdlets'
  PowerShellVersion = '5.1'
  DotNetFrameworkVersion = '4.7.2'
  RequiredAssemblies = './bin/Az.DataProtection.private.dll'
  FormatsToProcess = './Az.DataProtection.format.ps1xml'
  FunctionsToExport = 'Backup-AzDataProtectionBackupInstanceAdhoc', 'Edit-AzDataProtectionPolicyRetentionRuleClientObject', 'Edit-AzDataProtectionPolicyTagClientObject', 'Edit-AzDataProtectionPolicyTriggerClientObject', 'Find-AzDataProtectionRestorableTimeRange', 'Get-AzDataProtectionBackupInstance', 'Get-AzDataProtectionBackupInstanceOperationResult', 'Get-AzDataProtectionBackupPolicy', 'Get-AzDataProtectionBackupVault', 'Get-AzDataProtectionJob', 'Get-AzDataProtectionOperation', 'Get-AzDataProtectionOperationStatusBackupVaultContext', 'Get-AzDataProtectionOperationStatusResourceGroupContext', 'Get-AzDataProtectionPolicyTemplate', 'Get-AzDataProtectionRecoveryPoint', 'Get-AzDataProtectionResourceGuard', 'Initialize-AzDataProtectionBackupInstance', 'Initialize-AzDataProtectionRestoreRequest', 'New-AzDataProtectionBackupInstance', 'New-AzDataProtectionBackupPolicy', 'New-AzDataProtectionBackupVault', 'New-AzDataProtectionBackupVaultStorageSettingObject', 'New-AzDataProtectionPolicyTagCriteriaClientObject', 'New-AzDataProtectionPolicyTriggerScheduleClientObject', 'New-AzDataProtectionResourceGuard', 'New-AzDataProtectionRetentionLifeCycleClientObject', 'Remove-AzDataProtectionBackupInstance', 'Remove-AzDataProtectionBackupPolicy', 'Remove-AzDataProtectionBackupVault', 'Remove-AzDataProtectionResourceGuard', 'Resume-AzDataProtectionBackupInstanceBackup', 'Resume-AzDataProtectionBackupInstanceProtection', 'Search-AzDataProtectionBackupInstanceInAzGraph', 'Search-AzDataProtectionJobInAzGraph', 'Start-AzDataProtectionBackupInstanceRestore', 'Stop-AzDataProtectionBackupInstanceProtection', 'Suspend-AzDataProtectionBackupInstanceBackup', 'Sync-AzDataProtectionBackupInstance', 'Test-AzDataProtectionBackupInstance', 'Test-AzDataProtectionBackupInstanceRestore', 'Test-AzDataProtectionBackupVaultNameAvailability', 'Test-AzDataProtectionFeatureSupport', 'Update-AzDataProtectionBackupInstanceAssociatedPolicy', 'Update-AzDataProtectionBackupVault', 'Update-AzDataProtectionResourceGuard', '*'
  AliasesToExport = '*'
  PrivateData = @{
    PSData = @{
      Tags = 'Azure', 'ResourceManager', 'ARM', 'PSModule', 'DataProtection'
      LicenseUri = 'https://aka.ms/azps-license'
      ProjectUri = 'https://github.com/Azure/azure-powershell'
      ReleaseNotes = ''
    }
  }
}
