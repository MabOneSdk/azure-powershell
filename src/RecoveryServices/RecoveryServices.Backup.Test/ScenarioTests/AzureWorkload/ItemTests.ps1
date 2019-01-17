# ----------------------------------------------------------------------------------
#
# Copyright Microsoft Corporation
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
# http://www.apache.org/licenses/LICENSE-2.0
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# ----------------------------------------------------------------------------------

$containerName = "pstestwlvm1bca8"
$resourceGroupName = "pstestwlRG1bca8"
$vaultName = "pstestwlRSV1bca8"
$resourceId = "/subscriptions/da364f0f-307b-41c9-9d47-b7413ec45535/resourceGroups/pstestwlRG1bca8/providers/Microsoft.Compute/virtualMachines/pstestwlvm1bca8"
$policyName = "HourlyLogBackup"

function Test-AzureVmWorkloadProtectableItem
{
	try
	{
		$vault = Get-AzRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName

		$container = Register-AzRecoveryServicesBackupContainer `
			-ResourceId $resourceId `
			-BackupManagementType AzureWorkload `
			-WorkloadType MSSQL `
			-VaultId $vault.ID
		Assert-AreEqual $container.Status "Registered"
		
		$protectableItems = Get-AzRecoveryServicesBackupProtectableItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType "MSSQL";
		Assert-True { $protectableItems.ServerName -contains $containerName }

		$protectableItems = Get-AzRecoveryServicesBackupProtectableItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType "MSSQL" `
			-ItemType "SQLInstance";
		Assert-True { $protectableItems.ProtectableItemType -contains "SQLInstance" }
		Assert-False { $protectableItems.ProtectableItemType -contains "SQLDataBase" }
		
		$protectableItems = Get-AzRecoveryServicesBackupProtectableItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType "MSSQL" `
			-ItemType "SQLDataBase";
		Assert-True { $protectableItems.ProtectableItemType -contains "SQLDataBase" }
		Assert-False { $protectableItems.ProtectableItemType -contains "SQLInstance" }
	}
	finally
	{
		#Unregister container
		Unregister-AzRecoveryServicesBackupContainer `
			-VaultId $vault.ID `
			-Container $container
	}
}

function Test-AzureVmWorkloadInitializeProtectableItem
{
	try
	{
		$vault = Get-AzRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		
		$container = Register-AzRecoveryServicesBackupContainer `
			-ResourceId $resourceId `
			-BackupManagementType AzureWorkload `
			-WorkloadType MSSQL `
			-VaultId $vault.ID
		Assert-AreEqual $container.Status "Registered";

		$protectableItems = Get-AzRecoveryServicesBackupProtectableItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType "MSSQL";
		Assert-True { $protectableItems.ServerName -contains $containerName }

		Initialize-AzRecoveryServicesBackupProtectableItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType "MSSQL";

		$newprotectableItems = Get-AzRecoveryServicesBackupProtectableItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType "MSSQL";
		Assert-True { $newprotectableItems.count -ge $protectableItems.count }
	}
	finally
	{
		#Unregister container
		Unregister-AzRecoveryServicesBackupContainer `
			-VaultId $vault.ID `
			-Container $container
	}
}

function Test-AzureVmWorkloadEnableProtectableItem
{
	try
	{
		$vault = Get-AzRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		
		$policy = Get-AzRecoveryServicesBackupProtectionPolicy `
			-VaultId $vault.ID `
			-Name $policyName
		
		$container = Register-AzRecoveryServicesBackupContainer `
			-ResourceId $resourceId `
			-BackupManagementType AzureWorkload `
			-WorkloadType MSSQL `
			-VaultId $vault.ID
		Assert-AreEqual $container.Status "Registered";

		$protectableItems = Get-AzRecoveryServicesBackupProtectableItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType "MSSQL";

		Enable-AzRecoveryServicesBackupProtection `
			-VaultId $vault.ID `
			-Policy $policy `
			-ProtectableItem $protectableItems[1]

		$item = Get-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType MSSQL;
		Assert-True { $item.Name -contains $protectableItems[1].Name }
		Assert-True { $item.LastBackupStatus -eq "IRPending" }
		Assert-True { $item.ProtectionPolicyName -eq $policyName }
	}
	finally
	{
		Cleanup-Vault $vault $item $container
	}
}

function Test-AzureVmWorkloadBackupProtectionItem
{
	try
	{
		$vault = Get-AzRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		$container = Register-AzRecoveryServicesBackupContainer `
			-ResourceId $resourceId `
			-BackupManagementType AzureWorkload `
			-WorkloadType MSSQL `
			-VaultId $vault.ID

		Enable-Protection $vault $container

		$item = Get-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType MSSQL;

		$backupJob = Backup-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-Item $item `
			-BackupType "Full" | Wait-AzRecoveryServicesBackupJob -VaultId $vault.ID

		Assert-True { $backupJob.Status -eq "Completed" }
	}
	finally
	{
		Cleanup-Vault $vault $item $container
	}
}

function Test-AzureVmWorkloadGetRPs
{
	try
	{
		# Test 1: Get latest recovery point; should be only one
		$vault = Get-AzRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		$container = Register-AzRecoveryServicesBackupContainer `
			-ResourceId $resourceId `
			-BackupManagementType AzureWorkload `
			-WorkloadType MSSQL `
			-VaultId $vault.ID

		Enable-Protection $vault $container

		$item = Get-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType MSSQL;

		$backupJob = Backup-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-Item $item `
			-BackupType "Full" | Wait-AzRecoveryServicesBackupJob -VaultId $vault.ID

		$backupStartTime = $backupJob.StartTime.AddMinutes(-1);
		$backupEndTime = $backupJob.EndTime.AddMinutes(1);

		$recoveryPoint = Get-AzRecoveryServicesBackupRecoveryPoint `
			-VaultId $vault.ID `
			-StartDate $backupStartTime `
			-EndDate $backupEndTime `
			-Item $item;
	
		Assert-NotNull $recoveryPoint[0];
		Assert-True { $recoveryPoint[0].Id -match $item.Id };

		# Test 2: Get Recovery point detail
		$recoveryPointDetail = Get-AzRecoveryServicesBackupRecoveryPoint `
			-VaultId $vault.ID `
			-RecoveryPointId $recoveryPoint[0].RecoveryPointId `
			-Item $item;
	
		Assert-NotNull $recoveryPointDetail;
	}
	finally
	{
		Cleanup-Vault $vault $item $container
	}
}

function Test-AzureVmWorkloadGetLogChains
{
	try
	{
		# Test 1: Get latest recovery point; should be only one
		$vault = Get-AzRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		$container = Register-AzRecoveryServicesBackupContainer `
			-ResourceId $resourceId `
			-BackupManagementType AzureWorkload `
			-WorkloadType MSSQL `
			-VaultId $vault.ID

		$policy = Get-AzRecoveryServicesBackupProtectionPolicy `
			-VaultId $vault.ID `
			-Name $policyName

		$protectableItems = Get-AzRecoveryServicesBackupProtectableItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType "MSSQL";

		Enable-AzRecoveryServicesBackupProtection `
			-VaultId $vault.ID `
			-Policy $policy `
			-ProtectableItem $protectableItems[4]

		$item = Get-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType MSSQL;

		$backupJob = Backup-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-Item $item `
			-BackupType "Full" | Wait-AzRecoveryServicesBackupJob -VaultId $vault.ID

		$backupJob2 = Backup-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-Item $item `
			-BackupType "Log" | Wait-AzRecoveryServicesBackupJob -VaultId $vault.ID

		$backupStartTime = $backupJob2.StartTime.AddMinutes(-10);
		$backupEndTime = $backupJob2.EndTime.AddMinutes(10);

		$recoveryLogChain = Get-AzRecoveryServicesBackupRecoveryLogChain `
			-VaultId $vault.ID `
			-StartDate $backupStartTime `
			-EndDate $backupEndTime `
			-Item $item;
	
		Assert-NotNull $recoveryLogChain[0];
	}
	finally
	{
		Cleanup-Vault $vault $item $container
	}
}

function Test-AzureVmWorkloadAutoProtection
{
	try
	{
		$vault = Get-AzRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		
		$container = Get-AzRecoveryServicesBackupContainer `
			-VaultId $vault.ID `
			-ContainerType AzureWorkload `
			-Status Registered `
			-FriendlyName $containerName `
			-ResourceGroupName $resourceGroupName;

		$protectableItems = Get-AzRecoveryServicesBackupProtectableItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType "MSSQL" `
			-ItemType "SQLInstance";

		Enable-AzRecoveryServicesBackupAutoProtection `
			-VaultId $vault.ID `
			-InputItem $protectableItems[0].Id `
			-BackupManagementType "AzureWorkload" `
			-WorkloadType "MSSQL"
	}
	finally
	{
	
	}
}

function Test-AzureVmWorkloadFullRestore
{
	try
	{
		$vault = Get-AzRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		$policy = Get-AzRecoveryServicesBackupProtectionPolicy `
			-VaultId $vault.ID `
			-Name $policyName
		
		$container = Get-AzRecoveryServicesBackupContainer `
			-VaultId $vault.ID `
			-ContainerType AzureWorkload `
			-Status Registered `
			-FriendlyName $containerName `
			-ResourceGroupName $resourceGroupName;
		
		$items = Get-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-Container $container `
			-WorkloadType MSSQL;

		$recoveryPoint = Get-AzRecoveryServicesBackupRecoveryPoint `
			-VaultId $vault.ID `
			-Item $items[1];

		$restoreConfig = Get-AzRecoveryServicesBackupWorkloadRecoveryConfig `
			-VaultId $vault.ID `
			-RecoveryPoint $recoveryPoint[0] `
			-TargetItem $items[0] `
			–AlternateWorkloadRestore;

		Assert-NotNull $restoreConfig

		Restore-AzRecoveryServicesBackupItem `
			-VaultId $vault.ID `
			-WLRecoveryConfig $restoreConfig
	}
	finally
	{
	
	}
}