namespace CloudshareAgent
{
  using System.IO;
  using System.Text.RegularExpressions;
  using System.Threading;
  using System.Web;

  /// <summary>
  /// Summary description for Install1
  /// </summary>
  public class Install : IHttpHandler
  {
    /// <summary>
    /// The process request.
    /// </summary>
    /// <param name="context">
    /// The context.
    /// </param>
    public void ProcessRequest(HttpContext context)
    {
      context.Response.ContentType = "text/plain";
      if (context.Request.Params["zip"] != null)
      {
        // run mounting of Cloudshare folder. They should be available in current session
        var filename = context.Request.Params["zip"];
        System.Diagnostics.Process proc = new System.Diagnostics.Process(); 
        proc.StartInfo.FileName = @"C:\Windows\mount.bat";
        proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        proc.StartInfo.CreateNoWindow = true;
        proc.Start();
        Thread.Sleep(1000);

        var mountFileContent = File.ReadAllText(@"C:\windows\mount.bat");
        var regex = new Regex("use \"(.+)\"");
        var dir = regex.Match(mountFileContent);

        // conf.txt contains full path to Jetstream arhive on Cloudshare folders
        var pathToInstall = string.Format("{0}\\{1}\\{2}", 
          dir.Groups[1].Value, 
          context.Request.Params["subfolder"], 
          filename);
        File.WriteAllText(@"c:\installer\conf.txt", pathToInstall);

        // Starter.ps1 powershell script that start insallation of Jetstream with Administrator credentials
        var starter = string.Format("$secpasswd = ConvertTo-SecureString \"{0}\" -AsPlainText -Force\n\r" +
          "$mycreds = New-Object System.Management.Automation.PSCredential(\"{1}\", $secpasswd)\n\r" +
          "Write-Output $mycreds\n\r" + "Start-Process powershell c:\\Installer\\Jetstream.ps1 -Credential $mycreds",
          context.Request.Params["password"], 
          context.Request.Params["username"]);

        File.WriteAllText(@"c:\installer\Starter.ps1", starter);
      }
      else
      {
        context.Response.Write("No parameters" + "\r\n");
      }
    }

    /// <summary>
    /// Gets a value indicating whether is reusable.
    /// </summary>
    public bool IsReusable
    {
      get
      {
        return false;
      }
    }
  }
}