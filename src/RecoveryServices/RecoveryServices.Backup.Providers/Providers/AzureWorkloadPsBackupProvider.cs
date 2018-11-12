// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.Models;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.ServiceClientAdapterNS;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Properties;
using Microsoft.Azure.Management.RecoveryServices.Backup.Models;
using Microsoft.Rest.Azure.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using CmdletModel = Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.Models;
using RestAzureNS = Microsoft.Rest.Azure;
using ServiceClientModel = Microsoft.Azure.Management.RecoveryServices.Backup.Models;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.ProviderModel
{
    /// <summary>
    /// This class implements methods for azure DB Workload backup provider
    /// </summary>
    public class AzureWorkloadPsBackupProvider : IPsBackupProvider
    {
        private const int defaultOperationStatusRetryTimeInMilliSec = 5 * 1000; // 5 sec
        private const string separator = ";";
        private const CmdletModel.RetentionDurationType defaultFileRetentionType =
            CmdletModel.RetentionDurationType.Days;
        private const int defaultFileRetentionCount = 30;

        Dictionary<Enum, object> ProviderData { get; set; }

        ServiceClientAdapter ServiceClientAdapter { get; set; }

        AzureWorkloadProviderHelper AzureWorkloadProviderHelper { get; set; }

        /// <summary>
        /// Initializes the provider with the data recieved from the cmdlet layer
        /// </summary>
        /// <param name="providerData">Data from the cmdlet layer intended for the provider</param>
        /// <param name="serviceClientAdapter">Service client adapter for communicating with the backend service</param>
        public void Initialize(Dictionary<Enum, object> providerData, ServiceClientAdapter serviceClientAdapter)
        {
            ProviderData = providerData;
            ServiceClientAdapter = serviceClientAdapter;
            AzureWorkloadProviderHelper = new AzureWorkloadProviderHelper(ServiceClientAdapter);
        }

        public ResourceBackupStatus CheckBackupStatus()
        {
            throw new NotImplementedException();
        }

        public ProtectionPolicyResource CreatePolicy()
        {
            throw new NotImplementedException();
        }

        public RestAzureNS.AzureOperationResponse DisableProtection()
        {
            throw new NotImplementedException();
        }

        public RestAzureNS.AzureOperationResponse EnableProtection()
        {
            throw new NotImplementedException();
        }

        public RetentionPolicyBase GetDefaultRetentionPolicyObject()
        {
            throw new NotImplementedException();
        }

        public SchedulePolicyBase GetDefaultSchedulePolicyObject()
        {
            throw new NotImplementedException();
        }

        public ProtectedItemResource GetProtectedItem()
        {
            throw new NotImplementedException();
        }

        public RecoveryPointBase GetRecoveryPointDetails()
        {
            throw new NotImplementedException();
        }

        public List<CmdletModel.BackupEngineBase> ListBackupManagementServers()
        {
            throw new NotImplementedException();
        }

        public List<ItemBase> ListProtectedItems()
        {
            throw new NotImplementedException();
        }

        public List<ContainerBase> ListProtectionContainers()
        {
            CmdletModel.BackupManagementType? backupManagementTypeNullable =
                (CmdletModel.BackupManagementType?)
                    ProviderData[ContainerParams.BackupManagementType];

            if (backupManagementTypeNullable.HasValue)
            {
                ValidateAzureWorkloadBackupManagementType(backupManagementTypeNullable.Value);
            }

            return AzureWorkloadProviderHelper.ListProtectionContainers(
                ProviderData,
                ServiceClientModel.BackupManagementType.AzureWorkload);
        }

        public List<RecoveryPointBase> ListRecoveryPoints()
        {
            throw new NotImplementedException();
        }

        public RestAzureNS.AzureOperationResponse<ProtectionPolicyResource> ModifyPolicy()
        {
            throw new NotImplementedException();
        }

        public RPMountScriptDetails ProvisionItemLevelRecoveryAccess()
        {
            throw new NotImplementedException();
        }

        public void RevokeItemLevelRecoveryAccess()
        {
            throw new NotImplementedException();
        }

        public RestAzureNS.AzureOperationResponse TriggerBackup()
        {
            throw new NotImplementedException();
        }

        public RestAzureNS.AzureOperationResponse TriggerRestore()
        {
            throw new NotImplementedException();
        }

        private void ValidateAzureWorkloadBackupManagementType(
            CmdletModel.BackupManagementType backupManagementType)
        {
            if (backupManagementType != CmdletModel.BackupManagementType.AzureWorkload)
            {
                throw new ArgumentException(string.Format(Resources.UnExpectedBackupManagementTypeException,
                                            CmdletModel.BackupManagementType.AzureWorkload.ToString(),
                                            backupManagementType.ToString()));
            }
        }

        public void RegisterContainer()
        {
            string vaultName = (string)ProviderData[VaultParams.VaultName];
            string vaultResourceGroupName = (string)ProviderData[VaultParams.ResourceGroupName];
            string containerName = (string)ProviderData[ContainerParams.Name];
            string backupManagementType = (string)ProviderData[ContainerParams.BackupManagementType];
            string workloadType = (string)ProviderData[ContainerParams.ContainerType];

            //Trigger Discovery
            ODataQuery<BMSRefreshContainersQueryObject> queryParam = new ODataQuery<BMSRefreshContainersQueryObject>(
               q => q.BackupManagementType
                    == ServiceClientModel.BackupManagementType.AzureWorkload);
            AzureWorkloadProviderHelper.RefreshContainer(vaultName, vaultResourceGroupName, queryParam);

            List<ProtectableContainerResource> unregisteredVmContainers =
                    GetUnRegisteredVmContainers(vaultName, vaultResourceGroupName);
            ProtectableContainerResource unregisteredVmContainer = unregisteredVmContainers.Find(
                vmContainer => string.Compare(vmContainer.Name.Split(';').Last(),
                containerName, true) == 0);

            if (unregisteredVmContainer != null)
            {
                ProtectionContainerResource protectionContainerResource =
                        new ProtectionContainerResource(unregisteredVmContainer.Id,
                        unregisteredVmContainer.Name);
                AzureVMAppContainerProtectionContainer azureVMContainer = new AzureVMAppContainerProtectionContainer(
                    friendlyName: containerName,
                    backupManagementType: backupManagementType,
                    sourceResourceId: unregisteredVmContainer.Properties.ContainerId,
                    workloadType: workloadType.ToString());
                protectionContainerResource.Properties = azureVMContainer;

                AzureWorkloadProviderHelper.RegisterContainer(unregisteredVmContainer.Name,
                        protectionContainerResource,
                        vaultName,
                        vaultResourceGroupName);
            }
        }

        private List<ProtectableContainerResource> GetUnRegisteredVmContainers(string vaultName = null,
            string vaultResourceGroupName = null)
        {
            ODataQuery<BMSContainerQueryObject> queryParams = null;
            queryParams = new ODataQuery<BMSContainerQueryObject>(
                q => q.BackupManagementType == ServiceClientModel.BackupManagementType.AzureWorkload);

            var listResponse = ServiceClientAdapter.ListUnregisteredContainers(
                queryParams,
                vaultName: vaultName,
                resourceGroupName: vaultResourceGroupName);
            List<ProtectableContainerResource> containerModels = listResponse.ToList();

            return containerModels;
        }
    }
}