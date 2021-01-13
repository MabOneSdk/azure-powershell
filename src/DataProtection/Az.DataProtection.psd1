@{
  GUID = '0796af7c-0a7c-417f-8d0d-19f9179dac7a'
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
  FunctionsToExport = 'Backup-AzDataProtectionBackupInstanceAdhoc', 'Get-AzDataProtectionBackupInstance', 'Get-AzDataProtectionBackupPolicy', 'Get-AzDataProtectionBackupVault', 'Get-AzDataProtectionBackupVaultResource', 'Get-AzDataProtectionBackupVaultStorageSetting', 'Get-AzDataProtectionExportJobsOperationResult', 'Get-AzDataProtectionJob', 'Get-AzDataProtectionOperationResult', 'Get-AzDataProtectionOperationStatus', 'Get-AzDataProtectionPolicyTemplate', 'Get-AzDataProtectionRecoveryPoint', 'Get-AzDataProtectionRecoveryPointList', 'Get-AzDataProtectionResourceOperationGatekeeper', 'Get-AzDataProtectionResourceOperationGatekeeperResource', 'Get-AzDataProtectionResourceOperationResultPatch', 'GetBackupFrequencyString', 'GetTaggingPriority', 'LoadManifest', 'New-AzDataProtectionPolicyTagCriteria', 'New-AzDataProtectionPolicyTriggerSchedule', 'New-AzDataProtectionRetentionLifeCycle', 'Prepare-AzDataProtectionDatasourceInfo', 'Remove-AzDataProtectionBackupInstance', 'Remove-AzDataProtectionBackupPolicy', 'Remove-AzDataProtectionBackupVault', 'Remove-AzDataProtectionResourceOperationGatekeeper', 'Set-AzDataProtectionBackupInstance', 'Set-AzDataProtectionBackupPolicy', 'Set-AzDataProtectionBackupVault', 'Set-AzDataProtectionResourceOperationGatekeeper', 'Start-AzDataProtectionBackupInstanceRehydrate', 'Start-AzDataProtectionBackupInstanceRestore', 'Start-AzDataProtectionExportJob', 'Test-AzDataProtectionBackupInstance', 'Test-AzDataProtectionBackupInstanceRestore', 'Test-AzDataProtectionBackupVaultNameAvailability', 'TranslateBackupParam', 'TranslateBackupPolicy', 'TranslateBackupPolicyRule', 'TranslateBackupPolicyTagCriteria', 'TranslateBackupPolicyTrigger', 'TranslatePolicyRetentionLifeCycle', 'Update-AzDataProtectionBackupVault', 'Update-AzDataProtectionPolicyRetentionRule', 'Update-AzDataProtectionPolicyTag', 'Update-AzDataProtectionPolicyTrigger', 'Update-AzDataProtectionResourceOperationGatekeeper', '*'
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
