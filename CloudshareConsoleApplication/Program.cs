using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudshareClient;

namespace CloudshareConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = args[0];
            string blueprintName = args[1];
            string snapshotName = args[2];
            CloudshareManager.InstallZipOnCloudshare(filename, blueprintName, snapshotName);
        }
    }
}
