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
using Microsoft.WindowsAzure.Commands.Utilities.Common;
using Microsoft.Azure.Common.Authentication;
using Microsoft.Azure.Common.Authentication.Models;
using System.Threading;
using Hyak.Common;
using Microsoft.Azure.Commands.AzureBackup.Properties;
using System.Net;
using System.Linq;
using Microsoft.WindowsAzure.Management.Scheduler;
using Microsoft.Azure.Management.BackupServices;
using Microsoft.Azure.Management.BackupServices.Models;

namespace Microsoft.Azure.Commands.AzureBackup.ClientAdapter
{
    public partial class AzureBackupClientAdapter
    {
        /// <summary>
        /// Gets all containers in the vault
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<CSMContainerResponse> ListContainers(string filter)
        {
            CSMContainerListOperationResponse listResponse = AzureBackupClient.Container.ListAsync(filter, GetCustomRequestHeaders(), CmdletCancellationToken).Result;
            return listResponse.CSMContainerListResponse.Value;
        }

        /// <summary>
        /// Register container
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public Guid RegisterContainer(string containerName)
        {
            var response = AzureBackupClient.Container.RegisterAsync(containerName, GetCustomRequestHeaders(), CmdletCancellationToken).Result;
            return response.OperationId;
        }

        /// <summary>
        /// UnRegister container
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns></returns>
        public Guid UnRegisterContainer(string containerName)
        {
            var response = AzureBackupClient.Container.UnregisterAsync(containerName, GetCustomRequestHeaders(), CmdletCancellationToken).Result;
            return response.OperationId;
        }

        /// <summary>
        /// Refresh container list in service
        /// </summary>
        /// <returns></returns>
        public Guid RefreshContainers()
        {
            var response = AzureBackupClient.Container.RefreshAsync(GetCustomRequestHeaders(), CmdletCancellationToken).Result;
            return response.OperationId;
        }
    }
}