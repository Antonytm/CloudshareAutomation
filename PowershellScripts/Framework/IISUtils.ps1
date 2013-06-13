[System.Reflection.Assembly]::LoadFrom("C:\windows\system32\inetsrv\Microsoft.Web.Administration.dll") | out-null;
Import-Module WebAdministration

<#
	.DESCRIPTION
		Creates application pool in IIS
#>
Function Create-AppPool ($siteName, $runtime, $user, $password)
{  
    Write-Output "Site Name: $sitename" 
    Write-Output "AppPool UserName: $user" 

    $serverManager = New-Object Microsoft.Web.Administration.ServerManager;
    
    # Remove old AppPool (if exists with the came name)
    if ($serverManager.ApplicationPools[$siteName] -ne $NULL)
    {
        Write-Output "Old App Pool will be removed."
        $serverManager.ApplicationPools.Remove($serverManager.ApplicationPools[$siteName])
    }
        
    $appPool = $serverManager.ApplicationPools.Add($siteName);
    Write-Output "AppPool Created"

    $appPool.ManagedRuntimeVersion = $runtime

    "Setting AppPool identity."
    $appPool.ProcessModel.username = [string]($user)
    $appPool.ProcessModel.password = [string]($password)
    $appPool.ProcessModel.identityType = "NetworkService"
    $appPool.ProcessModel.IdleTimeout = [TimeSpan] "0.00:00:00"
    $appPool.Recycling.PeriodicRestart.time = [TimeSpan] "00:00:00"
    "AppPool identity set."  
    $serverManager.CommitChanges();    
    # Wait for the changes to apply
    Start-sleep -milliseconds 1000
}

<#
	.DESCRIPTION
		Creates website in IIS
#>
Function Create-Site ($siteName, $websiteUrl, $webroot)
{
    Write-Output "Website folder: $webroot" 
    $serverManager = New-Object Microsoft.Web.Administration.ServerManager;
   
    # Remove old site (if exists with the came name)
    if ($serverManager.Sites[$siteName] -ne $NULL) 
    {
        "Old site will be removed."
        $serverManager.Sites.Remove($serverManager.Sites[$siteName])
    }
    
    $webSite = $serverManager.Sites.Add($siteName, "http", ":80:$websiteUrl", $webroot + "\website");
    $webSite.Applications[0].ApplicationPoolName = $siteName;
    Write-Output "Website Created"
    Start-sleep -milliseconds 1000
    $serverManager.CommitChanges();    
    
    # Wait for the changes to apply
    Start-sleep -milliseconds 1000
}

Function Add-To-Hosts ($sitehost)
{
    $hostFile = "C:\Windows\System32\drivers\etc\hosts"
    $hostContent = [string](Get-Content $hostFile)

    if($hostContent.Contains("$sitehost") -eq $false)
    {
        Add-Content $hostFile "`n`r127.0.0.1   $sitehost"
    }

    Write-Output "$sitehost added to hosts file"
}

Write-Host "IISUtils Loaded"