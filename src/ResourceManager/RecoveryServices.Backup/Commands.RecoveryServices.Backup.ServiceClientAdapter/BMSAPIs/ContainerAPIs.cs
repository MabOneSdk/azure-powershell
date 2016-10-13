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

using Microsoft.Azure.Commands.RecoveryServices.Backup.Helpers;
using Microsoft.Azure.Commands.RecoveryServices.Backup.Properties;
using Microsoft.Azure.Management.RecoveryServices.Backup.Models;
using Microsoft.Rest.Azure;
using Microsoft.Rest.Azure.OData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.ServiceClientAdapterNS
{
    public partial class ServiceClientAdapter
    {
        /// <summary>
        /// Fetches protection containers in the vault according to the query params
        /// </summary>
        /// <param name="parameters">Query parameters</param>
        /// <returns>List of protection containers</returns>
        public IEnumerable<ProtectionContainerResource> ListContainers(
            ODataQuery<BMSContainerQueryObject> queryFilter,
            string skipToken = default(string))
        {
            Func<IPage<ProtectionContainerResource>> listAsync =
                () => BmsAdapter.Client.ProtectionContainers.ListWithHttpMessagesAsync(
                    BmsAdapter.GetResourceName(),
                    BmsAdapter.GetResourceGroupName(),
                    queryFilter,
                    cancellationToken: BmsAdapter.CmdletCancellationToken).Result.Body;

            Func<string, IPage<ProtectionContainerResource>> listNextAsync =
                nextLink => BmsAdapter.Client.ProtectionContainers.ListNextWithHttpMessagesAsync(
                    nextLink,
                    cancellationToken: BmsAdapter.CmdletCancellationToken).Result.Body;

            return HelperUtils.GetPagedList(listAsync, listNextAsync);
        }

        ///// <summary>
        ///// Fetches backup engines in the vault according to the query params
        ///// </summary>
        ///// <param name="parameters">Query parameters</param>
        ///// <returns>List of backup engines</returns>
        //public IEnumerable<BackupEngineResource> ListBackupEngines(BackupEngineListQueryParams queryParams)
        //{
        //    PaginationRequest paginationParam = new PaginationRequest();
        //    paginationParam.Top = "200";
        //    var listResponse = BmsAdapter.Client.BackupEngines.ListAsync(
        //                                BmsAdapter.GetResourceGroupName(), 
        //                                BmsAdapter.GetResourceName(), 
        //                                queryParams, 
        //                                paginationParam, 
        //                                cancellationToken: BmsAdapter.CmdletCancellationToken).Result;
        //    return listResponse.ItemList.BackupEngines;
        //}

        /// <summary>
        /// Fetches backup engines in the vault according to the query params
        /// </summary>
        /// <param name="parameters">Query parameters</param>
        /// <returns>List of backup engines</returns>
        public IEnumerable<BackupEngineBaseResource> ListBackupEngines(ODataQuery<BMSBackupEngineQueryObject> queryParams)
        {
            Func<IPage<BackupEngineBaseResource>> listAsync =
                () => BmsAdapter.Client.BackupEngines.GetWithHttpMessagesAsync(
                    BmsAdapter.GetResourceName(),
                    BmsAdapter.GetResourceGroupName(),
                    queryParams,
                    cancellationToken: BmsAdapter.CmdletCancellationToken).Result.Body;

            Func<string, IPage<BackupEngineBaseResource>> listNextAsync =
                nextLink => BmsAdapter.Client.BackupEngines.GetNextWithHttpMessagesAsync(
                    nextLink,
                    cancellationToken: BmsAdapter.CmdletCancellationToken).Result.Body;
            var listResponse = HelperUtils.GetPagedList(listAsync, listNextAsync);
            return listResponse;
        }

        /// <summary>
        /// Triggers refresh of container catalog in service
        /// </summary>
        /// <returns>Response of the job created in the service</returns>
        public Microsoft.Rest.Azure.AzureOperationResponse RefreshContainers()
        {
            string resourceName = BmsAdapter.GetResourceName();
            string resourceGroupName = BmsAdapter.GetResourceGroupName();
            var response = BmsAdapter.Client.ProtectionContainers.RefreshWithHttpMessagesAsync(
                                        resourceName,
                                        resourceGroupName, 
                                        AzureFabricName,
                                        cancellationToken: BmsAdapter.CmdletCancellationToken).Result;
            return response;
        }

        /// <summary>
        /// Triggers unregister of a container in service
        /// </summary>
        public Microsoft.Rest.Azure.AzureOperationResponse UnregisterContainers(string containerName)
        {
            string resourceName = BmsAdapter.GetResourceName();
            string resourceGroupName = BmsAdapter.GetResourceGroupName();
            
            var response = BmsAdapter.Client.ProtectionContainers.UnregisterWithHttpMessagesAsync(
                                        resourceName,
                                        resourceGroupName,
                                        containerName,
                                        cancellationToken: BmsAdapter.CmdletCancellationToken).Result;
            return response;
        }
    }
}
