CloudshareAutomation
====================

Automation tool for deploy Sitecore/Jetstream instances to Cloudshare environments


Quick configuration guide:
Cloudshare environment part:
1)  Copy all files from PowershellScripts to C:\Installer
2)	Copy license.xml file to C:\Installer\files
3)	Add web application on IIS from CloudshareAgent folder (It should be available on http://localhost/CloudshareAgent/Install.ashx address)
4)	Make sure that there is binding to all Unassigned IP addresses for web application (It should be available on http://YourAddressOfVirtualMachineOnWeb/CloudshareAgent/Install.ashx address)
Application part:
1)	Set ApiId to Cloudshare.exe.config file
 <add key="ApiId" value="XXXXXXX"/>
ApiId could be found on user details page on cloudshare site
2)	Set ApiKey to Cloudshare.exe.config file
<add key="ApiKey" value="XXXXXXX"/>
ApiKey could be found on user details page on cloudshare site

