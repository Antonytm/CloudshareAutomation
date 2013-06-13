using System.Collections.Generic;

namespace CloudshareAPIWrapper.Model
{
    public class Environments:BaseResponse
    {
        public List<EnvironmentsData> data { get; set; }
    }

    public class EnvironmentsData
    {
        public string envId { get; set; }
        public string envToken { get; set; }
        public string view_url { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int status_code { get; set; }
        public string status_text { get; set; }
        public string licenseValid { get; set; }
        public string owner { get; set; }
        public string blueprint { get; set; }
        public string expirationTime { get; set; }
        public string project { get; set; }
        public string organization { get; set; }
        public string snapshot { get; set; }
        public string environmentPolicy { get; set; }
        public bool invitationAllowed { get; set; }
    }
}
