namespace DotNet.Library.Services.Azure.Options
{
    public class DevOpsServiceOptions
    {
        /// <summary>
        /// A unique identifer used to identify options when multiple DevOps services are consumed by an application
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Azure DevOps organization name to use for this service connection (e.g. "MyOrg" in https://dev.azure.com/MyOrg/MyProject or https://MyOrg.visualstudio.com/MyProject)
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Either an actual PAT or a URL to a Key Vault Secret which contains a PAT
        /// </summary>
        public string PersonalAccessToken { get; set; }
    }
}
