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
using System.Management.Automation;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using Microsoft.Azure.Management.BackupServices.Models;
using MBS = Microsoft.Azure.Management.BackupServices;
using System.Runtime.Serialization;
using Microsoft.Azure.Management.BackupServices;
using Microsoft.Azure.Commands.AzureBackup.Models;

namespace Microsoft.Azure.Commands.AzureBackup.Cmdlets
{
    /// <summary>
    /// Enable Azure Backup protection
    /// </summary>
    [Cmdlet(VerbsLifecycle.Enable, "AzureBackupProtection"), OutputType(typeof(string))]
    public class EnableAzureBackupProtection : AzureBackupItemCmdletBase
    {
        [Parameter(Mandatory = true, HelpMessage = AzureBackupCmdletHelpMessage.PolicyName)]
        [ValidateNotNullOrEmpty]
        public AzureBackupProtectionPolicy Policy { get; set; }

        public override void ExecuteCmdlet()
        {
            ExecutionBlock(() =>
            {
                base.ExecuteCmdlet();

                WriteDebug("Making client call");
                string itemName = string.Empty;

                CSMSetProtectionRequest input = new CSMSetProtectionRequest();
                input.Properties.PolicyId = Policy.InstanceId;

                if (Item.GetType() == typeof(AzureBackupItem))
                {
                    itemName = (Item as AzureBackupItem).ItemName;
                }

                else if (Item.GetType() == typeof(AzureBackupContainer))
                {
                    WriteDebug("Input is container Type = " + Item.GetType());
                    if ((Item as AzureBackupContainer).ContainerType == AzureBackupContainerType.IaasVMContainer.ToString())
                    {
                        itemName = (Item as AzureBackupContainer).ContainerUniqueName;
                    }
                    else
                    {
                        throw new Exception("Uknown item type");
                    }
                }

                else
                {
                    throw new Exception("Uknown item type");
                }

                var operationId = AzureBackupClient.EnableProtection(Item.ContainerUniqueName,itemName, input);
                WriteDebug("Received enable azure backup protection response");

                var operationStatus = GetOperationStatus(operationId);
                this.WriteObject(GetCreatedJobs(new Models.AzurePSBackupVault(Item.ResourceGroupName, Item.ResourceName, Item.Location), operationStatus.JobList).FirstOrDefault());
            });
        }
    }
}
