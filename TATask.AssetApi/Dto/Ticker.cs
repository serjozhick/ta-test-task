using Newtonsoft.Json;

namespace TATask.AssetApi.Dto
{
    public class Ticker
    {
        [JsonProperty(PropertyName = "lastPrice")]
        public decimal Price { get; set; }
    }
}