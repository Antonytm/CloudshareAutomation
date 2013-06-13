Start-Process c:\windows\mount.bat -NoNewWindow -Wait

$packageStore = "C:\Zip Instalers"
$license = "C:\Installer\files"

$content = [IO.File]::ReadAllText("C:\Installer\conf.txt")
Write-Output "$content"
Test-Path "$content"

$archiv = Get-ChildItem -Path $content
Write-Output "$archiv"
Test-Path "$archiv"

$destination = "C:\inetpub\wwwroot"
$projectFolderName = "Jetstream"

$serverName = "localhost"
$DBForAttach = "Core", "Master", "Web", "Analytics", "WebForms"
$connectionStrDB = "core", "master", "web", "analytics"

$BDUser = "sa"
$BDPass = "12345"
$BDDataSource = "."

$appName = "jetstream"
$siteName = "jetstream"

$infoFile = "C:\Installer\files\info.txt"


$scriptRoot = Split-Path (Resolve-Path $myInvocation.MyCommand.Path)
.$scriptRoot\Framework\FileUtils.ps1
.$scriptRoot\Framework\DBUtils.ps1
.$scriptRoot\Framework\ConfigUtils.ps1
.$scriptRoot\Framework\IISUtils.ps1

$projectFolder = "$destination\$projectFolderName"
$websiteFolder = "$projectFolder\website"
$dataFolder = "$projectFolder\data"
$databasesFolder = "$projectFolder\databases"


$lastInstalDate = [string](Get-Content $infoFile)
$lastArchivDate = $archiv.CreationTime 

if($lastInstalDate.Length -ne 0 ) {
    $lastInstalDateFormated = $lastInstalDate | Get-Date -Format G
}


$downloaded = $true

try {
    $fileAccess = New-Object system.IO.StreamReader $archiv.FullName
}
catch {
    write-host "Archive not downloaded yet"
    $downloaded = $false
}

if($downloaded) {


	$fileAccess.Close()
	$server = New-Object ("Microsoft.SqlServer.Management.Smo.Server") $serverName

	foreach ($db in $DBForAttach) {
		if ($server.databases["Jetstream_$db"] -ne $NULL) {
			$server.KillAllProcesses("Jetstream_$db")
			$server.DetachDatabase("Jetstream_$db", $false)
		}
	}

	if($archiv -ne $NULL) {
		Unzip-Archive $archiv.FullName $destination
	}

	foreach ($db in $DBForAttach) {
		Attach-Database $server "Jetstream_$db" "$databasesFolder\Jetstream_$db.mdf" "$databasesFolder\Jetstream_$db.ldf"
	}

	foreach ($db in $connectionStrDB) {
		Set-ConnectionString $projectFolder $db "user id=$BDUser;password=$BDPass;Data Source=$BDDataSource;Database=Jetstream_$db"
	}

	Setup-FormsConfig $projectFolder "user id=$BDUser;password=$BDPass;Data Source=$BDDataSource;Database=Jetstream_WebForms"

	Set-ConfigAttribute $projectFolder "sitecore/sc.variable[@name='dataFolder']" "value" $dataFolder

	Copy "$license\license.xml" $dataFolder -Verbose

	Copy-Item "$websiteFolder\bin_x64\ChilkatDotNet2.dll" "$websiteFolder\bin" -Force -Verbose

	Create-AppPool $appName "v4.0" "NetworkService" "Password12345"

	Create-Site $appName $siteName $projectFolder

	Add-To-Hosts $siteName

	$lastArchivDateFormated | Out-File $infoFile

	Write-Output "Newest version instaled successed"
    
}