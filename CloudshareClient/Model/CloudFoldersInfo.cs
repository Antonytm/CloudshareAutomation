
namespace CloudshareAPIWrapper.Model
{
    public class CloudFoldersInfo : BaseResponse
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public string host { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string uri { get; set; }
        public string total_quota_gb { get; set; }
        public string quota_in_use_gb { get; set; }
    }
}
