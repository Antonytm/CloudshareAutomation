
namespace CloudshareAPIWrapper
{
  using System;
  using System.Configuration;
  using System.Linq;
  using System.Net;
  using System.Windows.Forms;
  using CloudshareAPIWrapper.Model;
  using WinSCP;

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
    private CloudshareClient client;

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

    public MainForm()
    {
      this.InitializeComponent();

      var ac = new CloudshareAccount();
      ac.ApiId = ConfigurationManager.AppSettings["ApiId"];
      ac.ApiKey = ConfigurationManager.AppSettings["ApiKey"];
      ac.ServerUrl = ConfigurationManager.AppSettings["ServerUrl"];

      this.client = new CloudshareClient(ac);

      this.environmentStates = this.client.GetAllAvailableBlueprints();

      this.cloudFoldersInfo = this.client.GetCloudFoldersInfo();

      var blueprints = (from data in this.environmentStates.data
                        from blueprint in data.Blueprints
                        orderby blueprint.Name
                        select blueprint.Name).Distinct().ToList();

      this.BlueprintsComboBox.DataSource = blueprints;

      var snapshots = (from data in this.environmentStates.data
                       from blueprint in data.Blueprints
                       from snapshot in blueprint.Snapshots
                       where blueprint.Name == this.BlueprintsComboBox.SelectedItem
                       orderby snapshot.Name
                       select snapshot.Name).ToList();
      this.SpapshotsComboBox.DataSource = snapshots.ToList();
    }

    private void InstallButton_Click(object sender, EventArgs e)
    {
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

      var environmentName = string.Format("CloudahreAutomation{0}", DateTime.Now.Ticks);
      var a = this.client.CreateBlueprintFromSnapshot(selectedData.EnvironmentPolicyId, selectedSnapshot.SnapshotId, environmentName, selectedBluprint.Name, selectedData.EnvironmentPolicyDuration);
      this.UploadZip();

      var env = client.GetEnvironmentsList();
      var envId = (from data in env.data
                   where data.name == environmentName
                   select data.envId).FirstOrDefault();

      this.networkFolder = this.client.Mount(envId);

      var vms = client.GetEnvironmentState(envId);
      var vmId = (from dat in vms.data.vms
                  select vms.data.vms[0].vmId).FirstOrDefault();

      var req = WebRequest.Create(@"http://" + vms.data.vms[0].FQDN + @"/CloudshareAgent/Install.ashx?zip=" + this.filename + @"&subfolder=" + this.remoteUserFolderName.Replace(" ", "-").Replace("'", "-").Replace("(", "-").Replace(")", "-")
        + @"&username=" + vms.data.vms[0].username + @"&password=" + vms.data.vms[0].password);
      req.Method = "GET";

      //TODO: response checker if anything is OK
      var response = req.GetResponse();

      var executedPath = client.ExecutePath(envId, vmId, @"c:\installer\Jetstream.bat");
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
      SessionOptions sessionOptions = new SessionOptions
      {
        Protocol = Protocol.Ftp,
        HostName = this.cloudFoldersInfo.data.host,
        UserName = this.cloudFoldersInfo.data.user,
        Password = this.cloudFoldersInfo.data.password
      };
      using (Session session = new Session())
      {
        // Connect
        session.Open(sessionOptions);

        // Upload files
        TransferOptions transferOptions = new TransferOptions();
        transferOptions.TransferMode = TransferMode.Binary;

        var a = session.ListDirectory("/");
        this.remoteUserFolderName = a.Files[2].Name;

        TransferOperationResult transferResult;
        transferResult = session.PutFiles(fileToUpload, "/" + a.Files[2].Name + "/*.*", false, transferOptions);

        // Throw on any error
        transferResult.Check();
      }
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
  }
}
