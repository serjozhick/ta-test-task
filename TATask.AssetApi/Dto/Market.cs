using Newtonsoft.Json;

namespace TATask.AssetApi.Dto
{
    public class Market
    {
        [JsonProperty(PropertyName = "baseSymbol")]
        public string BaseSymbol { get; set; }
        [JsonProperty(PropertyName = "marketSymbol")]
        public string Symbol { get; set; }
        [JsonProperty(PropertyName = "ticker")]
        public Ticker Ticker { get; set; }
    }
}