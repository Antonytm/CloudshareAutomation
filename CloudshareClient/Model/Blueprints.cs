using System.Collections.Generic;
namespace CloudshareAPIWrapper.Model
{
  public class Blueprints : BaseResponse
  {
    public List<Datum> data { get; set; }
  }

  public class Snapshot
  {
    public string SnapshotId { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public string CreationTime { get; set; }
    public string Comment { get; set; }
    public bool IsDefault { get; set; }
    public bool IsLatest { get; set; }
  }

  public class Blueprint
  {
    public string Name { get; set; }
    public List<Snapshot> Snapshots { get; set; }
  }

  public class Datum
  {
    public string EnvironmentPolicyId { get; set; }
    public string Project { get; set; }
    public List<Blueprint> Blueprints { get; set; }
    public string EnvironmentPolicyDuration { get; set; }
    public List<string> Organizations { get; set; }
  }
}
