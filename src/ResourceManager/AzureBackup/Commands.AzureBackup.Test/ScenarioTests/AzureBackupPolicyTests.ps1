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

$ResourceGroupName = "backuprg"
$ResourceName = "backuprn"
$DataSourceType = "VM"
$Location = "SouthEast Asia"
$PolicyName = "Policy10";
$PolicyId = "c87bbada-6e1b-4db2-b76c-9062d28959a4";
$POName = "iaasvmcontainer;dev01testing;dev01testing"
$WorkloadType = "VM"
$RetentionType = "Days"
$ScheduleRunTimes =  "2015-06-13T20:30:00"
$ScheduleRunDays = "Monday"
$RetentionDuration = 30
$BackupType = "Full"
$ScheduleType = "Daily"

<#
.SYNOPSIS
Tests creating new resource group and a simple resource.
#>
function Test-GetAzureBackupProtectionPolicyTests
{
	$vault = Get-AzureBackupVault -Name $ResourceName;
	$protectionPolicies = Get-AzureBackupProtectionPolicy -vault $vault
	Assert-NotNull $protectionPolicies 'Protection Policies should not be null'
	foreach($protectionPolicy in $protectionPolicies)
	{
		Assert-NotNull $protectionPolicy.InstanceId 'InstanceId should not be null'
		Assert-NotNull $protectionPolicy.Name 'Name should not be null'
		Assert-NotNull $protectionPolicy.WorkloadType 'WorkloadType should not be null'
		Assert-NotNull $protectionPolicy.ScheduleRunTimes 'ScheduleRunTimes should not be null'
		Assert-NotNull $protectionPolicy.RetentionPolicyList 'RetentionPolicyList should not be null'
		Assert-NotNull $protectionPolicy.ResourceGroupName 'ResourceGroupName should not be null'
		Assert-NotNull $protectionPolicy.ResourceName 'ResourceName should not be null'
		Assert-NotNull $protectionPolicy.Location 'Location should not be null'
	}
}

function Test-GetAzureBackupProtectionPolicyByNameTests
{
	$vault = Get-AzureBackupVault -Name $ResourceName;
	$protectionPolicy = Get-AzureBackupProtectionPolicy -vault $vault -Name $PolicyName
	
	Assert-NotNull $protectionPolicy.InstanceId 'InstanceId should not be null'
	Assert-NotNull $protectionPolicy.Name 'Name should not be null'
	Assert-NotNull $protectionPolicy.WorkloadType 'WorkloadType should not be null'
	Assert-NotNull $protectionPolicy.ScheduleRunTimes 'ScheduleRunTimes should not be null'
	Assert-NotNull $protectionPolicy.RetentionPolicyList 'RetentionPolicyList should not be null'
	Assert-NotNull $protectionPolicy.ResourceGroupName 'ResourceGroupName should not be null'
	Assert-NotNull $protectionPolicy.ResourceName 'ResourceName should not be null'
	Assert-NotNull $protectionPolicy.Location 'Location should not be null'
	
}

function Test-NewAzureBackupProtectionPolicyTests
{	
	$vault = Get-AzureBackupVault -Name $ResourceName;
	$r1 = New-AzureBackupRetentionPolicyObject -DailyRetention -Retention 20
	$r2 = New-AzureBackupRetentionPolicyObject -WeeklyRetention -DaysOfWeek "Monday" -Retention 10
	$r = ($r1, $r2)

	$protectionPolicy = New-AzureBackupProtectionPolicy -vault $vault -Name $PolicyName -WorkloadType $WorkloadType -Daily -RetentionPolicies $r -ScheduleRunTimes $ScheduleRunTimes
	
	Assert-NotNull $protectionPolicy.InstanceId 'InstanceId should not be null'
	Assert-NotNull $protectionPolicy.Name 'Name should not be null'
	Assert-NotNull $protectionPolicy.WorkloadType 'WorkloadType should not be null'
	Assert-NotNull $protectionPolicy.ScheduleRunTimes 'ScheduleRunTimes should not be null'
	Assert-NotNull $protectionPolicy.RetentionPolicyList 'RetentionPolicyList should not be null'
	Assert-NotNull $protectionPolicy.ResourceGroupName 'ResourceGroupName should not be null'
	Assert-NotNull $protectionPolicy.ResourceName 'ResourceName should not be null'
	Assert-NotNull $protectionPolicy.Location 'Location should not be null'
}

function Test-SetAzureBackupProtectionPolicyTests
{	
	$vault = Get-AzureBackupVault -Name $ResourceName;
	$protectionPolicy = Get-AzureBackupProtectionPolicy -vault $vault -Name $PolicyName
	$policyNewName = "policy09_new"
	
	Set-AzureBackupProtectionPolicy -ProtectionPolicy $protectionPolicy -NewName $policyNewName	
}

function Test-RemoveAzureBackupProtectionPolicyTests
{	
	$vault = Get-AzureBackupVault -Name $ResourceName;
	$protectionPolicy = Get-AzureBackupProtectionPolicy -vault $vault -Name $PolicyName
	
	Remove-AzureBackupProtectionPolicy -ProtectionPolicy $protectionPolicy
}
