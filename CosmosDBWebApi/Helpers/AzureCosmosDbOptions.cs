using System;

namespace CosmosDBWebApi.Helpers
{
    public class AzureCosmosDbOptions
    {
        public string DatabaseId { get; set; }
        public string Endpoint { get; set; }
        public Uri EndpointAsUri => new Uri(this.Endpoint);
        public string Key { get; set; }
    }
}
