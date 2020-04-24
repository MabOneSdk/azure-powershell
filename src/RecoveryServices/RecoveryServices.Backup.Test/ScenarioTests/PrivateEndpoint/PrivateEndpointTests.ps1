

function Test-AzurePrivateLinkService
{
	$location = "southeastasia"
	$resourceGroupName = Create-ResourceGroup $location
	$subnetConfigName = "PESubnet"
	$vnetName = "PEVnet"
	$privateLinkServiceConnectionName = "PrivateLinkConnectionRecSvc"
	$privateEndpointName = "PrivateEndpointRecSvc"
	try
	{
		$vault = Create-RecoveryServicesVault $resourceGroupName $location
		
        # get private link resource
        $privateLinkResource = Get-AzPrivateLinkResource -PrivateLinkResourceId $vault.ResourceId
        Assert-NotNull $privateLinkResource
        Assert-AreEqual "AzureBackup" $privateLinkResource.GroupId

		# create private endpoint connection
        $subnetConfig = New-AzVirtualNetworkSubnetConfig -Name $subnetConfigName -AddressPrefix "11.0.1.0/24" -PrivateEndpointNetworkPolicies "Disabled"
        $vnet = New-AzVirtualNetwork -ResourceGroupName $resourceGroupName -Name $vnetName -Location $location -AddressPrefix "11.0.0.0/16" -Subnet $subnetConfig
        $privateLinkServiceConnection = New-AzPrivateLinkServiceConnection -Name $privateLinkServiceConnectionName -PrivateLinkServiceId $vault.ResourceId -GroupId $privateLinkResource.GroupId
        New-AzPrivateEndpoint -ResourceGroupName $resourceGroupName -Name $privateEndpointName -Location $location -Subnet $vnet.subnets[0] -PrivateLinkServiceConnection $privateLinkServiceConnection -ByManualRequest
        $privateEndpointConnection = Get-AzPrivateEndpointConnection -PrivateLinkResourceId $vault.ResourceId
        Assert-NotNull $privateEndpointConnection
        Assert-AreEqual "Pending" $privateEndpointConnection.PrivateLinkServiceConnectionState.Status
	}
	finally
	{
		# Cleanup
		Cleanup-ResourceGroup $resourceGroupName
	}
}