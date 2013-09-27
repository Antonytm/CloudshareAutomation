using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Cloudshare.Controls;
using CloudshareAPIWrapper;
using CloudshareAPIWrapper.Model;
using System.Collections.Generic;
using WinSCP;

namespace Cloudshare
{
  /// <summary>
  /// The main form.
  /// </summary>
  public partial class MainForm : Form
  {
    /// <summary>
    /// The environment states.
    /// </summary>
    private Blueprints environmentStates;

    /// <summary>
    /// The client.
    /// </summary>
    private CloudshareAPIWrapper.CloudshareClient client;

    /// <summary>
    /// The cloud folders info.
    /// </summary>
    private CloudFoldersInfo cloudFoldersInfo;

    /// <summary>
    /// The remote user folder name.
    /// </summary>
    private string remoteUserFolderName;

    /// <summary>
    /// The filename of zip arhive.
    /// </summary>
    private string filename;

    /// <summary>
    /// The network folder.
    /// </summary>
    private NetworkFolder networkFolder;


    /// <summary>
    /// List of Blueprints
    /// </summary>
    private List<string> _blueprints;

    /// <summary>
    /// List of Snapshots
    /// </summary>
    private List<string> _snapshots;

    /// <summary>
    /// List of Alive Environments
    /// </summary>
    private List<string> _aliveEnvironments;

    public MainForm()
    {
      this.InitializeComponent();

      InitData();

      this.BlueprintsComboBox.DataSource = _blueprints;
      this.SpapshotsComboBox.DataSource = _snapshots.ToList();

      lbLogs.Items.Add("Runned!");
    }

    private void InitData()
    {
      var cloudshareAccount = new CloudshareAccount
                 {
                   ApiId = ConfigurationManager.AppSettings["ApiId"],
                   ApiKey = ConfigurationManager.AppSettings["ApiKey"],
                   ServerUrl = ConfigurationManager.AppSettings["ServerUrl"]
                 };

      this.client = new CloudshareAPIWrapper.CloudshareClient(cloudshareAccount);
      this.environmentStates = this.client.GetAllAvailableBlueprints();
      this.cloudFoldersInfo = this.client.GetCloudFoldersInfo();
      var env = client.GetEnvironmentsList();

      _blueprints = (from data in this.environmentStates.data
                     from blueprint in data.Blueprints
                     orderby blueprint.Name
                     select blueprint.Name).Distinct().ToList();
      _snapshots = (from data in this.environmentStates.data
                    from blueprint in data.Blueprints
                    from snapshot in blueprint.Snapshots
                    where blueprint.Name == this.BlueprintsComboBox.SelectedItem
                    orderby snapshot.Name
                    select snapshot.Name).ToList();


      _aliveEnvironments = (from data in env.data
                            where data.status_code == 0
                            select data.name).Distinct().ToList();
    }

    private void InstallButton_Click(object sender, EventArgs e)
    {
      bpLoadFile.DisplayStyle = ProgressBarDisplayText.CustomText;
      bpLoadFile.CustomText = "Load package...";
     
      string environmentName;

      if (rbNewEnvironment.Checked)
      {
        environmentName = string.Format("CloudahreAutomation{0}", DateTime.Now.Ticks);

        //TODO: log handling to listbox
        var selectedSnapshot = (from data in environmentStates.data
                                from blueprint in data.Blueprints
                                from snapshot in blueprint.Snapshots
                                where snapshot.Name == this.SpapshotsComboBox.SelectedItem
                                select snapshot).FirstOrDefault();
        var selectedBluprint = (from data in environmentStates.data
                                from blueprint in data.Blueprints
                                from snapshot in blueprint.Snapshots
                                where snapshot.Name == this.SpapshotsComboBox.SelectedItem
                                select blueprint).FirstOrDefault();

        var selectedData = (from data in environmentStates.data
                            from blueprint in data.Blueprints
                            from snapshot in blueprint.Snapshots
                            where snapshot.Name == this.SpapshotsComboBox.SelectedItem
                            select data).FirstOrDefault();

        if (selectedData != null && selectedSnapshot != null && selectedBluprint != null)
        {
          var newBlueprint = this.client.CreateBlueprintFromSnapshot(selectedData.EnvironmentPolicyId,
            selectedSnapshot.SnapshotId,
            environmentName, selectedBluprint.Name,
            selectedData.EnvironmentPolicyDuration);
          if (newBlueprint.status_text == "Success")
            lbLogs.Items.Add(string.Format("New Blueprint {0} has been created!", environmentName));
          else
          {
            lbLogs.Items.Add(string.Format("Create of new Blueprint {0} failed! ", environmentName));
            lbLogs.Items.Add(string.Format("Exception: {0} ", newBlueprint.status_text));
          }
        }
      }
      else
      {
        environmentName = BlueprintsComboBox.SelectedItem.ToString();
      }

      this.UploadZip();
      var env = client.GetEnvironmentsList();
      var envId = (from data in env.data
                   where data.name == environmentName
                   select data.envId).FirstOrDefault();

      if (envId == null) return;

      this.networkFolder = this.client.Mount(envId);
      lbLogs.Items.Add("Network Folder has been mounted!");
      var vms = client.GetEnvironmentState(envId);
      var vmId = (from dat in vms.data.vms
                  select vms.data.vms[0].vmId).FirstOrDefault();

      var req = WebRequest.Create(@"http://" + vms.data.vms[0].FQDN + @"/CloudshareAgent/Install.ashx?zip=" + this.filename + @"&subfolder=" + this.remoteUserFolderName.Replace(" ", "-").Replace("'", "-").Replace("(", "-").Replace(")", "-")
                                  + @"&username=" + vms.data.vms[0].username + @"&password=" + vms.data.vms[0].password);

      lbLogs.Items.Add("WebRequest has been made!");
      req.Method = "GET";
      //TODO: response checker if anything is OK
      var response = req.GetResponse();
      var executedPath = client.ExecutePath(envId, vmId, @"c:\installer\Jetstream.bat");
      lbLogs.Items.Add("Jetstream.bat has been executed!");
      lbLogs.Items.Add("Status of run Jetstream.bat is:" + executedPath.status_text);
      lbLogs.Items.Add("Done!");
      lbLogs.Items.Add("************************************************************");
    

    }


    private void UploadZip()
    {
      /* WinSCP */
      filename = this.FilenameTextBox.Text.Substring(this.FilenameTextBox.Text.LastIndexOf("\\") + 1);
      var fileToUpload = this.FilenameTextBox.Text;

      UploadFile(fileToUpload);
    }

    private void UploadFile(string fileToUpload)
    {
      lbLogs.Items.Add("File to upload: " + fileToUpload);

      bpLoadFile.Value = 0;
      bpLoadFile.DisplayStyle = ProgressBarDisplayText.Percentage;

      SessionOptions sessionOptions = new SessionOptions
      {
        Protocol = Protocol.Ftp,
        HostName = this.cloudFoldersInfo.data.host,
        UserName = this.cloudFoldersInfo.data.user,
        Password = this.cloudFoldersInfo.data.password,
      };

      using (Session session = new Session())
      {
        // Will continuously report progress of transfer
        session.FileTransferProgress += SessionFileTransferProgress;
        // Connect
        session.Open(sessionOptions);
        // Upload files
        TransferOptions transferOptions = new TransferOptions();
        transferOptions.TransferMode = TransferMode.Binary;

        var a = session.ListDirectory("/");
        this.remoteUserFolderName = a.Files[2].Name;
        var filePath = "/" + this.remoteUserFolderName + "/" + filename;
        if (!session.FileExists(filePath))
        {
          TransferOperationResult transferResult;
          transferResult = session.PutFiles(fileToUpload, "/" + this.remoteUserFolderName + "/*.*", false, transferOptions);
          transferResult.Check();
          if (transferResult.IsSuccess)
          {
            bpLoadFile.CustomText = "Succeed!";
          }
          else
          {
            bpLoadFile.CustomText = "Failed!";
          }
        }
        lbLogs.Items.Add("Package has been uploaded!");
        bpLoadFile.DisplayStyle = ProgressBarDisplayText.CustomText;
      }
    }

    private void SessionFileTransferProgress(object sender, FileTransferProgressEventArgs e)
    {
      bpLoadFile.Value = Convert.ToInt32(e.FileProgress * 100);
      bpLoadFile.Update();
    }

    /// <summary>
    /// The blueprints combo box selected index changed.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void BlueprintsComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      var snapshots = (from data in environmentStates.data
                       from blueprint in data.Blueprints
                       from snapshot in blueprint.Snapshots
                       where blueprint.Name == this.BlueprintsComboBox.SelectedItem
                       orderby snapshot.Name
                       select snapshot.Name).ToList();
      this.SpapshotsComboBox.DataSource = snapshots.ToList();
    }

    /// <summary>
    /// The browse button_ click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void BrowseButton_Click(object sender, EventArgs e)
    {
      var result = OpenZipFileDialog.ShowDialog(); // Show the dialog.
      if (result == DialogResult.OK) // Test result.
      {
        var file = OpenZipFileDialog.FileName;
        this.FilenameTextBox.Text = file;
      }
    }

    private void rbNewEnvironment_CheckedChanged(object sender, EventArgs e)
    {
      SpapshotsComboBox.Visible = true;
      lblSnapshotName.Visible = true;
      lblBluePrint.Text = "BluePrint Name";
      this.BlueprintsComboBox.DataSource = _blueprints;
    }

    private void rbAliveEnvironment_CheckedChanged(object sender, EventArgs e)
    {
      SpapshotsComboBox.Visible = false;
      lblSnapshotName.Visible = false;
      lblBluePrint.Text = "Environment Name";
      this.BlueprintsComboBox.DataSource = _aliveEnvironments;
    }
  }
}
