using System.Collections.Generic;

namespace CloudshareAPIWrapper.Model
{
  public class EnvironmentDetails : BaseResponse
  {
    public EnvData data { get; set; }
  }

  public class Vm
  {
    public string vmId { get; set; }
    public string vmToken { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int status_code { get; set; }
    public string status_text { get; set; }
    public string IP { get; set; }
    public string FQDN { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public int progress { get; set; }
    public string webAccessUrl { get; set; }
    public string os { get; set; }
    public string url { get; set; }
    public string image_url { get; set; }
  }

  public class AvailableActions
  {
    public bool resume_environment { get; set; }
    public bool revert_environment { get; set; }
    public bool revert_vm { get; set; }
    public bool delete_vm { get; set; }
    public bool reboot_vm { get; set; }
    public bool add_vms { get; set; }
    public bool take_snapshot { get; set; }
  }

  public class Resources
  {
    public int total_memory_in_use_mb { get; set; }
    public int disk_size_in_use_mb { get; set; }
    public int cpu_in_use { get; set; }
    public int total_memory_qouta_mb { get; set; }
    public int disk_size_qouta_mb { get; set; }
    public int cpu_qouta { get; set; }
  }

  public class EnvData
  {
    public string envId { get; set; }
    public string envToken { get; set; }
    public string view_url { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public int status_code { get; set; }
    public string status_text { get; set; }
    public string licenseValid { get; set; }
    public List<Vm> vms { get; set; }
    public string owner { get; set; }
    public string blueprint { get; set; }
    public string expirationTime { get; set; }
    public string project { get; set; }
    public string organization { get; set; }
    public string snapshot { get; set; }
    public string environmentPolicy { get; set; }
    public bool invitationAllowed { get; set; }
    public AvailableActions available_actions { get; set; }
    public Resources resources { get; set; }
  }
}
