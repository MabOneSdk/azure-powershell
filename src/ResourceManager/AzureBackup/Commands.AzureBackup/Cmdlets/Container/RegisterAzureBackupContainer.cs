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
using System.Web;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Commands.Compute;
using Microsoft.Azure.Management.BackupServices.Models;
using MBS = Microsoft.Azure.Management.BackupServices;
using Microsoft.Azure.Commands.Compute.Models;
using Microsoft.WindowsAzure.Commands.ServiceManagement.Model;

namespace Microsoft.Azure.Commands.AzureBackup.Cmdlets
{
    /// <summary>
    /// Get list of containers
    /// </summary>
    [Cmdlet(VerbsLifecycle.Register, "AzureBackupContainer", DefaultParameterSetName = V1VMParameterSet), OutputType(typeof(Guid))]
    public class RegisterAzureBackupContainer : AzureBackupVaultCmdletBase
    {
        internal const string V1VMParameterSet = "V1VM";
        internal const string V2VMParameterSet = "V2VM";

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = V1VMParameterSet, HelpMessage = AzureBackupCmdletHelpMessage.VirtualMachine)]
        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = V2VMParameterSet, HelpMessage = AzureBackupCmdletHelpMessage.VirtualMachine)]
        public string Name { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = V1VMParameterSet, HelpMessage = AzureBackupCmdletHelpMessage.VirtualMachine)]
        public string ServiceName { get; set; }

        [Parameter(Mandatory = false, ValueFromPipelineByPropertyName = true, ParameterSetName = V2VMParameterSet, HelpMessage = AzureBackupCmdletHelpMessage.VirtualMachine)]
        public string VMResourceGroupName { get; set; }

        
        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            ExecutionBlock(() =>
            {
                string vmName = String.Empty;
                string rgName = String.Empty;
                string ServiceOrRG = String.Empty;

                if(this.ParameterSetName == V1VMParameterSet)
                {
                    vmName = Name;
                    rgName = ServiceName;
                    WriteDebug(String.Format("Registering ARM-V1 VM, VMName: {0}, CloudServiceName: {1}", vmName, rgName));
                    ServiceOrRG = "CloudServiceName";
                }
                else if(this.ParameterSetName == V2VMParameterSet)
                {
                    vmName = Name;
                    rgName = VMResourceGroupName;
                    WriteDebug(String.Format("Registering ARM-V2 VM, VMName: {0}, ResourceGroupName: {1}", vmName, rgName));
                    ServiceOrRG = "ResourceGroupName";
                }

                else
                {
                    throw new PSArgumentException("Please make sure you have pass right set of parameters"); //TODO: PM scrub needed
                }

                Guid jobId = Guid.Empty;
                bool isDiscoveryNeed = false;
                MBS.OperationResponse operationResponse;

                ContainerInfo container = null;
                isDiscoveryNeed = IsDiscoveryNeeded(vmName, rgName, out container);
                if(isDiscoveryNeed)
                {
                    WriteDebug(String.Format("VM {0} is not yet discovered. Triggering Discovery", vmName));
                    RefreshContainer();
                    isDiscoveryNeed = IsDiscoveryNeeded(vmName, rgName, out container);
                    if ((isDiscoveryNeed == true) || (container == null))
                    {
                        //Container is not discovered. Throw exception
                        string errMsg = String.Format("Failed to discover VM {0} under {1} {2}. Please make sure names are correct and VM is not deleted", vmName, ServiceOrRG, rgName);
                        WriteDebug(errMsg);
                        throw new Exception(errMsg); //TODO: Sync with piyush and srub error msg 
                    }
                }                

                //Container is discovered. Register the container
                WriteDebug(String.Format("Going to register VM {0}", vmName));
                List<string> containerNameList = new List<string>();
                containerNameList.Add(container.Name);
                RegisterContainerRequestInput registrationRequest = new RegisterContainerRequestInput(containerNameList, AzureBackupContainerType.IaasVMContainer.ToString());
                operationResponse = AzureBackupClient.Container.RegisterAsync(registrationRequest, GetCustomRequestHeaders(), CmdletCancellationToken).Result;

                //TODO fix the OperationResponse to JobID conversion
                jobId = operationResponse.OperationId;
                WriteObject(jobId);
            });
        }

        private void RefreshContainer()
        {
            bool isRetyNeeded = true;
            int retryCount = 1;
            bool isDiscoverySuccessful = false;
            while (isRetyNeeded && retryCount <= 3)
            {
                MBS.OperationResponse opResponse =
                    AzureBackupClient.Container.RefreshAsync(GetCustomRequestHeaders(), CmdletCancellationToken).Result;

                //Now wait for the operation to Complete               
                isRetyNeeded = WaitForDiscoveryToCOmplete(opResponse.OperationId.ToString(), out isDiscoverySuccessful);
                retryCount++;
            }

            if (!isDiscoverySuccessful)
            {
                //Discovery failed
                throw new Exception(); //TODO:
            }
        }

        private bool WaitForDiscoveryToCOmplete(string operationId, out bool isDiscoverySuccessful)
        {
            bool isRetryNeeded = false;
            AzureBackupOperationStatusResponse status = new AzureBackupOperationStatusResponse() 
                        {
                            OperationStatus = AzureBackupOperationStatus.InProgress.ToString() 
                        };
            while (status.OperationStatus != AzureBackupOperationStatus.Completed.ToString())
            {
                status = AzureBackupClient.OperationStatus.GetAsync(operationId, GetCustomRequestHeaders(), CmdletCancellationToken).Result;
                WriteDebug(String.Format("Status of DiscoveryOperation: {0}", status.OperationStatus));
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(15));
            }

            isDiscoverySuccessful = true;
            //If operation fails check if retry is needed or not
            if (status.OperationResult != AzureBackupOperationResult.Succeeded.ToString())
            {
                isDiscoverySuccessful = false;
                WriteDebug(String.Format("DiscoveryOperation failed wit ErrorCOde: {0}", status.ErrorCode));
                if ((status.ErrorCode == AzureBackupOperationErrorCode.DiscoveryInProgress.ToString() ||
                    (status.ErrorCode == AzureBackupOperationErrorCode.BMSUserErrorObjectLocked.ToString())))
                {
                    //Need to retry for this errors
                    isRetryNeeded = true;
                    WriteDebug(String.Format("Going to retry Discovery if retry count is not exceeded"));
                }
            }
            return isRetryNeeded;         
        }

        private bool IsDiscoveryNeeded(string vmName, string rgName, out ContainerInfo container)
        {
            bool isDiscoveryNeed = false;
            //First check if container is discoverd or not
            ListContainerQueryParameter queryParams = new ListContainerQueryParameter();
            queryParams.ContainerTypeField = AzureBackupContainerType.IaasVMContainer.ToString();
            queryParams.ContainerStatusField = String.Empty;
            queryParams.ContainerFriendlyNameField = vmName;
            string queryString = GetQueryFileter(queryParams);

            ListContainerResponse containers = AzureBackupClient.Container.ListAsync(queryString,
                            GetCustomRequestHeaders(), CmdletCancellationToken).Result;
            WriteDebug(String.Format("Container count returned from service: {0}.", containers.Objects.Count()));
            if (containers.Objects.Count() == 0)
            {
                //Container is not discover
                WriteDebug("Container is not discovered");
                container = null;
                isDiscoveryNeed = true;
            }

            else
            {
                //We can have multiple container with same friendly name.
                //Look for resourceGroup name in the container unoque name                
                container = containers.Objects.Where(c => c.ParentContainerFriendlyName.ToLower().Equals(rgName.ToLower())).FirstOrDefault();
                if (container == null)
                {
                    //Container is not in list of registered container
                    WriteDebug(String.Format("Desired Container is not found. Returning with isDiscoveryNeed = true"));
                    isDiscoveryNeed = true;
                }
            }
            return isDiscoveryNeed;
        }

        private string GetQueryFileter(ListContainerQueryParameter queryParams)
        {
            NameValueCollection collection = new NameValueCollection();
            if (!String.IsNullOrEmpty(queryParams.ContainerTypeField))
            {
                collection.Add("ContainerType", queryParams.ContainerTypeField);
            }

            if (!String.IsNullOrEmpty(queryParams.ContainerStatusField))
            {
                collection.Add("ContainerStatus", queryParams.ContainerStatusField);
            }

            if (!String.IsNullOrEmpty(queryParams.ContainerFriendlyNameField))
            {
                collection.Add("FriendlyName", queryParams.ContainerFriendlyNameField);
            }

            if (collection == null || collection.Count == 0)
            {
                return String.Empty;
            }

            var httpValueCollection = HttpUtility.ParseQueryString(String.Empty);
            httpValueCollection.Add(collection);

            return "&" + httpValueCollection.ToString();
        }
    }
}
