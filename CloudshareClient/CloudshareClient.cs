namespace CloudshareAPIWrapper
{
  using System;
  using System.Globalization;
  using System.Linq;
  using System.Security.Cryptography;
  using CloudshareAPIWrapper.Model;
  using RestSharp;

  /// <summary>
  /// The cloudshare client.
  /// </summary>
  public class CloudshareClient
  {
    /// <summary>
    /// The client.
    /// </summary>
    private readonly RestClient client;

    /// <summary>
    /// The api id.
    /// </summary>
    private readonly string apiId;

    /// <summary>
    /// The api key.
    /// </summary>
    private readonly string apiKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="CloudshareClient"/> class. 
    /// Constructs a CloudshareClient.
    /// </summary>
    /// <param name="account">
    /// Cloudshare account information
    /// </param>
    public CloudshareClient(CloudshareAccount account)
    {
      this.client = new RestClient(account.ServerUrl);
      
      // To debug with fiddler use line below
      // this.client.Proxy = new WebProxy("localhost", 8888);
      this.apiId = account.ApiId;
      this.apiKey = account.ApiKey;
    }


    /// <summary>
    /// The get timestamp.
    /// </summary>
    /// <returns>
    /// The <see cref="long"/>.
    /// </returns>
    private long GetTimeStamp()
    {
      var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return Convert.ToInt64((DateTime.Now.ToUniversalTime() - epoch).TotalSeconds);
    }

    /// <summary>
    /// Convert string to byte array.
    /// </summary>
    /// <param name="str">
    /// The str.
    /// </param>
    /// <returns>
    /// The <see cref="byte[]"/>.
    /// </returns>
    private static byte[] StrToByteArray(string str)
    {
      var encoding = new System.Text.UTF8Encoding();
      return encoding.GetBytes(str);
    }

    /// <summary>
    /// Executes a RestRequest and returns the deserialized response. If
    /// the response hasn't got the specified expected response code or if an
    /// exception was thrown during execution a Exception will be 
    /// thrown.
    /// </summary>
    /// <typeparam name="T">
    /// Request return type
    /// </typeparam>
    /// <param name="request">
    /// request to execute
    /// </param>
    /// <param name="expectedResponseCode">
    /// The expected Response Code.
    /// </param>
    /// <returns>
    /// deserialized response of request
    /// </returns>
    public T Execute<T>(RestRequest request) where T : new()
    {
      string ts = this.GetTimeStamp().ToString(CultureInfo.InvariantCulture);
      request.AddParameter("timestamp", ts);

      //TODO: make token parameter be random
      //but it works anyway
      request.AddParameter("token", "JAHL6X5H2I");
      request.AddParameter("userapiid", this.apiId);

      var sortedParameters = request.Parameters.OrderBy(x => x.Name);
      var toHash = this.apiKey + request.Resource.Substring(request.Resource.LastIndexOf("/", StringComparison.Ordinal) + 1).ToLower();
      foreach (var param in sortedParameters)
      {
        toHash += param.Name + param.Value;
      }
      SHA1 sha = new SHA1CryptoServiceProvider();

      // This is one implementation of the abstract class SHA1.
      var dhmac = sha.ComputeHash(StrToByteArray(toHash));
      var hmac = BitConverter.ToString(dhmac).Replace("-", string.Empty).ToLower();
      request.AddParameter("hmac", hmac);

      var response = this.client.Execute<T>(request);

      
      if (response.ResponseStatus != ResponseStatus.Completed || response.ErrorException != null)
        throw new Exception(
              "RestSharp response status: " + response.ResponseStatus + " - HTTP response: " + response.StatusCode + " - " + response.StatusDescription
            + " - " + response.Content);
      
        
      return response.Data;
    }

    /// <summary>
    /// The get all available blueprints.
    /// </summary>
    /// <returns>
    /// The <see cref="Blueprints"/>.
    /// </returns>
    public Blueprints GetAllAvailableBlueprints()
    {
      var request = new RestRequest("v2/Env/CreateEntAppEnvOptions", Method.GET);
      return this.Execute<Blueprints>(request);
    }

    /// <summary>
    /// The create blueprint from snapshot.
    /// </summary>
    /// <param name="environmentPolicyId">
    /// The environment policy id.
    /// </param>
    /// <param name="snapshotId">
    /// The snapshot id.
    /// </param>
    /// <param name="environmentNewName">
    /// The environment new name.
    /// </param>
    /// <param name="blueprintFilter">
    /// The blueprint filter.
    /// </param>
    /// <param name="environmentPolicyFilter">
    /// The environment policy filter.
    /// </param>
    /// <returns>
    /// The <see cref="BaseResponse"/>.
    /// </returns>
    public BaseResponse CreateBlueprintFromSnapshot(string environmentPolicyId, string snapshotId, string environmentNewName, string blueprintFilter, string environmentPolicyFilter)
    {
      var request = new RestRequest("v2/Env/CreateEntAppEnv", Method.GET);
      request.AddParameter("environmentpolicyid", environmentPolicyId);
      request.AddParameter("snapshotid", snapshotId);
      request.AddParameter("environmentnewname", environmentNewName);
      request.AddParameter("blueprintfilter", blueprintFilter);
      request.AddParameter("projectfilter", "Sitecore");
      request.AddParameter("environmentpolicyfilter", environmentPolicyFilter);

      return this.Execute<BaseResponse>(request); 	
    }

    /// <summary>
    /// The get environments list.
    /// </summary>
    /// <returns>
    /// The <see cref="Environments"/>.
    /// </returns>
    public Environments GetEnvironmentsList()
    {
        var request = new RestRequest("v2/Env/ListEnvironments", Method.GET);
        return this.Execute<Environments>(request);
    }

    /// <summary>
    /// The get cloud folders info.
    /// </summary>
    /// <returns>
    /// The <see cref="CloudFoldersInfo"/>.
    /// </returns>
    public CloudFoldersInfo GetCloudFoldersInfo()
    {
        var request = new RestRequest("v2/Env/GetCloudFoldersInfo", Method.GET);
        return this.Execute<CloudFoldersInfo>(request);
    }

    /// <summary>
    /// The mount.
    /// </summary>
    /// <param name="envId">
    /// The env id.
    /// </param>
    /// <returns>
    /// The <see cref="NetworkFolder"/>.
    /// </returns>
    public NetworkFolder Mount(string envId)
    {
      var request = new RestRequest("v2/Env/Mount", Method.GET);
      request.AddParameter("envid", envId);
      return this.Execute<NetworkFolder>(request);
    }

    /// <summary>
    /// The get environment state.
    /// </summary>
    /// <param name="envId">
    /// The env id.
    /// </param>
    /// <returns>
    /// The <see cref="EnvironmentDetails"/>.
    /// </returns>
    public EnvironmentDetails GetEnvironmentState(string envId)
    {
      var request = new RestRequest("v2/Env/GetEnvironmentState", Method.GET);
      request.AddParameter("envid", envId);
      return this.Execute<EnvironmentDetails>(request);
    }

    /// <summary>
    /// The execute path.
    /// </summary>
    /// <param name="envId">
    /// The env id.
    /// </param>
    /// <param name="vmId">
    /// The vm id.
    /// </param>
    /// <param name="path">
    /// The path.
    /// </param>
    /// <returns>
    /// The <see cref="ExecutedPath"/>.
    /// </returns>
    public ExecutedPath ExecutePath(string envId, string vmId, string path)
    {
      var request = new RestRequest("v2/Env/ExecutePath", Method.GET);
      request.AddParameter("envid", envId);
      request.AddParameter("vmid", vmId);
      request.AddParameter("path", path);
      return this.Execute<ExecutedPath>(request);
    }

    public BaseResponse TakeSnapshot(string envId, string snapshotName, string description, bool setAsDefault)
    {
        var request = new RestRequest("v2/Env/EntAppTakeSnapshot", Method.GET);
        request.AddParameter("envid", envId);
        request.AddParameter("snapshotname", snapshotName);
        request.AddParameter("description", description);
        request.AddParameter("setasdefault", setAsDefault);
        return this.Execute<ExecutedPath>(request);       
    }
  }

}
