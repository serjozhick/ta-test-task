using Newtonsoft.Json;
using TATask.AssetApi.Dto;

namespace TATask.AssetApi.Blocktap
{
    public class PricesResponse
    {
        [JsonProperty(PropertyName = "markets")]
        public Market[] Markets { get; set; }
    }
}