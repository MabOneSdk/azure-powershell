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

function Test-AzureFileContainer
{
	$location = "southeastasia"
	$resourceGroupName = "sam-rg-sea-can"
	$vaultName = "sam-rv-sea-can"

	try
	{
		$vault = Get-AzureRmRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		$containers = Get-AzureRmRecoveryServicesBackupContainer -VaultId $vault.ID -ContainerType AzureStorage -Status Registered -Name "nilshaseacan"
	}
	finally
	{
		# Cleanup
	}
}

function Test-AzureFileUnregisterContainer
{
	$location = "westus"
	$resourceGroupName = "sisi-RSV"
	$vaultName = "sisi-RSV-29-6"

	try
	{
		$vault = Get-AzureRmRecoveryServicesVault -ResourceGroupName $resourceGroupName -Name $vaultName
		$containers = Get-AzureRmRecoveryServicesBackupContainer -VaultId $vault.ID -ContainerType AzureStorage -Status Registered -FriendlyName  "sisitestaccount"
		Unregister-AzureRmRecoveryServicesBackupContainer -VaultId $vault.ID -Container $containers[0]
	}
	finally
	{
		# Cleanup
	}
}