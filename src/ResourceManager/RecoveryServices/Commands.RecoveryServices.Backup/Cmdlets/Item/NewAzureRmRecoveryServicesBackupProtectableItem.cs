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
using Microsoft.Azure.Commands.RecoveryServices.Backup.Helpers;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Properties;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using Microsoft.Azure.Management.RecoveryServices.Backup.Models;
using Microsoft.Rest.Azure.OData;
using System.Management.Automation;
using SystemNet = System.Net;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets
{
    /// <summary>
    /// Get list of items associated with the recovery services vault 
    /// according to the filters passed via the cmdlet parameters.
    /// </summary>
    [Cmdlet("New", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "RecoveryServicesBackupProtectableItem"), OutputType(typeof(ItemBase))]
    public class NewAzureRmRecoveryServicesBackupProtectableItem : RSBackupVaultCmdletBase
    {
        /// <summary>
        /// Workload type of the item to be returned.
        /// </summary>
        [Parameter(Mandatory = true, Position = 1,
            HelpMessage = ParamHelpMsgs.Common.WorkloadType)]
        [ValidateNotNullOrEmpty]
        public Models.WorkloadType WorkloadType { get; set; }


        ///// <summary>
        ///// Container base
        ///// </summary>
        //[Parameter(
        //    Mandatory = true, Position = 2,
        //    HelpMessage = ParamHelpMsgs.Item.Container,
        //    ValueFromPipelineByPropertyName = true)]
        //[ValidateNotNullOrEmpty]
        //public ContainerBase Container { get; set; }

        public override void ExecuteCmdlet()
        {
            ExecutionBlock(() =>
            {
                base.ExecuteCmdlet();

                ResourceIdentifier resourceIdentifier = new ResourceIdentifier(VaultId);
                string vaultName = resourceIdentifier.ResourceName;
                string vaultResourceGroupName = resourceIdentifier.ResourceGroupName;
                string workloadType = ConversionUtils.GetServiceClientWorkloadType(WorkloadType.ToString());

                ODataQuery<BMSContainersInquiryQueryObject> queryParams = new ODataQuery<BMSContainersInquiryQueryObject>(
                q => q.WorkloadType
                     == workloadType);
                string errorMessage = string.Empty;
                var inquiryResponse = ServiceClientAdapter.InquireContainer(
                   "compute;shracrg;shrac3",
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

                if (inquiryResponse.Response.StatusCode
                        == SystemNet.HttpStatusCode.OK)
                {
                    Logger.Instance.WriteDebug(errorMessage);
                }
                //Now wait for the operation to Complete
                if (inquiryResponse.Response.StatusCode
                        != SystemNet.HttpStatusCode.NoContent)
                {
                    errorMessage = string.Format(Resources.TriggerEnquiryFailureErrorCode,
                        inquiryResponse.Response.StatusCode);
                    Logger.Instance.WriteDebug(errorMessage);
                }

                // WriteObject(itemModels, enumerateCollection: true);
            });
        }
    }
}
