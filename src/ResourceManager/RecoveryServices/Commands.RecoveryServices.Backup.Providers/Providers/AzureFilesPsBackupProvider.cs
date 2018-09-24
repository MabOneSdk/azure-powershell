﻿// ----------------------------------------------------------------------------------
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
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using Microsoft.Azure.Management.RecoveryServices.Backup.Models;
using Microsoft.Rest.Azure.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using CmdletModel = Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.Models;
using RestAzureNS = Microsoft.Rest.Azure;
using ServiceClientModel = Microsoft.Azure.Management.RecoveryServices.Backup.Models;
using SystemNet = System.Net;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.ProviderModel
{
    /// <summary>
    /// This class implements methods for azure files backup provider
    /// </summary>
    public class AzureFilesPsBackupProvider : IPsBackupProvider
    {
        private const int defaultOperationStatusRetryTimeInMilliSec = 5 * 1000; // 5 sec
        private const string separator = ";";

        Dictionary<Enum, object> ProviderData { get; set; }

        ServiceClientAdapter ServiceClientAdapter { get; set; }

        AzureWorkloadProviderHelper AzureWorkloadProviderHelper { get; set; }

        /// <summary>
        /// Initializes the provider with the data recieved from the cmdlet layer
        /// </summary>
        /// <param name="providerData">Data from the cmdlet layer intended for the provider</param>
        /// <param name="serviceClientAdapter">Service client adapter for communicating with the backend service</param>
        public void Initialize(
            Dictionary<Enum, object> providerData, ServiceClientAdapter serviceClientAdapter)
        {
            ProviderData = providerData;
            ServiceClientAdapter = serviceClientAdapter;
            AzureWorkloadProviderHelper = new AzureWorkloadProviderHelper(ServiceClientAdapter);
        }

        /// <summary>
        /// Triggers the enable protection operation for the given item
        /// </summary>
        /// <returns>The job response returned from the service</returns>
        public RestAzureNS.AzureOperationResponse EnableProtection()
        {
            return EnableOrModifyProtection();
        }

        /// <summary>
        /// Triggers the disable protection operation for the given item
        /// </summary>
        /// <returns>The job response returned from the service</returns>
        public RestAzureNS.AzureOperationResponse DisableProtection()
        {
            string vaultName = (string)ProviderData[VaultParams.VaultName];
            string vaultResourceGroupName = (string)ProviderData[VaultParams.ResourceGroupName];
            bool deleteBackupData = ProviderData.ContainsKey(ItemParams.DeleteBackupData) ?
                (bool)ProviderData[ItemParams.DeleteBackupData] : false;

            ItemBase itemBase = (ItemBase)ProviderData[ItemParams.Item];

            AzureFileShareItem item = (AzureFileShareItem)ProviderData[ItemParams.Item];

            string containerUri = "";
            string protectedItemUri = "";
            AzureFileshareProtectedItem properties = new AzureFileshareProtectedItem();

            if (deleteBackupData)
            {
                ValidateAzureFileShareDisableProtectionRequest(itemBase);

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
                return EnableOrModifyProtection();
            }
        }

        public List<ContainerBase> ListProtectionContainers()
        {
            CmdletModel.BackupManagementType? backupManagementTypeNullable =
                (CmdletModel.BackupManagementType?)
                    ProviderData[ContainerParams.BackupManagementType];

            if (backupManagementTypeNullable.HasValue)
            {
                ValidateAzureStorageBackupManagementType(backupManagementTypeNullable.Value);
            }

            return AzureWorkloadProviderHelper.ListProtectionContainers(
                ProviderData,
                ServiceClientModel.BackupManagementType.AzureStorage);
        }

        public RestAzureNS.AzureOperationResponse TriggerBackup()
        {
            string vaultName = (string)ProviderData[VaultParams.VaultName];
            string vaultResourceGroupName = (string)ProviderData[VaultParams.ResourceGroupName];
            ItemBase item = (ItemBase)ProviderData[ItemParams.Item];
            DateTime? expiryDateTime = (DateTime?)ProviderData[ItemParams.ExpiryDateTimeUTC];
            AzureFileShareItem azureFileShareItem = item as AzureFileShareItem;
            BackupRequestResource triggerBackupRequest = new BackupRequestResource();
            AzureFileShareBackupRequest azureFileShareBackupRequest = new AzureFileShareBackupRequest();
            azureFileShareBackupRequest.RecoveryPointExpiryTimeInUTC = expiryDateTime;
            triggerBackupRequest.Properties = azureFileShareBackupRequest;

            return ServiceClientAdapter.TriggerBackup(
               IdUtils.GetValueByName(azureFileShareItem.Id, IdUtils.IdNames.ProtectionContainerName),
               IdUtils.GetValueByName(azureFileShareItem.Id, IdUtils.IdNames.ProtectedItemName),
               triggerBackupRequest,
               vaultName: vaultName,
               resourceGroupName: vaultResourceGroupName);
        }

        public RestAzureNS.AzureOperationResponse TriggerRestore()
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

        public List<RecoveryPointBase> ListRecoveryPoints()
        {
            throw new NotImplementedException();
        }

        public ProtectionPolicyResource CreatePolicy()
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

        public List<CmdletModel.BackupEngineBase> ListBackupManagementServers()
        {
            throw new NotImplementedException();
        }

        public ProtectionPolicyResource GetPolicy()
        {
            throw new NotImplementedException();
        }

        public void DeletePolicy()
        {
            throw new NotImplementedException();
        }

        public SchedulePolicyBase GetDefaultSchedulePolicyObject()
        {
            throw new NotImplementedException();
        }

        public RetentionPolicyBase GetDefaultRetentionPolicyObject()
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
                ServiceClientModel.BackupManagementType.AzureStorage,
                DataSourceType.AzureFileShare);

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
                    AzureFileShareItemExtendedInfo extendedInfo = new AzureFileShareItemExtendedInfo();
                    var serviceClientExtendedInfo = ((AzureFileshareProtectedItem)protectedItemGetResponse.Properties).ExtendedInfo;
                    if (serviceClientExtendedInfo.OldestRecoveryPoint.HasValue)
                    {
                        extendedInfo.OldestRecoveryPoint = serviceClientExtendedInfo.OldestRecoveryPoint;
                    }
                    extendedInfo.PolicyState = serviceClientExtendedInfo.PolicyState.ToString();
                    extendedInfo.RecoveryPointCount =
                        (int)(serviceClientExtendedInfo.RecoveryPointCount.HasValue ?
                            serviceClientExtendedInfo.RecoveryPointCount : 0);
                    ((AzureFileShareItem)itemModel).ExtendedInfo = extendedInfo;
                });

            // 3. Filter by item's Protection Status
            if (protectionStatus != 0)
            {
                itemModels = itemModels.Where(itemModel =>
                {
                    return ((AzureFileShareItem)itemModel).ProtectionStatus == protectionStatus;
                }).ToList();
            }

            // 4. Filter by item's Protection State
            if (status != 0)
            {
                itemModels = itemModels.Where(itemModel =>
                {
                    return ((AzureFileShareItem)itemModel).ProtectionState == status;
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

        private RestAzureNS.AzureOperationResponse EnableOrModifyProtection()
        {
            string vaultName = (string)ProviderData[VaultParams.VaultName];
            string vaultResourceGroupName = (string)ProviderData[VaultParams.ResourceGroupName];
            string azureFileShareName = ProviderData.ContainsKey(ItemParams.ItemName) ?
                (string)ProviderData[ItemParams.ItemName] : null;
            string storageAccountName = ProviderData.ContainsKey(ItemParams.StorageAccountName) ?
                (string)ProviderData[ItemParams.StorageAccountName] : null;
            string parameterSetName = ProviderData.ContainsKey(ItemParams.ParameterSetName) ?
                (string)ProviderData[ItemParams.ParameterSetName] : null;

            PolicyBase policy = ProviderData.ContainsKey(ItemParams.Policy) ?
                (PolicyBase)ProviderData[ItemParams.Policy] : null;
            bool deleteBackupData = ProviderData.ContainsKey(ItemParams.DeleteBackupData) ?
                (bool)ProviderData[ItemParams.DeleteBackupData] : false;

            ItemBase itemBase = (ItemBase)ProviderData[ItemParams.Item];

            AzureFileShareItem item = (AzureFileShareItem)ProviderData[ItemParams.Item];

            string containerUri = "";
            string protectedItemUri = "";
            string sourceResourceId = null;
            AzureFileshareProtectedItem properties = new AzureFileshareProtectedItem();

            if (policy != null)
            {
                if (itemBase == null)
                {
                    ValidateAzureFilesWorkloadType(policy.WorkloadType);

                    ValidateFileShareEnableProtectionRequest(
                        azureFileShareName,
                        storageAccountName);

                    WorkloadProtectableItemResource protectableObjectResource =
                        GetAzureFileShareProtectableObject(
                            azureFileShareName,
                            storageAccountName,
                            vaultName: vaultName,
                            vaultResourceGroupName: vaultResourceGroupName);

                    Dictionary<UriEnums, string> keyValueDict =
                        HelperUtils.ParseUri(protectableObjectResource.Id);
                    containerUri = HelperUtils.GetContainerUri(
                        keyValueDict, protectableObjectResource.Id);
                    protectedItemUri = HelperUtils.GetProtectableItemUri(
                        keyValueDict, protectableObjectResource.Id);

                    AzureFileShareProtectableItem azureFileShareProtectableItem =
                        (AzureFileShareProtectableItem)protectableObjectResource.Properties;
                    if (azureFileShareProtectableItem != null)
                    {
                        sourceResourceId = azureFileShareProtectableItem.ParentContainerFabricId;
                    }
                }
                else
                {
                    ValidateAzureFilesWorkloadType(item.WorkloadType, policy.WorkloadType);
                    ValidateAzureFilesModifyProtectionRequest(itemBase, policy);

                    Dictionary<UriEnums, string> keyValueDict = HelperUtils.ParseUri(item.Id);
                    containerUri = HelperUtils.GetContainerUri(keyValueDict, item.Id);
                    protectedItemUri = HelperUtils.GetProtectedItemUri(keyValueDict, item.Id);
                    sourceResourceId = item.SourceResourceId;
                }

                // construct Service Client protectedItem request
                properties.PolicyId = policy.Id;
                properties.SourceResourceId = sourceResourceId;
            }
            else
            {
                ValidateAzureFileShareDisableProtectionRequest(itemBase);

                Dictionary<UriEnums, string> keyValueDict = HelperUtils.ParseUri(item.Id);
                containerUri = HelperUtils.GetContainerUri(keyValueDict, item.Id);
                protectedItemUri = HelperUtils.GetProtectedItemUri(keyValueDict, item.Id);
                properties.PolicyId = string.Empty;
                properties.ProtectionState = ProtectionState.ProtectionStopped;
                properties.SourceResourceId = item.SourceResourceId;
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

        private void ValidateFileShareEnableProtectionRequest(string fileShareName, string storageAccountName)
        {
            if (string.IsNullOrEmpty(fileShareName))
            {
                throw new ArgumentException(string.Format(Resources.InvalidFileShareName));
            }
            if (string.IsNullOrEmpty(storageAccountName))
            {
                throw new ArgumentException(
                    string.Format(Resources.StorageAccountNameShouldNotBeEmpty)
                    );
            }
        }

        private void ValidateAzureFilesModifyProtectionRequest(ItemBase itemBase,
            PolicyBase policy)
        {
            if (itemBase == null || itemBase.GetType() != typeof(AzureFileShareItem))
            {
                throw new ArgumentException(string.Format(Resources.InvalidProtectionPolicyException,
                                            typeof(AzureFileShareItem).ToString()));
            }

            if (string.IsNullOrEmpty(((AzureFileShareItem)itemBase).ParentContainerFabricId))
            {
                throw new ArgumentException(Resources.VirtualMachineIdIsEmptyOrNull);
            }
        }

        private WorkloadProtectableItemResource GetAzureFileShareProtectableObject(
            string azureFileShareName,
            string storageAccountName,
            string vaultName = null,
            string vaultResourceGroupName = null)
        {
            //Trigger Discovery
            ODataQuery<BMSRefreshContainersQueryObject> queryParam = new ODataQuery<BMSRefreshContainersQueryObject>(
               q => q.BackupManagementType
                    == ServiceClientModel.BackupManagementType.AzureStorage);
            AzureWorkloadProviderHelper.RefreshContainer(vaultName, vaultResourceGroupName, queryParam);

            //get registered storage accounts
            bool isRegistered = false;
            string storageContainerName = null;
            List<ContainerBase> registeredStorageAccounts = GetRegisteredStorageAccounts(vaultName, vaultResourceGroupName);
            ContainerBase registeredStorageAccount = registeredStorageAccounts.Find(
                storageAccount => string.Compare(storageAccount.Name.Split(';').Last(),
                storageAccountName, true) == 0);
            if (registeredStorageAccount != null)
            {
                isRegistered = true;
                storageContainerName = "StorageContainer;" + registeredStorageAccount.Name;
            }

            //get unregistered storage account
            if (!isRegistered)
            {
                List<ProtectableContainerResource> unregisteredStorageAccounts =
                    GetUnRegisteredStorageAccounts(vaultName, vaultResourceGroupName);
                ProtectableContainerResource unregisteredStorageAccount = unregisteredStorageAccounts.Find(
                    storageAccount => string.Compare(storageAccount.Name.Split(';').Last(),
                    storageAccountName, true) == 0);
                if (unregisteredStorageAccount != null)
                {
                    //unregistered
                    //check for source Id for storageAccountId in ProtectionContainerResource
                    storageContainerName = unregisteredStorageAccount.Name;
                    ProtectionContainerResource protectionContainerResource =
                        new ProtectionContainerResource(unregisteredStorageAccount.Id,
                        unregisteredStorageAccount.Name);
                    AzureStorageContainer azureStorageContainer = new AzureStorageContainer(
                        friendlyName: storageAccountName,
                        backupManagementType: ServiceClientModel.BackupManagementType.AzureStorage,
                        sourceResourceId: unregisteredStorageAccount.Properties.ContainerId,
                        resourceGroup: vaultResourceGroupName);
                    protectionContainerResource.Properties = azureStorageContainer;
                    AzureWorkloadProviderHelper.RegisterContainer(unregisteredStorageAccount.Name,
                        protectionContainerResource,
                        vaultName,
                        vaultResourceGroupName);
                }
            }

            //inquiry
            TriggerInquiry(vaultName, vaultResourceGroupName, storageContainerName);

            //get protectable item
            WorkloadProtectableItemResource protectableObjectResource = null;
            protectableObjectResource = GetProtectableItem(vaultName, vaultResourceGroupName, azureFileShareName, storageAccountName);

            if (protectableObjectResource == null)
            {
                // Container is not discovered. Throw exception
                string errorMessage = string.Format(
                    Resources.DiscoveryFailure,
                    azureFileShareName,
                    vaultResourceGroupName);
                Logger.Instance.WriteDebug(errorMessage);
                Logger.Instance.WriteError(
                    new ErrorRecord(new Exception(Resources.FileShareNotDiscovered),
                        string.Empty, ErrorCategory.InvalidArgument, null));
            }

            return protectableObjectResource;
        }

        private WorkloadProtectableItemResource GetProtectableItem(string vaultName, string vaultResourceGroupName,
            string azureFileShareName, string storageAccountName)
        {
            WorkloadProtectableItemResource protectableObjectResource = null;
            ODataQuery<BMSPOQueryObject> queryParam = new ODataQuery<BMSPOQueryObject>(
                q => q.BackupManagementType
                     == ServiceClientModel.BackupManagementType.AzureStorage);

            var protectableItemList = ServiceClientAdapter.ListProtectableItem(
                queryParam,
                vaultName: vaultName,
                resourceGroupName: vaultResourceGroupName);

            if (protectableItemList.Count == 0)
            {
                //Container is not discovered
                Logger.Instance.WriteDebug(Resources.ContainerNotDiscovered);
            }

            foreach (var protectableItem in protectableItemList)
            {
                AzureFileShareProtectableItem azureFileShareProtectableItem =
                    (AzureFileShareProtectableItem)protectableItem.Properties;
                if (azureFileShareProtectableItem != null &&
                    string.Compare(azureFileShareProtectableItem.FriendlyName, azureFileShareName, true) == 0 &&
                    string.Compare(azureFileShareProtectableItem.ParentContainerFriendlyName, storageAccountName, true) == 0)
                {
                    protectableObjectResource = protectableItem;
                    break;
                }
            }
            return protectableObjectResource;
        }

        private void TriggerInquiry(string vaultName, string vaultResourceGroupName,
            string storageContainerName)
        {
            ODataQuery<BMSContainersInquiryQueryObject> queryParams = new ODataQuery<BMSContainersInquiryQueryObject>(
                q => q.WorkloadType
                     == ServiceClientModel.WorkloadType.AzureFileShare);
            string errorMessage = string.Empty;
            var inquiryResponse = ServiceClientAdapter.InquireContainer(
               storageContainerName,
               queryParams,
               vaultName,
               vaultResourceGroupName);

            var operationStatus = TrackingHelpers.GetOperationResult(
                inquiryResponse,
                operationId =>
                    ServiceClientAdapter.GetContainerRefreshOrInquiryOperationResult(
                        operationId,
                        vaultName: vaultName,
                        resourceGroupName: vaultResourceGroupName));

            //Now wait for the operation to Complete
            if (inquiryResponse.Response.StatusCode
                    != SystemNet.HttpStatusCode.NoContent)
            {
                errorMessage = string.Format(Resources.TriggerEnquiryFailureErrorCode,
                    inquiryResponse.Response.StatusCode);
                Logger.Instance.WriteDebug(errorMessage);
            }
        }

        private List<ContainerBase> GetRegisteredStorageAccounts(string vaultName = null,
            string vaultResourceGroupName = null)
        {
            ODataQuery<BMSContainerQueryObject> queryParams = null;
            queryParams = new ODataQuery<BMSContainerQueryObject>(
                q => q.BackupManagementType == ServiceClientModel.BackupManagementType.AzureStorage);

            var listResponse = ServiceClientAdapter.ListContainers(
                queryParams,
                vaultName: vaultName,
                resourceGroupName: vaultResourceGroupName);

            List<ContainerBase> containerModels = ConversionHelpers.GetContainerModelList(listResponse);

            return containerModels;
        }

        private List<ProtectableContainerResource> GetUnRegisteredStorageAccounts(string vaultName = null,
            string vaultResourceGroupName = null)
        {
            ODataQuery<BMSContainerQueryObject> queryParams = null;
            queryParams = new ODataQuery<BMSContainerQueryObject>(
                q => q.BackupManagementType == ServiceClientModel.BackupManagementType.AzureStorage);

            var listResponse = ServiceClientAdapter.ListUnregisteredContainers(
                queryParams,
                vaultName: vaultName,
                resourceGroupName: vaultResourceGroupName);
            List<ProtectableContainerResource> containerModels = listResponse.ToList();

            return containerModels;
        }

        private void ValidateAzureStorageBackupManagementType(
            CmdletModel.BackupManagementType backupManagementType)
        {
            if (backupManagementType != CmdletModel.BackupManagementType.AzureStorage)
            {
                throw new ArgumentException(string.Format(Resources.UnExpectedBackupManagementTypeException,
                                            CmdletModel.BackupManagementType.AzureStorage.ToString(),
                                            backupManagementType.ToString()));
            }
        }

        private void ValidateAzureFileShareDisableProtectionRequest(ItemBase itemBase)
        {

            if (itemBase == null || itemBase.GetType() != typeof(AzureFileShareItem))
            {
                throw new ArgumentException(string.Format(Resources.InvalidProtectionPolicyException,
                                            typeof(AzureFileShareItem).ToString()));
            }

            ValidateAzureFilesWorkloadType(itemBase.WorkloadType);
            ValidateAzureFilesContainerType(itemBase.ContainerType);
        }

        private void ValidateAzureFilesWorkloadType(CmdletModel.WorkloadType type)
        {
            if (type != CmdletModel.WorkloadType.AzureFiles)
            {
                throw new ArgumentException(string.Format(Resources.UnExpectedWorkLoadTypeException,
                                            CmdletModel.WorkloadType.AzureFiles.ToString(),
                                            type.ToString()));
            }
        }

        private void ValidateAzureFilesWorkloadType(CmdletModel.WorkloadType itemWorkloadType,
            CmdletModel.WorkloadType policyWorkloadType)
        {
            ValidateAzureFilesWorkloadType(itemWorkloadType);
            ValidateAzureFilesWorkloadType(policyWorkloadType);
            if (itemWorkloadType != policyWorkloadType)
            {
                throw new ArgumentException(string.Format(Resources.UnExpectedWorkLoadTypeException,
                                            CmdletModel.WorkloadType.AzureFiles.ToString(),
                                            itemWorkloadType.ToString()));
            }
        }

        public ResourceBackupStatus CheckBackupStatus()
        {
            string azureStorageAccountName = (string)ProviderData[ProtectionCheckParams.Name];
            string azureStorageAccountResourceGroupName =
                (string)ProviderData[ProtectionCheckParams.ResourceGroupName];

            ODataQuery<ProtectedItemQueryObject> queryParams =
                new ODataQuery<ProtectedItemQueryObject>(
                    q => q.BackupManagementType
                            == ServiceClientModel.BackupManagementType.AzureStorage &&
                         q.ItemType == DataSourceType.AzureFileShare);

            var vaultIds = ServiceClientAdapter.ListVaults();
            foreach (var vaultId in vaultIds)
            {
                ResourceIdentifier vaultIdentifier = new ResourceIdentifier(vaultId);

                var items = ServiceClientAdapter.ListProtectedItem(
                    queryParams,
                    vaultName: vaultIdentifier.ResourceName,
                    resourceGroupName: vaultIdentifier.ResourceGroupName);

                if (items.Any(
                    item =>
                    {
                        ResourceIdentifier storageIdentifier =
                            new ResourceIdentifier(item.Properties.SourceResourceId);
                        var itemStorageAccountName = storageIdentifier.ResourceName;
                        var itemStorageAccountRgName = storageIdentifier.ResourceGroupName;

                        return itemStorageAccountName.ToLower() == azureStorageAccountName.ToLower() &&
                            itemStorageAccountRgName.ToLower() == azureStorageAccountResourceGroupName.ToLower();
                    }))
                {
                    return new ResourceBackupStatus(
                        azureStorageAccountName,
                        azureStorageAccountResourceGroupName,
                        vaultId,
                        true);
                }
            }

            return new ResourceBackupStatus(
                azureStorageAccountName,
                azureStorageAccountResourceGroupName,
                null,
                false);
        }

        private void ValidateAzureFilesContainerType(CmdletModel.ContainerType type)
        {
            if (type != CmdletModel.ContainerType.AzureStorage)
            {
                throw new ArgumentException(string.Format(Resources.UnExpectedContainerTypeException,
                                            CmdletModel.ContainerType.AzureStorage.ToString(),
                                            type.ToString()));
            }
        }
    }
}