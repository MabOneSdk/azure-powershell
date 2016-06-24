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

$resourceGroupName = "labRG1";
$resourceName = "pstestrsvault";
$vaultLocation = "westus";
$policyName = "pwtest1";

function Test-PolicyScenario
{
	# 1. Create / update and get vault
	$vault = New-AzureRmRecoveryServicesVault -Name $resourceName -ResourceGroupName $resourceGroupName -Location $vaultLocation;
	
	# 2. Set vault context
	Set-AzureRmRecoveryServicesVaultContext -Vault $vault;

	# get default objects
	$schedulePolicy = Get-AzureRmRecoveryServicesBackupSchedulePolicyObject -WorkloadType "AzureVM"
	Assert-NotNull $schedulePolicy
	$retPolicy = Get-AzureRmRecoveryServicesBackupRetentionPolicyObject -WorkloadType "AzureVM"
	Assert-NotNull $retPolicy

	# now create new policy
<<<<<<< HEAD
	$policy = New-AzureRmRecoveryServicesBackupProtectionPolicy -Name $policyName -WorkloadType "AzureVM" -RetentionPolicy $retPolicy -SchedulePolicy $schedulePolicy
	Assert-NotNull $policy
	Assert-AreEqual $policy.Name $policyName
=======
	$policy = New-AzureRmRecoveryServicesBackupProtectionPolicy -Name "pwtest1" -WorkloadType "AzureVM" -RetentionPolicy $retPolicy -SchedulePolicy $schedulePolicy
	Assert-NotNull $policy
	Assert-AreEqual $policy.Name "pwtest1"
>>>>>>> 99bbde85768e4aa70311e268685a49ac8ce3328b
		
	# now get policy and update it with new schedule/retention
	$schedulePolicy = Get-AzureRmRecoveryServicesBackupSchedulePolicyObject -WorkloadType "AzureVM"
	Assert-NotNull $schedulePolicy
	$retPolicy = Get-AzureRmRecoveryServicesBackupRetentionPolicyObject -WorkloadType "AzureVM"
	Assert-NotNull $retPolicy
<<<<<<< HEAD

    $temp = Get-AzureRmRecoveryServicesBackupProtectionPolicy -Name $policyName	
	Assert-NotNull $temp
	Assert-AreEqual $temp.Name $policyName

=======

    $temp = Get-AzureRmRecoveryServicesBackupProtectionPolicy -Name "pwtest1"	
	Assert-NotNull $temp
	Assert-AreEqual $temp.Name "pwtest1"

>>>>>>> 99bbde85768e4aa70311e268685a49ac8ce3328b
	Set-AzureRmRecoveryServicesBackupProtectionPolicy -RetentionPolicy $retPolicy -SchedulePolicy $schedulePolicy -Policy $temp	

	#cleanup 
	Remove-AzureRmRecoveryServicesBackupProtectionPolicy -Policy $temp -Force
}