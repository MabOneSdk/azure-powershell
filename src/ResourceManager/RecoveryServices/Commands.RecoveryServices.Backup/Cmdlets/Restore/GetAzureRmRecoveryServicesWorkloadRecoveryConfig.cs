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
using Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.ProviderModel;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Properties;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using ServiceClientModel = Microsoft.Azure.Management.RecoveryServices.Backup.Models;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets
{
    /// <summary>
    /// Restores an item using the recovery point provided within the recovery services vault
    /// </summary>
    [Cmdlet("Get", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "RecoveryServicesWorkloadRecoveryConfig",
        DefaultParameterSetName = AzureVMParameterSet, SupportsShouldProcess = true), OutputType(typeof(JobBase))]
    public class GetAzureRmRecoveryServicesWorkloadRecoveryConfig : RSBackupVaultCmdletBase
    {
        internal const string AzureVMParameterSet = "AzureVMParameterSet";
        internal const string AzureFileParameterSet = "AzureFileParameterSet";

        /// <summary>
        /// Recovery point of the item to be restored
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = true, Position = 0,
            HelpMessage = ParamHelpMsgs.RestoreDisk.RecoveryPoint)]
        [ValidateNotNullOrEmpty]
        public RecoveryPointBase RecoveryPoint { get; set; }

        /// <summary>
        /// End time of Time range for which recovery points need to be fetched
        /// </summary>
        [Parameter(Mandatory = false, ValueFromPipeline = false, Position = 1,
            HelpMessage = ParamHelpMsgs.RecoveryPoint.EndDate)]
        [ValidateNotNullOrEmpty]
        public DateTime? PointInTime { get; set; }

        /// <summary>
        /// Protected Item object for which recovery points need to be fetched
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = false, Position = 2,
            HelpMessage = ParamHelpMsgs.RecoveryPoint.Item)]
        [ValidateNotNullOrEmpty]
        public ItemBase Item { get; set; }

        /// <summary>
        /// Use this switch if the disks from the recovery point are to be restored to their original storage accounts
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = AzureVMParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreVM.OsaOption)]
        public SwitchParameter OriginalWorkloadRestore { get; set; }

        public override void ExecuteCmdlet()
        {
            ExecutionBlock(() =>
            {
                base.ExecuteCmdlet();

                ResourceIdentifier resourceIdentifier = new ResourceIdentifier(VaultId);
                string vaultName = resourceIdentifier.ResourceName;
                string resourceGroupName = resourceIdentifier.ResourceGroupName;
                Dictionary<Enum, object> providerParameters = new Dictionary<Enum, object>();

                providerParameters.Add(VaultParams.VaultName, vaultName);
                providerParameters.Add(VaultParams.ResourceGroupName, resourceGroupName);
                providerParameters.Add(WorkloadRecoveryConfigParams.RecoveryPoint, RecoveryPoint);
                providerParameters.Add(WorkloadRecoveryConfigParams.OriginalWorkloadRestore, OriginalWorkloadRestore.IsPresent);
                providerParameters.Add(WorkloadRecoveryConfigParams.Item, Item);
                providerParameters.Add(WorkloadRecoveryConfigParams.PointInTime, PointInTime);

                PsBackupProviderManager providerManager =
                    new PsBackupProviderManager(providerParameters, ServiceClientAdapter);
                IPsBackupProvider psBackupProvider = providerManager.GetProviderInstance(
                    RecoveryPoint.WorkloadType, RecoveryPoint.BackupManagementType);
                var jobResponse = psBackupProvider.TriggerRestore();

                ServiceClientModel.AzureWorkloadRestoreRequest;
                ServiceClientModel.AzureWorkloadSQLPointInTimeRestoreRequest;
                ServiceClientModel.AzureWorkloadSQLRestoreRequest;
                ServiceClientModel.AzureWorkloadSQLRecoveryPoint

                WriteDebug(string.Format("Restore submitted"));
                HandleCreatedJob(
                    jobResponse,
                    Resources.RestoreOperation,
                    vaultName: vaultName,
                    resourceGroupName: resourceGroupName);
            }, ShouldProcess(RecoveryPoint.ItemName, VerbsData.Restore));
        }
    }
}
