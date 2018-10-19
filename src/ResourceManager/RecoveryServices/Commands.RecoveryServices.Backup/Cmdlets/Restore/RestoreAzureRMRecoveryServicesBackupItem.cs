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
using Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.ProviderModel;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Properties;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Microsoft.Azure.Management.Internal.Resources;
using Microsoft.Azure.Management.Internal.Resources.Models;
using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets
{
    /// <summary>
    /// Restores an item using the recovery point provided within the recovery services vault
    /// </summary>
    [Cmdlet("Restore", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "RecoveryServicesBackupItem",
        DefaultParameterSetName = AzureVMParameterSet, SupportsShouldProcess = true), OutputType(typeof(JobBase))]
    public class RestoreAzureRmRecoveryServicesBackupItem : RSBackupVaultCmdletBase
    {
        internal const string AzureVMParameterSet = "AzureVMParameterSet";
        internal const string AzureFileParameterSet = "AzureFileParameterSet";

        /// <summary>
        /// Location of the Recovery Services Vault.
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = AzureVMParameterSet,
            HelpMessage = "Location of the Recovery Services Vault.",
            ValueFromPipeline = true)]
        [LocationCompleter("Microsoft.RecoveryServices/vaults")]
        [ValidateNotNullOrEmpty]
        public string VaultLocation { get; set; }

        /// <summary>
        /// Recovery point of the item to be restored
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0,
            HelpMessage = ParamHelpMsgs.RestoreDisk.RecoveryPoint)]
        [ValidateNotNullOrEmpty]
        public RecoveryPointBase RecoveryPoint { get; set; }

        /// <summary>
        /// Storage account name where the disks need to be recovered
        /// </summary>
        [Parameter(Mandatory = true, Position = 1,
            HelpMessage = ParamHelpMsgs.RestoreDisk.StorageAccountName)]
        [ValidateNotNullOrEmpty]
        public string StorageAccountName { get; set; }

        /// <summary>
        /// Resource group name of Storage account name where the disks need to be recovered
        /// </summary>
        [Parameter(Mandatory = true, Position = 2,
            HelpMessage = ParamHelpMsgs.RestoreDisk.StorageAccountResourceGroupName)]
        [ValidateNotNullOrEmpty]
        public string StorageAccountResourceGroupName { get; set; }

        /// <summary> 
        /// The resource group to which the managed disks are restored. Applicable to backup of VM with managed disks.
        /// </summary>
        [Parameter(Mandatory = false, Position = 3, ParameterSetName = AzureVMParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreVM.TargetResourceGroupName)]
        [ValidateNotNullOrEmpty]
        public string TargetResourceGroupName { get; set; }

        /// <summary>
        /// Resolve conflict option
        /// </summary>
        [Parameter(Mandatory = true, Position = 3, ParameterSetName = AzureFileParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreFS.ResolveConflict)]
        [ValidateNotNullOrEmpty]
        public RestoreFSResolveConfictOption ResolveConflict { get; set; }

        /// <summary>
        /// Source File Path of the file to be recovered
        /// </summary>
        [Parameter(Mandatory = false, Position = 4, ParameterSetName = AzureFileParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreFS.SourceFilePath)]
        [ValidateNotNullOrEmpty]
        public string SourceFilePath { get; set; }

        /// <summary>
        /// Source File Type of the file to be recovered
        /// </summary>
        [Parameter(Mandatory = false, Position = 5, ParameterSetName = AzureFileParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreFS.SourceFilePath)]
        [ValidateNotNullOrEmpty]
        public SourceFileType? SourceFileType { get; set; }

        /// <summary>
        /// Target storage account name where the disks need to be recovered
        /// </summary>
        [Parameter(Mandatory = false, Position = 6, ParameterSetName = AzureFileParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreFS.TargetStorageAccountName)]
        [ValidateNotNullOrEmpty]
        public string TargetStorageAccountName { get; set; }

        /// <summary>
        /// Target storage account resource group name where the file need to be recovered
        /// </summary>
        [Parameter(Mandatory = false, Position = 7, ParameterSetName = AzureFileParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreFS.TargetStorageAccountResourceGroupName)]
        [ValidateNotNullOrEmpty]
        public string TargetStorageAccountResourceGroupName { get; set; }

        /// <summary> 
        /// The target file share name to which the files are restored.
        /// </summary>
        [Parameter(Mandatory = false, Position = 8, ParameterSetName = AzureFileParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreFS.TargetFileShareName)]
        [ValidateNotNullOrEmpty]
        public string TargetFileShareName { get; set; }

        /// <summary> 
        /// The target folder name to which the files are restored.
        /// </summary>
        [Parameter(Mandatory = false, Position = 9, ParameterSetName = AzureFileParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreFS.TargetFolder)]
        [ValidateNotNullOrEmpty]
        public string TargetFolder { get; set; }

        /// <summary>
        /// Use this switch if the disks from the recovery point are to be restored to their original storage accounts
        /// </summary>
        [Parameter(Mandatory = false, ParameterSetName = AzureVMParameterSet,
            HelpMessage = ParamHelpMsgs.RestoreVM.OsaOption)]
        public SwitchParameter UseOriginalStorageAccount { get; set; }

        public override void ExecuteCmdlet()
        {
            ExecutionBlock(() =>
            {
                base.ExecuteCmdlet();

                ResourceIdentifier resourceIdentifier = new ResourceIdentifier(VaultId);
                string vaultName = resourceIdentifier.ResourceName;
                string resourceGroupName = resourceIdentifier.ResourceGroupName;
                Dictionary<Enum, object> providerParameters = new Dictionary<Enum, object>();

                GenericResource storageAccountResource = GetStorageAccountResource(StorageAccountName,
                    StorageAccountResourceGroupName);
                WriteDebug(string.Format("StorageId = {0}", storageAccountResource.Id));
                providerParameters.Add(RestoreBackupItemParams.StorageAccountId, storageAccountResource.Id);
                providerParameters.Add(RestoreBackupItemParams.StorageAccountLocation, storageAccountResource.Location);
                providerParameters.Add(RestoreBackupItemParams.StorageAccountType, storageAccountResource.Type);

                if (ParameterSetName == AzureFileParameterSet && TargetStorageAccountName != null)
                {
                    GenericResource targetStorageAccountResource = GetStorageAccountResource(TargetStorageAccountName,
                        TargetStorageAccountResourceGroupName);
                    WriteDebug(string.Format("StorageId = {0}", targetStorageAccountResource.Id));
                    providerParameters.Add(RestoreFSBackupItemParams.TargetStorageAccountId,
                        targetStorageAccountResource.Id);
                    providerParameters.Add(RestoreFSBackupItemParams.TargetStorageAccountLocation,
                        targetStorageAccountResource.Location);
                }

                providerParameters.Add(VaultParams.VaultName, vaultName);
                providerParameters.Add(VaultParams.ResourceGroupName, resourceGroupName);
                providerParameters.Add(VaultParams.VaultLocation, VaultLocation);
                providerParameters.Add(RestoreBackupItemParams.RecoveryPoint, RecoveryPoint);
                providerParameters.Add(RestoreVMBackupItemParams.OsaOption, UseOriginalStorageAccount.IsPresent);
                providerParameters.Add(RestoreFSBackupItemParams.ResolveConflict, ResolveConflict.ToString());
                providerParameters.Add(RestoreFSBackupItemParams.SourceFilePath, SourceFilePath);
                providerParameters.Add(RestoreFSBackupItemParams.TargetStorageAccountName, TargetStorageAccountName);
                providerParameters.Add(RestoreFSBackupItemParams.TargetFileShareName, TargetFileShareName);
                providerParameters.Add(RestoreFSBackupItemParams.TargetFolder, TargetFolder);

                if (TargetResourceGroupName != null)
                {
                    providerParameters.Add(RestoreVMBackupItemParams.TargetResourceGroupName, TargetResourceGroupName);
                }

                if (SourceFileType != null)
                {
                    providerParameters.Add(RestoreFSBackupItemParams.SourceFileType, SourceFileType.ToString());
                }

                PsBackupProviderManager providerManager =
                    new PsBackupProviderManager(providerParameters, ServiceClientAdapter);
                IPsBackupProvider psBackupProvider = providerManager.GetProviderInstance(
                    RecoveryPoint.WorkloadType, RecoveryPoint.BackupManagementType);
                var jobResponse = psBackupProvider.TriggerRestore();

                WriteDebug(string.Format("Restore submitted"));
                HandleCreatedJob(
                    jobResponse,
                    Resources.RestoreOperation,
                    vaultName: vaultName,
                    resourceGroupName: resourceGroupName);
            }, ShouldProcess(RecoveryPoint.ItemName, VerbsData.Restore));
        }

        private GenericResource GetStorageAccountResource(string storageAccountName, string storageAccountResourceGroupName)
        {
            storageAccountName = storageAccountName.ToLower();
            ResourceIdentity identity = new ResourceIdentity();
            identity.ResourceName = storageAccountName;
            identity.ResourceProviderNamespace = "Microsoft.ClassicStorage/storageAccounts";
            identity.ResourceProviderApiVersion = "2015-12-01";
            identity.ResourceType = string.Empty;
            identity.ParentResourcePath = string.Empty;

            GenericResource resource = null;
            try
            {
                WriteDebug(string.Format("Query Microsoft.ClassicStorage with name = {0}",
                    StorageAccountName));
                resource = RmClient.Resources.GetAsync(
                    storageAccountResourceGroupName,
                    identity.ResourceProviderNamespace,
                    identity.ParentResourcePath,
                    identity.ResourceType,
                    identity.ResourceName,
                    identity.ResourceProviderApiVersion,
                    CancellationToken.None).Result;
            }
            catch (Exception)
            {
                identity.ResourceProviderNamespace = "Microsoft.Storage/storageAccounts";
                identity.ResourceProviderApiVersion = "2016-01-01";
                resource = RmClient.Resources.GetAsync(
                    storageAccountResourceGroupName,
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
