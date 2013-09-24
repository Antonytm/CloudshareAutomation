using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CloudshareAPIWrapper;
using CloudshareAPIWrapper.Model;
using WinSCP;

namespace CloudshareClient
{
    public static class CloudshareManager
    {
        private static CloudshareAPIWrapper.CloudshareClient client;

        /// <summary>
        /// The cloud folders info.
        /// </summary>
        private static CloudFoldersInfo cloudFoldersInfo;

        /// <summary>
        /// The remote user folder name.
        /// </summary>
        private static string remoteUserFolderName;

        public static void InstallZipOnCloudshare(string filename, string blueprintName, string snapshotName)
        {
            var ac = new CloudshareAccount();
            ac.ApiId = ConfigurationManager.AppSettings["ApiId"];
            ac.ApiKey = ConfigurationManager.AppSettings["ApiKey"];
            ac.ServerUrl = ConfigurationManager.AppSettings["ServerUrl"];

            client = new CloudshareAPIWrapper.CloudshareClient(ac);

            var environmentStates = CloudshareManager.client.GetAllAvailableBlueprints();

            cloudFoldersInfo = CloudshareManager.client.GetCloudFoldersInfo();

            var blueprints = (from data in environmentStates.data
                              from blueprint in data.Blueprints
                              orderby blueprint.Name
                              select blueprint.Name).Distinct().ToList();

            var snapshots = (from data in environmentStates.data
                             from blueprint in data.Blueprints
                             from snapshot in blueprint.Snapshots
                             where blueprint.Name == blueprintName
                             orderby snapshot.Name
                             select snapshot.Name).ToList();

            var selectedSnapshot = (from data in environmentStates.data
                                    from blueprint in data.Blueprints
                                    from snapshot in blueprint.Snapshots
                                    where snapshot.IsLatest == true && blueprint.Name == blueprintName
                                    select snapshot).FirstOrDefault();
            var selectedBluprint = (from data in environmentStates.data
                                    from blueprint in data.Blueprints
                                    from snapshot in blueprint.Snapshots
                                    where snapshot.IsLatest == true && blueprint.Name == blueprintName
                                    select blueprint).FirstOrDefault();

            var selectedData = (from data in environmentStates.data
                                from blueprint in data.Blueprints
                                from snapshot in blueprint.Snapshots
                                where snapshot.IsLatest == true && blueprint.Name == blueprintName
                                select data).FirstOrDefault();

            CloudshareManager.client = new CloudshareAPIWrapper.CloudshareClient(ac);
            var environmentName = string.Format("CloudahreAutomation{0}", DateTime.Now.Ticks);
            var a = CloudshareManager.client.CreateBlueprintFromSnapshot(selectedData.EnvironmentPolicyId, selectedSnapshot.SnapshotId, environmentName, selectedBluprint.Name, selectedData.EnvironmentPolicyDuration);
            CloudshareManager.UploadFile(filename);

            var env = client.GetEnvironmentsList();
            var envId = (from data in env.data
                         where data.name == environmentName
                         select data.envId).FirstOrDefault();

            var networkFolder = CloudshareManager.client.Mount(envId);

            var vms = client.GetEnvironmentState(envId);
            var vmId = (from dat in vms.data.vms
                        select vms.data.vms[0].vmId).FirstOrDefault();

            var fileName = Path.GetFileName(filename);
            
            var req = WebRequest.Create(@"http://" + vms.data.vms[0].FQDN + @"/CloudshareAgent/Install.ashx?zip=" + fileName + @"&subfolder=" + CloudshareManager.remoteUserFolderName.Replace(" ", "-").Replace("'", "-").Replace("(", "-").Replace(")", "-")
              + @"&username=" + vms.data.vms[0].username + @"&password=" + vms.data.vms[0].password);
            req.Method = "GET";

            //TODO: response checker if anything is OK
            var response = req.GetResponse();

            var executedPath = client.ExecutePath(envId, vmId, @"c:\installer\Jetstream.bat");
            Thread.Sleep(1800000);
            
            var resp = client.TakeSnapshot(envId, "Jestream " + DateTime.Now.ToString("yyyyMMdd"), "Jetstream cloudshare created on " + DateTime.Now.ToShortDateString(), false);
            Console.WriteLine(resp.status_text);
        }

        private static void UploadFile(string fileToUpload)
        {
            SessionOptions sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = CloudshareManager.cloudFoldersInfo.data.host,
                UserName = CloudshareManager.cloudFoldersInfo.data.user,
                Password = CloudshareManager.cloudFoldersInfo.data.password
            };
            using (Session session = new Session())
            {
                // Connect
                session.DisableVersionCheck = true;
                session.Open(sessionOptions);

                // Upload files
                TransferOptions transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;

                var a = session.ListDirectory("/");
                CloudshareManager.remoteUserFolderName = a.Files[2].Name;

                TransferOperationResult transferResult;
                transferResult = session.PutFiles(fileToUpload, "/" + a.Files[2].Name + "/*.*", false, transferOptions);

                // Throw on any error
                transferResult.Check();
            }
        }
    }
}
