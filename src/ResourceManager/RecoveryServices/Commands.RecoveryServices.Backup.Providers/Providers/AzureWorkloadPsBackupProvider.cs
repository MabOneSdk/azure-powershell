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
using Microsoft.Azure.Management.RecoveryServices.Backup.Models;
using System;
using System.Collections.Generic;
using CmdletModel = Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.Models;
using RestAzureNS = Microsoft.Rest.Azure;

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
            return AzureWorkloadProviderHelper.GetRecoveryPointDetails(ProviderData);
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
            throw new NotImplementedException();
        }

        public List<RecoveryPointBase> ListRecoveryPoints()
        {
            return AzureWorkloadProviderHelper.ListRecoveryPoints(ProviderData);
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

        public List<PointInTimeRange> GetLogChains()
        {
            return AzureWorkloadProviderHelper.ListLogChains(ProviderData);
        }
    }
}