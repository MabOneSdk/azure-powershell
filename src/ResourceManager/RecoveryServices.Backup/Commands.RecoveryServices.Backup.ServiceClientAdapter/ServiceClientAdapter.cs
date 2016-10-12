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

using Microsoft.Azure.Commands.Common.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RecoveryServicesModelsNS = Microsoft.Azure.Management.RecoveryServices.Backup.Models;
using RecoveryServicesNS = Microsoft.Azure.Management.RecoveryServices.Backup;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Cmdlets.ServiceClientAdapterNS
{
    public partial class ServiceClientAdapter
    {
        const string AppSettingsSectionName = "appSettings";
        const string ProviderNamespaceKey = "ProviderNamespace";
        const string AzureFabricName = "Azure";
        public const string ResourceProviderProductionNamespace = "Microsoft.RecoveryServices";

        ClientProxy<RecoveryServicesNS.RecoveryServicesBackupClient> BmsAdapter;

        public string ResourceProviderNamespace { get; private set; }

        public ServiceClientAdapter(AzureContext context)
        {
            BmsAdapter = new ClientProxy<RecoveryServicesNS.RecoveryServicesBackupClient>(context);

            System.Configuration.Configuration exeConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.Configuration.AppSettingsSection appSettings = (System.Configuration.AppSettingsSection)exeConfiguration.GetSection(AppSettingsSectionName);
            string resourceProviderNamespace = ResourceProviderProductionNamespace;
            if (appSettings.Settings[ProviderNamespaceKey] != null)
            {
                resourceProviderNamespace = appSettings.Settings[ProviderNamespaceKey].Value;
            }

            ResourceProviderNamespace = resourceProviderNamespace;
        }
    }
}
