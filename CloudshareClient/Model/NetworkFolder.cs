
namespace CloudshareAPIWrapper.Model
{
  public class NetworkFolder:BaseResponse
  {
    public NetworkFolderData data { get; set; }    
  }
  public class NetworkFolderData
  {
    public string apiId { get; set; }
  }
}
