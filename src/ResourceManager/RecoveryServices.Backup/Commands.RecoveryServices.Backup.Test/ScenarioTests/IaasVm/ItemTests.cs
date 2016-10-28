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
using Microsoft.Azure.ServiceManagemenet.Common.Models;
using Microsoft.WindowsAzure.Commands.ScenarioTest;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.Azure.Commands.RecoveryServices.Backup.Test.ScenarioTests
{
    public partial class ItemTests : RMTestBase
    {
        public ItemTests(ITestOutputHelper output)
        {
            XunitTracingInterceptor.AddToContext(new XunitTracingInterceptor(output));
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestGetItemScenario()
        {
            TestController.NewInstance.RunPsTest(PsBackupProviderTypes.IaasVm, "Test-GetItemScenario");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestEnableAzureVMProtectionScenario()
        {
            TestController.NewInstance.RunPsTest(PsBackupProviderTypes.IaasVm, "Test-EnableAzureVMProtectionScenario");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestDisableAzureVMProtectionScenario()
        {
            TestController.NewInstance.RunPsTest(PsBackupProviderTypes.IaasVm, "Test-DisableAzureVMProtectionScenario");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestBackupItemScenario()
        {
            TestController.NewInstance.RunPsTest(PsBackupProviderTypes.IaasVm, "Test-BackupItemScenario");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestGetAzureVMRecoveryPointsScenario()
        {
            TestController.NewInstance.RunPsTest(PsBackupProviderTypes.IaasVm, "Test-GetAzureVMRecoveryPointsScenario");
        }

        [Fact]
        [Trait(Category.AcceptanceType, Category.CheckIn)]
        public void TestRestoreAzureVMItemScenario()
        {
            TestController.NewInstance.RunPsTest(PsBackupProviderTypes.IaasVm, "Test-RestoreAzureVMRItemScenario");
        }
    }
}
