using Newtonsoft.Json;
using TATask.AssetApi.Dto;

namespace TATask.AssetApi.Blocktap
{
    public class AssetResponse
    {
        [JsonProperty(PropertyName = "assets")]
        public Asset[] Assets { get; set; }
    }
}