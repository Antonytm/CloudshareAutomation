
namespace CloudshareAPIWrapper.Model
{
  public class ExecutedPath:BaseResponse
  {
    public ExecutedPathData data { get; set; }    
  }
  public class ExecutedPathData
  {
    public string executed_path { get; set; }
  }
}
