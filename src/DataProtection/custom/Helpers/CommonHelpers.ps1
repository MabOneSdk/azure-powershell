﻿
function GetDatasourceSetInfo
{
	param(
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202001Alpha.IDatasource]
		$DatasourceInfo
	)

	process 
	{
		$DataSourceSetInfo = [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202001Alpha.DatasourceSet]::new()
		$DataSourceSetInfo.DatasourceType = $DatasourceInfo.Type
        $DataSourceSetInfo.ObjectType = "DatasourceSet"
        $splitResourceId = $DatasourceInfo.ResourceId.Split("/")
        $DataSourceSetInfo.ResourceId = $splitResourceId[0..($splitResourceId.Count -3)] | Join-String -Separator '/'
        $DataSourceSetInfo.ResourceLocation = $DatasourceInfo.ResourceLocation
        $DataSourceSetInfo.ResourceName = $splitResourceId[$splitResourceId.Count -3]
        $splitResourceType = $DatasourceInfo.ResourceType.Split("/")
        $DataSourceSetInfo.ResourceType =  $splitResourceType[0..($splitResourceType.Count -2)] | Join-String -Separator '/'     # Use manifest for datasource set type
        $DataSourceSetInfo.ResourceUri = ""

		return $DataSourceSetInfo
	}
}

function GetDatasourceInfo
{
	param(
		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[System.String]
		$ResourceId,

		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[System.String]
		$ResourceLocation,

		[Parameter(Mandatory=$true)]
		[ValidateNotNullOrEmpty()]
		[System.String]
		$DatasourceType
	)

	process
	{
		$manifest = LoadManifest -DatasourceType $DatasourceType.ToString()
		$DataSourceInfo = [Microsoft.Azure.PowerShell.Cmdlets.DataProtection.Models.Api202001Alpha.Datasource]::new()
		$DataSourceInfo.ObjectType = "Datasource"
        $DataSourceInfo.ResourceId = $ResourceId
        $DataSourceInfo.ResourceLocation = $ResourceLocation
        $DataSourceInfo.ResourceName = $ResourceId.Split("/")[-1]
        $DataSourceInfo.ResourceType = $manifest.resourceType
        $DataSourceInfo.ResourceUri = ""
        if($manifest.isProxyResource -eq $false)
        {
            $DataSourceInfo.ResourceUri = $ResourceId
        }
        $DataSourceInfo.Type = $manifest.datasourceType

		return $DataSourceInfo
	}
}