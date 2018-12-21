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
using Microsoft.Azure.Commands.RecoveryServices.Backup.Helpers;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Properties;
using Microsoft.Azure.Management.RecoveryServices.Backup.Models;
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
            string vaultName = (string)ProviderData[VaultParams.VaultName];
            string vaultResourceGroupName = (string)ProviderData[VaultParams.ResourceGroupName];
            bool deleteBackupData = ProviderData.ContainsKey(ItemParams.DeleteBackupData) ?
                (bool)ProviderData[ItemParams.DeleteBackupData] : false;

            ItemBase itemBase = (ItemBase)ProviderData[ItemParams.Item];

            AzureWorkloadSQLDatabaseProtectedItem item = (AzureWorkloadSQLDatabaseProtectedItem)ProviderData[ItemParams.Item];
            string containerUri = "";
            string protectedItemUri = "";
            AzureVmWorkloadSQLDatabaseProtectedItem properties = new AzureVmWorkloadSQLDatabaseProtectedItem();

            if (deleteBackupData)
            {
                //Disable protection and delete backup data
                ValidateAzureWorkloadSQLDatabaseDisableProtectionRequest(itemBase);

                Dictionary<UriEnums, string> keyValueDict = HelperUtils.ParseUri(item.Id);
                containerUri = HelperUtils.GetContainerUri(keyValueDict, item.Id);
                protectedItemUri = HelperUtils.GetProtectedItemUri(keyValueDict, item.Id);

                return ServiceClientAdapter.DeleteProtectedItem(
                                    containerUri,
                                    protectedItemUri,
                                    vaultName: vaultName,
                                    resourceGroupName: vaultResourceGroupName);
            }
            else
            {
                return EnableOrModifyProtection(disableWithRetentionData: true);
            }
        }

        public RestAzureNS.AzureOperationResponse EnableProtection()
        {
            return EnableOrModifyProtection();
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
            string vaultName = (string)ProviderData[VaultParams.VaultName];
            string resourceGroupName = (string)ProviderData[VaultParams.ResourceGroupName];
            ContainerBase container =
                (ContainerBase)ProviderData[ItemParams.Container];
            string itemName = (string)ProviderData[ItemParams.ItemName];
            ItemProtectionStatus protectionStatus =
                (ItemProtectionStatus)ProviderData[ItemParams.ProtectionStatus];
            ItemProtectionState status =
                (ItemProtectionState)ProviderData[ItemParams.ProtectionState];
            CmdletModel.WorkloadType workloadType =
                (CmdletModel.WorkloadType)ProviderData[ItemParams.WorkloadType];
            PolicyBase policy = (PolicyBase)ProviderData[PolicyParams.ProtectionPolicy];

            // 1. Filter by container
            List<ProtectedItemResource> protectedItems = AzureWorkloadProviderHelper.ListProtectedItemsByContainer(
                vaultName,
                resourceGroupName,
                container,
                policy,
                ServiceClientModel.BackupManagementType.AzureWorkload,
                DataSourceType.SQLDataBase);

            List<ProtectedItemResource> protectedItemGetResponses =
                new List<ProtectedItemResource>();

            // 2. Filter by item name
            List<ItemBase> itemModels = AzureWorkloadProviderHelper.ListProtectedItemsByItemName(
                protectedItems,
                itemName,
                vaultName,
                resourceGroupName,
                (itemModel, protectedItemGetResponse) =>
                {
                    AzureWorkloadSQLDatabaseProtectedItemExtendedInfo extendedInfo = new AzureWorkloadSQLDatabaseProtectedItemExtendedInfo();
                    var serviceClientExtendedInfo = ((AzureVmWorkloadSQLDatabaseProtectedItem)protectedItemGetResponse.Properties).ExtendedInfo;
                    if (serviceClientExtendedInfo.OldestRecoveryPoint.HasValue)
                    {
                        extendedInfo.OldestRecoveryPoint = serviceClientExtendedInfo.OldestRecoveryPoint;
                    }
                    extendedInfo.PolicyState = serviceClientExtendedInfo.PolicyState.ToString();
                    extendedInfo.RecoveryPointCount =
                        (int)(serviceClientExtendedInfo.RecoveryPointCount.HasValue ?
                            serviceClientExtendedInfo.RecoveryPointCount : 0);
                    ((AzureWorkloadSQLDatabaseProtectedItem)itemModel).ExtendedInfo = extendedInfo;
                });

            // 3. Filter by item's Protection Status
            if (protectionStatus != 0)
            {
                itemModels = itemModels.Where(itemModel =>
                {
                    return ((AzureWorkloadSQLDatabaseProtectedItem)itemModel).ProtectionStatus == protectionStatus;
                }).ToList();
            }

            // 4. Filter by item's Protection State
            if (status != 0)
            {
                itemModels = itemModels.Where(itemModel =>
                {
                    return ((AzureWorkloadSQLDatabaseProtectedItem)itemModel).ProtectionState == status;
                }).ToList();
            }

            // 5. Filter by workload type
            if (workloadType != 0)
            {
                itemModels = itemModels.Where(itemModel =>
                {
                    return itemModel.WorkloadType == workloadType;
                }).ToList();
            }

            return itemModels;
        }

        public List<ContainerBase> ListProtectionContainers()
        {
            throw new NotImplementedException();
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

        private RestAzureNS.AzureOperationResponse EnableOrModifyProtection(bool disableWithRetentionData = false)
        {
            string vaultName = (string)ProviderData[VaultParams.VaultName];
            string vaultResourceGroupName = (string)ProviderData[VaultParams.ResourceGroupName];

            PolicyBase policy = ProviderData.ContainsKey(ItemParams.Policy) ?
                (PolicyBase)ProviderData[ItemParams.Policy] : null;

            ProtectableItemBase protectableItemBase = (ProtectableItemBase)ProviderData[ItemParams.ProtectableItem];

            AzureWorkloadProtectableItem protectableItem = (AzureWorkloadProtectableItem)ProviderData[ItemParams.ProtectableItem];

            string containerUri = "";
            string protectedItemUri = "";
            AzureVmWorkloadSQLDatabaseProtectedItem properties = new AzureVmWorkloadSQLDatabaseProtectedItem();

            if (protectableItemBase == null)
            {
                Dictionary<UriEnums, string> keyValueDict =
                    HelperUtils.ParseUri(protectableItem.Id);
                containerUri = HelperUtils.GetContainerUri(
                    keyValueDict, protectableItem.Id);
                protectedItemUri = HelperUtils.GetProtectableItemUri(
                    keyValueDict, protectableItem.Id);

                properties.PolicyId = policy.Id;
            }
            ProtectedItemResource serviceClientRequest = new ProtectedItemResource()
            {
                Properties = properties
            };

            return ServiceClientAdapter.CreateOrUpdateProtectedItem(
                containerUri,
                protectedItemUri,
                serviceClientRequest,
                vaultName: vaultName,
                resourceGroupName: vaultResourceGroupName);
        }

        private void ValidateAzureWorkloadSQLDatabaseDisableProtectionRequest(ItemBase itemBase)
        {

            if (itemBase == null || itemBase.GetType() != typeof(AzureWorkloadSQLDatabaseProtectedItem))
            {
                throw new ArgumentException(string.Format(Resources.InvalidProtectionPolicyException,
                                            typeof(AzureWorkloadSQLDatabaseProtectedItem).ToString()));
            }

            ValidateAzureVmWorkloadType(itemBase.WorkloadType);
            ValidateAzureVmContainerType(itemBase.ContainerType);
        }

        private void ValidateAzureVmWorkloadType(CmdletModel.WorkloadType type)
        {
            if (type != CmdletModel.WorkloadType.MSSQL)
            {
                throw new ArgumentException(string.Format(Resources.UnExpectedWorkLoadTypeException,
                                            CmdletModel.WorkloadType.MSSQL.ToString(),
                                            type.ToString()));
            }
        }

        private void ValidateAzureVmContainerType(CmdletModel.ContainerType type)
        {
            if (type != CmdletModel.ContainerType.AzureWorkload)
            {
                throw new ArgumentException(string.Format(Resources.UnExpectedContainerTypeException,
                                            CmdletModel.ContainerType.AzureWorkload.ToString(),
                                            type.ToString()));
            }
        }
    }
}