﻿# ----------------------------------------------------------------------------------
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

$location = "westus"
$resourceGroupName = "sisi-RSV"
$vaultName = "sisi-RSV-29-6"
$newPolicyName = "newSqlPolicy"

function Test-AzureVmWorkloadPolicy
{
	$vault = Get-AzureRmRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		
	# Get default policy objects
	$schedulePolicy = Get-AzureRmRecoveryServicesBackupSchedulePolicyObject -WorkloadType MSSQL
	Assert-NotNull $schedulePolicy
	$retentionPolicy = Get-AzureRmRecoveryServicesBackupRetentionPolicyObject -WorkloadType MSSQL
	Assert-NotNull $retentionPolicy

	# Create policy
	$policy = New-AzureRmRecoveryServicesBackupProtectionPolicy `
		-VaultId $vault.ID `
		-Name $newPolicyName `
		-WorkloadType MSSQL `
		-RetentionPolicy $retentionPolicy `
		-SchedulePolicy $schedulePolicy
	Assert-NotNull $policy
	Assert-AreEqual $policy.Name $newPolicyName

	# Get policy
	$policy = Get-AzureRmRecoveryServicesBackupProtectionPolicy `
		-VaultId $vault.ID `
		-Name $newPolicyName
	Assert-NotNull $policy
	Assert-AreEqual $policy.Name $newPolicyName

  # Get default policy objects (this data is generated partially at random. So, running this again gives different values)
  $schedulePolicy = Get-AzureRmRecoveryServicesBackupSchedulePolicyObject -WorkloadType MSSQL
  $schedulePolicy.FullBackupSchedulePolicy.ScheduleRunFrequency = "Weekly"
  $schedulePolicy.IsDifferentialBackupEnabled = $true
  $schedulePolicy.IsCompression = $true
  Assert-NotNull $schedulePolicy
  
  $retentionPolicy = Get-AzureRmRecoveryServicesBackupRetentionPolicyObject -WorkloadType MSSQL
  $retentionPolicy.FullBackupRetentionPolicy.IsDailyScheduleEnabled = $false
  $retentionPolicy.DifferentialBackupRetentionPolicy.RetentionCount = 31
  Assert-NotNull $retentionPolicy

	# Update policy
	Set-AzureRmRecoveryServicesBackupProtectionPolicy `
		-VaultId $vault.ID `
		-RetentionPolicy $retentionPolicy `
		-SchedulePolicy $schedulePolicy `
		-Policy $policy
	$policy = Get-AzureRmRecoveryServicesBackupProtectionPolicy `
		-VaultId $vault.ID `
		-Name $newPolicyName
	Assert-AreEqual $policy.DifferentialBackupRetentionPolicy.RetentionCount $retentionPolicy.DifferentialBackupRetentionPolicy.RetentionCount
  Assert-AreEqual $policy.IsCompression $schedulePolicy.IsCompression

  # Delete policy
  Remove-AzureRmRecoveryServicesBackupProtectionPolicy `
	  -VaultId $vault.ID `
	  -Policy $policy `
	  -Force
  $policy = Get-AzureRmRecoveryServicesBackupProtectionPolicy `
	  -VaultId $vault.ID `
	  -WorkloadType MSSQL
  Assert-False { $policy.Name -contains $newPolicyName }
}