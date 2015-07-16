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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Management.BackupServices.Models;

namespace Microsoft.Azure.Commands.AzureBackup.Models
{
    /// <summary>
    /// Represents Azure Backup Item
    /// </summary>
    public class AzureBackupItem : AzureBackupItemContextObject
    {
        /// <summary>
        /// Status for the Azure Backup Item
        /// </summary>
        public string DataSourceStatus { get; set; }

        /// <summary>
        /// Protection Status for the Azure Backup Item
        /// </summary>
        public string ProtectionStatus { get; set; }

        /// <summary>
        /// DataSourceId of Azure Backup Item
        /// </summary>
        public string DataSourceId { get; set; }

        /// <summary>
        /// DataSourceId of Azure Backup Item
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Protection Policy Name for the Azure Backup Item
        /// </summary>
        public string ProtectionPolicyName { get; set; }

        /// <summary>
        /// Protection Policy Id for the Azure Backup Item
        /// </summary>
        public string ProtectionPolicyId { get; set; }

        /// <summary>
        /// Recovery Points Count for the Azure Backup Item
        /// </summary>
        public int RecoveryPointsCount { get; set; }

        public AzureBackupItem()
            : base()
        {
        }

        public AzureBackupItem(DataSourceInfo datasource, AzureBackupContainer azureBackupContainer)
            : base(datasource, azureBackupContainer)
        {
            DataSourceStatus = datasource.Status;
            ProtectionStatus = datasource.ProtectionStatus;
            ProtectionPolicyName = datasource.ProtectionPolicyName;
            ProtectionPolicyId = datasource.ProtectionPolicyId;
            RecoveryPointsCount = datasource.RecoveryPointsCount;
            DataSourceId = datasource.InstanceId;
            Type = datasource.Type;
        }

        public AzureBackupItem(ProtectableObjectInfo pPOItem, AzureBackupContainer azureBackupContainer)
            : base(pPOItem, azureBackupContainer)
        {
            ProtectionStatus = pPOItem.ProtectionStatus;
            DataSourceId = pPOItem.InstanceId;
            Type = pPOItem.Type;
        }
    }
}