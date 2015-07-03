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
using Microsoft.Azure.Commands.AzureBackup.Helpers;
using Microsoft.Azure.Commands.AzureBackup.Models;

namespace Microsoft.Azure.Commands.AzureBackup.Cmdlets
{
    /// <summary>
    /// Create new protection policy
    /// </summary>
    [Cmdlet(VerbsCommon.New, "AzureBackupProtectionPolicy", DefaultParameterSetName = NoScheduleParamSet), OutputType(typeof(AzureBackupProtectionPolicy))]
    public class NewAzureBackupProtectionPolicy : AzureBackupVaultCmdletBase
    {
        protected const string WeeklyScheduleParamSet = "WeeklyScheduleParamSet";
        protected const string DailyScheduleParamSet = "DailyScheduleParamSet";
        protected const string NoScheduleParamSet = "NoScheduleParamSet";

        [Parameter(Position = 1, Mandatory = true, HelpMessage = AzureBackupCmdletHelpMessage.PolicyName)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Position = 2, Mandatory = true, HelpMessage = AzureBackupCmdletHelpMessage.WorkloadType, ValueFromPipelineByPropertyName = true)]
        [ValidateSet("VM", IgnoreCase = true)]
        public string WorkloadType { get; set; }

        [Parameter(Position = 3, Mandatory = true, HelpMessage = AzureBackupCmdletHelpMessage.BackupType, ValueFromPipelineByPropertyName = true)]
        [ValidateSet("Full", IgnoreCase = true)]
        public string BackupType { get; set; }

        [Parameter(ParameterSetName = DailyScheduleParamSet, Position = 4, Mandatory = false, HelpMessage = AzureBackupCmdletHelpMessage.DailyScheduleType)]
        public SwitchParameter Daily { get; set; }

        [Parameter(ParameterSetName = WeeklyScheduleParamSet, Position = 5, Mandatory = true, HelpMessage = AzureBackupCmdletHelpMessage.WeeklyScheduleType)]
        public SwitchParameter Weekly { get; set; }

        [Parameter(Position = 6, Mandatory = true, HelpMessage = AzureBackupCmdletHelpMessage.ScheduleRunTimes, ValueFromPipelineByPropertyName = true)]
        public DateTime ScheduleRunTimes { get; set; }

        [Parameter(Position = 7, Mandatory = true, HelpMessage = AzureBackupCmdletHelpMessage.RetentionType, ValueFromPipelineByPropertyName = true)]
        [ValidateSet("Days", "Weeks", IgnoreCase = true)]
        public string RetentionType { get; set; }

        [Parameter(Position = 8, Mandatory = true, HelpMessage = AzureBackupCmdletHelpMessage.RententionDuration, ValueFromPipelineByPropertyName = true)]
        public int RetentionDuration { get; set; }

        [Parameter(ParameterSetName = WeeklyScheduleParamSet, Position = 9, Mandatory = true, HelpMessage = AzureBackupCmdletHelpMessage.ScheduleRunDays, ValueFromPipelineByPropertyName = true)]
        [Parameter(ParameterSetName = NoScheduleParamSet, Position = 9, Mandatory = false, HelpMessage = AzureBackupCmdletHelpMessage.ScheduleRunDays, ValueFromPipelineByPropertyName = true)]
        [AllowEmptyCollection]
        [ValidateSet("Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday", IgnoreCase = true)]
        public string[] ScheduleRunDays { get; set; }

        public override void ExecuteCmdlet()
        {
            ExecutionBlock(() =>
            {
                base.ExecuteCmdlet();
                WriteDebug("Making client call");

                ProtectionPolicyHelpers.ValidateProtectionPolicyName(Name);
                AzureBackupClient.CheckProtectionPolicyNameAvailability(this.Name);

                var ScheduleType = ProtectionPolicyHelpers.GetScheduleType(ScheduleRunDays, this.ParameterSetName, 
                                    DailyScheduleParamSet, WeeklyScheduleParamSet);

                var backupSchedule = ProtectionPolicyHelpers.FillBackupSchedule(BackupType, ScheduleType, ScheduleRunTimes,
                   RetentionType, RetentionDuration, ScheduleRunDays);
                
                var addProtectionPolicyRequest = new AddProtectionPolicyRequest();
                addProtectionPolicyRequest.PolicyName = this.Name;
                addProtectionPolicyRequest.Schedule = backupSchedule;
                addProtectionPolicyRequest.WorkloadType = Enum.Parse(typeof(WorkloadType), this.WorkloadType, true).ToString();

                AzureBackupClient.AddProtectionPolicy(addProtectionPolicyRequest);
                WriteDebug("Protection policy created successfully");

                var policyInfo = AzureBackupClient.GetProtectionPolicyByName(Name);
                WriteObject(ProtectionPolicyHelpers.GetCmdletPolicy(vault, policyInfo));
            });
        }
    }
}

