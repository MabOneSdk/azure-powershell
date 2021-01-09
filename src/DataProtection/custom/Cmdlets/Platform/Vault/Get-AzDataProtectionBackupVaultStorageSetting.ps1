﻿
function Get-AzDataProtectionBackupVaultStorageSetting{
	[OutputType('PSObject')]
    [CmdletBinding(PositionalBinding=$false, SupportsShouldProcess)]
    [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Description('Get Backup Vault storage setting object')]

    param(
        [Parameter(Mandatory, HelpMessage='Storage Type of the vault')]
        [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Support.StorageSettingType]
        ${Type},

        [Parameter(Mandatory, HelpMessage='DataStore Type of the vault')]
        [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Support.DataStoreType]
        ${DataStoreType}
    )

    process {
        $storageSetting = [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202001Alpha.StorageSetting]::new()
        $storageSetting.Type = $Type
        $storageSetting.DataStoreType = $DataStoreType

        $storageSetting
    }
}