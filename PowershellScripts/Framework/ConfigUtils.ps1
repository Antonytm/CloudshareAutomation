<#
	.DESCRIPTION
		Setup web.config: sitecore.net - specific stuff. TODO: Should be refactored.
#>
function Setup-WebConfig([string]$webroot, [string] $coveoServer)
{
    $configTemplatePath = "$webroot\website\Web.config.template"
    If (Test-Path $configTemplatePath)
    {
	   move $configTemplatePath "$webroot\website\Web.config" -Force -Verbose
    }
    
    Set-ConfigAttribute "$webroot\website\web.config" "sitecore/sc.variable[@name='dataFolder']" "value" "$webroot\data"    
    Set-ConfigAttribute "$webroot\website\web.config" "coveoEnterpriseSearch/server" "hostname" $coveoServer
}

<#
	.DESCRIPTION
		Setup connection strings
#>
function Setup-ConnectionStrings([string]$webroot, [string]$sitename, [xml]$config)
{	
	$databaseServer = $databaseServer = $config.InstallSettings.DatabaseDeployment.DatabaseServer
	$baseConnectionString = "Trusted_Connection=Yes;Data Source=$databaseServer;Database="
	
	# define database connections
	$databaseConnections = @{
						"core" = "$sitename" + "_Core";
						"master" = "$sitename" + "_Master";
						"web" = "$sitename" + "_Web";						
						"CT3Translation" = "sitecore.net_CT3Translation";
                        "ElmahConnString" = "sitecore.net_ELMAH"; }

    # define external connections, like CRM, AD, SAC
    $CRM = $config.InstallSettings.ConnectionStrings.CRM
	$AD = $config.InstallSettings.ConnectionStrings.AD
	$SAC = $config.InstallSettings.ConnectionStrings.SAC
	$DMS = $config.InstallSettings.ConnectionStrings.DMS
	
	$externalConnections = @{   
								"analytics" = "$DMS".Replace("(Source)" , $config.InstallSettings.DatabaseDeployment.DMSServer);
								"CRMConnString" = "$CRM";
								"ManagersConnString" = "$AD";
								"SitecoreAppCenter" = "$SAC" }
		
	# edit connectionStrings.config start
	Write-Output "Editing connectionStrings.config ..."
	$connectionStringsConfigTemplatePath = "$webroot\website\App_Config\connectionStrings.config.template"
	$connectionStringsConfigPath = $connectionStringsConfigTemplatePath.Substring(0, $connectionStringsConfigTemplatePath.LastIndexOf('.'))
	$healthMonitorConfigPath = "C:\inetpub\wwwroot\HealthMonitor\App_Config\ConnectionStrings.config"
    
	# get content of connectionStrings.config.template
	$connectionStringsConfig = [xml](get-content $connectionStringsConfigTemplatePath)	
		
	foreach ($db in $databaseConnections.Keys)
	{
	    $connectionStringsConfig.SelectSingleNode("connectionStrings/add[@name='$db']").SetAttribute("connectionString", $baseConnectionString + $databaseConnections[$db]);
	}
	
	foreach ($connection in $externalConnections.Keys)
	{
	    $connectionStringsConfig.SelectSingleNode("connectionStrings/add[@name='$connection']").SetAttribute("connectionString", $externalConnections[$connection]);
	}
	
	# save xml content to connectionStrings.config
	$connectionStringsConfig.Save($connectionStringsConfigPath)
    Write-Output "$connectionStringsConfigPath saved"
    
    if (Test-Path $healthMonitorConfigPath)
    {
    	$connectionStringsConfig.Save($healthMonitorConfigPath)
        Write-Output "$healthMonitorConfigPath saved"
    }
	
	Write-Output "Editing connectionStrings.config Done."                              
}


<#
	.DESCRIPTION
		Setup forms.config 
#>
function Setup-FormsConfig([string]$webroot, [string]$config)
{	
	Write-Output "Setup forms.config"
	
	<#$formsConfigTemplatePath = "$webroot\website\App_Config\Include\forms.config.template"
	$formsConfigPath = $formsConfigTemplatePath.Substring(0, $formsConfigTemplatePath.LastIndexOf('.'))#>
    
    $formsConfigPath = "$webroot\Website\App_Config\Include\forms.config"

	#get content of forms.config.template
	$formsConfig = [xml](get-content $formsConfigPath)	
		
	$connectionString = $formsConfig.configuration.sitecore.SelectSingleNode("formsDataProvider/param[@desc='connection string']")
	$connectionString.InnerText = $config
			
	# save xml content to connectionStrings.config
	$formsConfig.Save($formsConfigPath)
}

<#
	.DESCRIPTION
		Setup sitecore.website.config
#>
function Setup-SitecoreWebsiteConfig([string]$webroot, [string]$boostServer, [string] $emailAddress)
{
	$websiteConfigPath = "$webroot\website\App_Config\Include\Sitecore.Website.config"
    $websiteConfig = [xml](get-content $websiteConfigPath)
	$pingConnection = $websiteConfig.configuration.sitecore.SelectSingleNode("settings/setting[@name='PingConnectionString']").GetAttribute("value");
	$boostConnection = $websiteConfig.configuration.sitecore.SelectSingleNode("settings/setting[@name='Boost.ConnectionString']").GetAttribute("value");

    Set-SitecoreSetting $websiteConfigPath "PingConnectionString" $pingConnection.Replace("(source)" ,$boostServer)
    Set-SitecoreSetting $websiteConfigPath "Boost.ConnectionString" $boostConnection.Replace("(source)" ,$boostServer)
    Set-SitecoreSetting $websiteConfigPath "ErrorNotificationEmail" $emailAddress
}


<#
	.DESCRIPTION
		Uncomment analytics scheduling
#>
function Uncomment-AnalyticsSchedule([string]$webroot)
{	
	Write-Output "uncommentAnalyticsSchedule - started"
	
	$analyticsConfigPath = "$webroot\website\App_Config\Include\Sitecore.Analytics.config"
    Uncomment-ConfigSection $analyticsConfigPath "Sitecore.Components.OmsContrib.BusinessLayer.Tasks.SubscriptionTask, Sitecore.Components.OmsContrib"
    Uncomment-ConfigSection $analyticsConfigPath "Sitecore.Analytics.Tasks.EmailReportsTask"
    Uncomment-ConfigSection $analyticsConfigPath "Sitecore.Analytics.Tasks.UpdateReportsSummaryTask"    
}

<#
	.DESCRIPTION
		Uncomment ECM scheduling
#>
function Uncomment-ECMSchedule([string]$webroot)
{	
	Write-Output "uncommentECMSchedule - started"
	
	$ecmConfigPath = "$webroot\website\App_Config\Include\Sitecore.EmailCampaign.config"
    Uncomment-ConfigSection $ecmConfigPath "<scheduling>"  
}

<#
	.DESCRIPTION
		Enable content freeze warning
#>
function Enable-ContentFreezeWarning([string]$webroot)
{	
    Write-Output "EnableContentFreezeWarning - started"
	
	$utilsConfigPath = "$webroot\website\App_Config\Include\Sitecore.Components.Utils.config"	    
    Set-SitecoreSetting $utilsConfigPath "EnableContentFreezeWarning" "true"
}

<#
	.DESCRIPTION
		Enable CT3 config
#>
function Enable-CT3Config([string]$webroot)
{	
	$CT3ConfigPath = "$webroot\website\App_Config\Include\CT3Translation.config.template"
    $CT3ConfigEnabledPath = "$webroot\website\App_Config\Include\CT3Translation.config"
    
    $CT3Config = Get-Item $CT3ConfigPath  -ErrorAction SilentlyContinue
    if ($CT3Config -ne $NULL)
    {
        Copy-Item $CT3ConfigPath $CT3ConfigEnabledPath -Force -Verbose
    }
}

<#
	.DESCRIPTION
		Disabe CT3 config of live site
#>
function Disable-CT3Config([string]$webroot)
{	
	$CT3ConfigPath = "$webroot\website\App_Config\Include\CT3Translation.config"
    $CT3ConfigDisabledPath = "$webroot\website\App_Config\Include\CT3Translation.config.disabled"
        
    if (Test-Path $CT3ConfigPath)
    {
        $CT3Config = Get-Item $CT3ConfigPath  -ErrorAction SilentlyContinue
        Rename-Item $CT3ConfigPath $CT3ConfigDisabledPath -Verbose
    }
    else
    {
        Write-Output "CT3 config not found."
    }
}

<#
	.DESCRIPTION
		Disable Analytics Lookups
#>
function Disable-AnalyticsLookups([string]$webroot)
{	
	$analyticsConfigPath = "$webroot\website\App_Config\Include\Sitecore.Analytics.config"    
    Set-SitecoreSetting $analyticsConfigPath "Analytics.PerformLookup" "false"    
	
	Write-Output "Analytics lookups disabled."
}

<#
	.DESCRIPTION
		Enable Shell Redirect
        For example - redirect from /sitecore to cms.sitecore.net
#>
function Enable-ShellRedirect([string]$webroot)
{	
	$redirectConfigPath = "$webroot\website\sitecore\web.config.disabled"
	$redirectConfigEnabledPath = "$webroot\website\sitecore\web.config"

    if (Test-Path $redirectConfigPath)
    {
        $redirectConfig = Get-Item $redirectConfigPath  -ErrorAction SilentlyContinue
        Rename-Item $redirectConfigPath $redirectConfigEnabledPath -Verbose
    }
    
	Write-Output "Shell redirect enabled"
}

<#
	.DESCRIPTION
		Disable Analytics for all sites in <sites> section of Web.config
#>
function Disable-SitesAnalytics([string]$webroot)
{	
    Write-Output "Disabling analytics for <sites>" 
	$webConfigPath = "$webroot\website\Web.config"
    $webConfig = [xml](get-content $webConfigPath)

    foreach ($i in $webConfig.SelectNodes("/configuration/sitecore/sites"))
    {
        foreach ($site in $i.ChildNodes) 
        {
            $site.SetAttribute("enableAnalytics", "false")                    
        }
    }  
   
    $webConfig.Save($webConfigPath)
}

<#
	.DESCRIPTION
		Set AutomationMachineName setting in config
#>
function Set-AutomationMachineName([string]$webroot, [string]$machineName)
{	
	Write-Output "Setting Automation.MachineName to $machineName"
	$analyticsConfigPath = "$webroot\website\App_Config\Include\Sitecore.Analytics.config"    
    Set-SitecoreSetting $analyticsConfigPath "Analytics.Automation.MachineName" $machineName
}


<#
	.DESCRIPTION
		Set Execution Timeout
#>
function Set-ExecutionTimeout([string]$webroot, [string] $timeout)
{
	Write-Output "Setting Execution Timeout in web.config to $timeout"
	
    $webConfigPath = "$webroot\website\web.config"	
    Set-ConfigAttribute $webConfigPath "system.web/httpRuntime" "executionTimeout" $timeout
}

# Set CDN.CdnEnabled setting in config
<#
	.DESCRIPTION
		Set CDN.CdnEnabled setting in config
#>
function Enable-Cdn([string]$webroot, [string]$cdnEnabled)
{	
	Write-Output "Enabling CDN"
	
	$cdnConfigPath = "$webroot\website\App_Config\Include\Sitecore.Components.Cdn.config"    
    Set-SitecoreSetting $cdnConfigPath "CDN.CdnEnabled" $cdnEnabled    
}

<#
	.DESCRIPTION
		Uncomment CDN scheduling
#>
function Uncomment-CdnSchedule([string]$webroot)
{
    Write-Output "Uncommenting CDN schedule"
	
	$cdnConfigPath = "$webroot\website\App_Config\Include\Sitecore.Components.Cdn.config"
    Uncomment-ConfigSection $cdnConfigPath "Sitecore.Components.Cdn.BusinessLayer.Tasks.InvalidateCdnItems, Sitecore.Components.Cdn"        
}

# Enable LocalMTA
<#
	.DESCRIPTION
		
#>
function EnableLocalMTA([string]$webroot)
{
	Write-Output "Enabling local MTA"
	
	$websiteConfigPath = "$webroot\website\App_Config\Include\Sitecore.EmailCampaign.config"
    Set-SitecoreSetting $websiteConfigPath "UseLocalMTA" "true"    
    Set-SitecoreSetting $websiteConfigPath "SMTP.AuthMethod" "NONE"         
}

<#
	.DESCRIPTION
		Disable Chars Validation
#>
function Disable-CharsValidation([string]$webroot)
{
	Write-Output "Disabling Chars Validation"
	
	$webConfigPath = "$webroot\website\web.config"    
    Set-SitecoreSetting $webConfigPath "InvalidItemNameChars" ""
	Set-SitecoreSetting $webConfigPath "ItemNameValidation" "^[\w\*\$][\.\w\s\-\$]*(\(\d{1,}\)){0,1}$" 	
}

<#
	.DESCRIPTION
		Enable Chars Validation
#>
function Enable-CharsValidation([string]$webroot)
{
	Write-Output "Enabling Chars Validation"
	
	$webConfigPath = "$webroot\website\web.config"
	$InvalidItemNameChars = $config.InstallSettings.CustomSettings.InvalidItemNameChars
    Set-SitecoreSetting $webConfigPath "InvalidItemNameChars" $InvalidItemNameChars
	Set-SitecoreSetting $webConfigPath "ItemNameValidation" "^[\w\*\$][\w\s\-\$]*(\(\d{1,}\)){0,1}$" 	
}

<#
	.DESCRIPTION
		Set Config Attribute
#>
function Set-ConfigAttribute([string]$webroot, [string] $xpath, [string] $attribute, [string] $value)
{
    Write-Output "Setting attribute $xpath in $configPath to $value"
    
    $configPath	= "$webroot\Website\Web.config"

	$config = [xml](get-content $configPath)
	$config.configuration.SelectSingleNode($xpath).SetAttribute($attribute, $value)	
	$config.Save($configPath)
}

<#
	.DESCRIPTION
		Set Connection String
#>
function Set-ConnectionString([string]$webroot, [string] $connectionStringName, [string] $value)
{
    $configPath = "$webroot\website\App_Config\ConnectionStrings.config"
    Write-Output "Setting connection string $connectionStringName in $configPath to $value"
	
	$config = [xml](get-content $configPath)
	$config.SelectSingleNode("connectionStrings/add[@name='$connectionStringName']").SetAttribute("connectionString", $value)	
	$config.Save($configPath)
}

<#
	.DESCRIPTION
		Set Sitecore Setting
#>
function Set-SitecoreSetting([string]$configPath, [string] $name, [string] $value)
{
    Write-Output "Setting Sitecore setting $name"
	
    $xpath = "settings/setting[@name='" + $name + "']"   
	$config = [xml](get-content $configPath)
	$config.configuration.sitecore.SelectSingleNode($xpath).SetAttribute("value", $value)	
	$config.Save($configPath)
}

<#
	.DESCRIPTION
		Uncomment config file section
#>
function Uncomment-ConfigSection([string]$configPath, [string] $pattern)
{
    Write-Output "Uncommenting section containing text $pattern in $configPath"

    $xDoc = [System.Xml.Linq.XDocument]::Load($configPath)
    $endpoints = $xDoc.Descendants("configuration") | foreach { $_.DescendantNodes()}               
    
    $configSection = $endpoints | Where-Object { $_.NodeType -eq [System.Xml.XmlNodeType]::Comment -and $_.Value -match $pattern }        
    if ($configSection -ne $NULL)
    {    
        $configSection | foreach { $_.ReplaceWith([System.Xml.Linq.XElement]::Parse($_.Value)) }
    }
    
    $emailReportsAgent | foreach { Write-Output $_.Value; }
    $xDoc.Save($configPath)
}

<#
	.DESCRIPTION
		Enable Elmah tool
#>
function Enable-Elmah([string]$webRoot)
{    
    Write-Output "Enableing Elmah"
    
    $webConfig = "$webroot\website\web.config"

    Uncomment-ConfigSection $webConfig "Elmah.ErrorLogModule, Elmah"
    Uncomment-ConfigSection $webConfig "Elmah.ErrorFilterModule, Elmah"
    Uncomment-ConfigSection $webConfig "Elmah.ErrorMailModule, Elmah"
    
    $xpath = "elmah/errorMail"
    $attribute = "subject"
    
    $config = [xml](get-content $webConfig)
	$attrValue = $config.configuration.SelectSingleNode($xpath).GetAttribute($attribute);
    Set-ConfigAttribute $webConfig $xpath $attribute $attrValue.Replace("#SERVERNAME#", [Environment]::MachineName);
}

<#
	.DESCRIPTION
		Turn On Crm Profiling
#>
function Enable-CrmProfiling([string]$webroot)
{
    Write-Output "Enabling CRM Profiling"
    
    $webConfig = "$webroot\website\web.config"
    $crmConfig = "$webroot\website\App_Config\Include\crm.config"
    
    $attributeName = "providerName"
    $targetProvider = "wrapper"
    
    $webConfigContents = [xml](get-content $webConfig)
    
    # Change crm providers for wrappers 
    $xpath = "sitecore/switchingProviders/membership/provider[@providerName='crm']";
    $attrValue = $webConfigContents.configuration.SelectSingleNode($xpath);
    if($attrValue -ne $NULL)
    {
        Set-ConfigAttribute $webConfig $xpath $attributeName $targetProvider;
    }
    
    $xpath = "sitecore/switchingProviders/roleManager/provider[@providerName='crm']";
    $attrValue = $webConfigContents.configuration.SelectSingleNode($xpath);
    if($attrValue -ne $NULL)
    {
        Set-ConfigAttribute $webConfig $xpath $attributeName $targetProvider;        
    }
    
    $xpath = "sitecore/switchingProviders/profile/provider[@providerName='crm']";
    $attrValue = $webConfigContents.configuration.SelectSingleNode($xpath);
    if($attrValue -ne $NULL)
    {
        Set-ConfigAttribute $webConfig $xpath $attributeName $targetProvider;        
    }            
    
    # Turn on crm profiling setting
    Set-SitecoreSetting $crmConfig "Crm.CrmAccessProfiling" "true";
        
    Write-Output "Enable-CrmProfiling - done"
}

Write-Host "ConfigUtils Loaded"