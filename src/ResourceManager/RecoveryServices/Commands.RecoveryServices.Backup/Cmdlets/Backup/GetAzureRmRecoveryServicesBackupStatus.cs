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

using System;
using System.Collections.Generic;
using System.Management.Automation;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.Models;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.ProviderModel;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Properties;
using Microsoft.Azure.Management.Internal.Resources.Models;
using Microsoft.Azure.Management.Internal.Resources;
using System.Threading;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets
{
    /// <summary>
    /// This cmdlet can be used to check if a VM is backed up by any vault in the subscription.
    /// </summary>
    [Cmdlet("Get", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "RecoveryServicesBackupStatus", DefaultParameterSetName = NameParamSet)]
    [OutputType(typeof(ResourceBackupStatus))]
    public class GetAzureRmRecoveryServicesBackupStatus : RecoveryServicesBackupCmdletBase
    {
        const string NameParamSet = "Name";
        const string IdParamSet = "Id";

        /// <summary>
        /// Name of the Azure Resource whose representative item needs to be checked 
        /// if it is already protected by some Recovery Services Vault in the subscription.
        /// </summary>
        [Parameter(ParameterSetName = NameParamSet, Mandatory = true,
            HelpMessage = ParamHelpMsgs.ProtectionCheck.Name)]
        public string Name { get; set; }

        /// <summary>
        /// Name of the resource group of the Azure Resource whose representative item 
        /// needs to be checked if it is already protected by some RecoveryServices Vault 
        /// in the subscription.
        /// </summary>
        [Parameter(ParameterSetName = NameParamSet, Mandatory = true,
            HelpMessage = ParamHelpMsgs.ProtectionCheck.ResourceGroupName)]
        [ResourceGroupCompleter]
        public string ResourceGroupName { get; set; }

        /// <summary>
        /// Type of the Azure Resource whose representative item needs to be checked 
        /// if it is already protected by some Recovery Services Vault in the subscription.
        /// </summary>
        [Parameter(ParameterSetName = NameParamSet, Mandatory = true,
            HelpMessage = ParamHelpMsgs.ProtectionCheck.Type)]
        [ValidateSet("AzureVM", "AzureFiles")]
        public string Type { get; set; }

        [Parameter(ParameterSetName = IdParamSet, ValueFromPipelineByPropertyName = true,
            HelpMessage = ParamHelpMsgs.ProtectionCheck.ResourceId, Mandatory = true)]
        public string ResourceId { get; set; }

        public override void ExecuteCmdlet()
        {
            ExecutionBlock(() =>
            {
                base.ExecuteCmdlet();

                string name = Name;
                string resourceGroupName = ResourceGroupName;
                string type = Type;

                if (ParameterSetName == IdParamSet)
                {
                    ResourceIdentifier resourceIdentifier = new ResourceIdentifier(ResourceId);
                    name = resourceIdentifier.ResourceName;
                    resourceGroupName = resourceIdentifier.ResourceGroupName;
                    type = resourceIdentifier.ResourceType;
                }

                GenericResource resource = null;
                if (type == "Microsoft.Compute/virtualMachines" ||
                type == "Microsoft.ClassicCompute/virtualMachines" ||
                type == "AzureVM")
                {
                    resource = GetVirtualMachineResource(name,
                    resourceGroupName);
                }
                else if (type == "AzureFiles")
                {
                    
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(Resources.UnsupportedResourceTypeException,
                        type));
                }

                PsBackupProviderManager providerManager =
                    new PsBackupProviderManager(new Dictionary<Enum, object>()
                    {
                        { ProtectionCheckParams.Name, name },
                        { ProtectionCheckParams.ResourceGroupName, resourceGroupName },
                        { ProtectionCheckParams.ResourceId, resource.Id },
                        { ProtectionCheckParams.ResourceType, resource.Type },
                        { ProtectionCheckParams.Location, resource.Location }
                    }, ServiceClientAdapter);

                IPsBackupProvider psBackupProvider =
                    providerManager.GetProviderInstance(type);

                WriteObject(psBackupProvider.CheckBackupStatus());
            });
        }

        public GenericResource GetVirtualMachineResource(string name, string resourceGroupName)
        {
            name = name.ToLower();
            ResourceIdentity identity = new ResourceIdentity();
            identity.ResourceName = name;
            identity.ResourceProviderNamespace = "Microsoft.ClassicCompute/virtualMachines";
            identity.ResourceProviderApiVersion = "2015-12-01";
            identity.ResourceType = string.Empty;
            identity.ParentResourcePath = string.Empty;

            GenericResource resource = null;
            try
            {
                WriteDebug(string.Format("Query Microsoft.ClassicCompute with name = {0}",
                    name));
                resource = RmClient.Resources.GetAsync(
                    resourceGroupName,
                    identity.ResourceProviderNamespace,
                    identity.ParentResourcePath,
                    identity.ResourceType,
                    identity.ResourceName,
                    identity.ResourceProviderApiVersion,
                    CancellationToken.None).Result;
            }
            catch (Exception)
            {
                identity.ResourceProviderNamespace = "Microsoft.Compute/virtualMachines";
                identity.ResourceProviderApiVersion = "2018-06-01";
                resource = RmClient.Resources.GetAsync(
                    resourceGroupName,
                    identity.ResourceProviderNamespace,
                    identity.ParentResourcePath,
                    identity.ResourceType,
                    identity.ResourceName,
                    identity.ResourceProviderApiVersion,
                    CancellationToken.None).Result;
            }

            return resource;
        }
    }
}
