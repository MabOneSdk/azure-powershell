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

using Microsoft.Azure.Commands.AzureBackup.Helpers;
using Microsoft.Azure.Commands.AzureBackup.Library;
using Microsoft.Azure.Commands.AzureBackup.Models;
using Microsoft.Azure.Management.BackupServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Commands.AzureBackup.Cmdlets
{
    /// <summary>
    /// Get list of containers
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "AzureBackupContainer"), OutputType(typeof(AzureBackupContainer), typeof(List<AzureBackupContainer>))]
    public class GetAzureBackupContainer : AzureBackupVaultCmdletBase
    {
        [Parameter(Mandatory = false, HelpMessage = AzureBackupCmdletHelpMessage.ManagedResourceGroupName)]
        [ValidateNotNullOrEmpty]
        public string ManagedResourceGroupName { get; set; }

        [Parameter(Mandatory = false, HelpMessage = AzureBackupCmdletHelpMessage.ManagedResourceName)]
        [ValidateNotNullOrEmpty]
        public string ManagedResourceName { get; set; }

        [Parameter(Mandatory = false, HelpMessage = AzureBackupCmdletHelpMessage.ContainerRegistrationStatus)]
        [ValidateNotNullOrEmpty]
        public AzureBackupContainerStatusInput Status { get; set; }

        [Parameter(Mandatory = false, HelpMessage = AzureBackupCmdletHelpMessage.ContainerType)]
        [ValidateNotNullOrEmpty]
        public AzureBackupContainerTypeInput Type { get; set; }

        public override void ExecuteCmdlet()
        {
            ExecutionBlock(() =>
            {
                base.ExecuteCmdlet();

                string queryFilterString = string.Empty;
                queryFilterString = ConstructQueryFilterString();
                var containerList = AzureBackupClient.ListContainers(queryFilterString);

                WriteDebug(string.Format("Fetched {0} containers", containerList.Count()));

                List<CSMContainerResponse> containerInfos = containerList.ToList();

                // When resource group name is specified, remove all containers whose resource group name
                // doesn't match the given resource group name
                if (!string.IsNullOrEmpty(ManagedResourceGroupName))
                {
                    containerInfos.RemoveAll(containerInfo =>
                    {
                        string rgName = ContainerHelpers.GetRGNameFromId(containerInfo.Properties.ParentContainerId);
                        return rgName != ManagedResourceGroupName;
                    });
                    WriteDebug(string.Format("Count of containers after resource group filter = {0}", containerInfos.Count));                
                }

                // TODO: Container friendly name is not captures in Container response
                // BUG: Friendly name was previously assigned to ResourceName (vault name)
                List<AzureBackupContainer> containers = containerInfos.ConvertAll(containerInfo =>
                {
                    return new AzureBackupContainer(Vault, containerInfo);
                });

                // When container resource name and container resource group name are specified, this parameter set
                // identifies a container uniquely. Thus, we return just one container instead of a list.
                if (!string.IsNullOrEmpty(ManagedResourceName) & !string.IsNullOrEmpty(ManagedResourceGroupName))
                {
                    if (containers.Any())
                    {
                        WriteObject(containers.First());
                    }
                }
                else
                {
                    WriteObject(containers);
                }
            });
        }

        private string ConstructQueryFilterString()
        {
            var containerQueryObject = new ListContainerQueryParameter();

            switch (Type)
            {
                case AzureBackupContainerTypeInput.AzureVirtualMachine:
                    containerQueryObject.ContainerTypeField = AzureBackupContainerType.IaasVMContainer.ToString();
                    break;
                default:
                    break;
            }

            switch (Status)
            {
                case AzureBackupContainerStatusInput.Registered:
                    containerQueryObject.ContainerStatusField = AzureBackupContainerRegistrationStatus.Registered.ToString();
                    break;
                case AzureBackupContainerStatusInput.Registering:
                    containerQueryObject.ContainerStatusField = AzureBackupContainerRegistrationStatus.Registering.ToString();
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(ManagedResourceName))
            {
                containerQueryObject.ContainerFriendlyNameField = ManagedResourceName;
            }

            return ContainerHelpers.GetQueryFilter(containerQueryObject);
        }
    }
}
