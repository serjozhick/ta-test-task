using Newtonsoft.Json;

namespace TATask.AssetApi.Dto
{
    public class Asset
    {
        [JsonProperty(PropertyName = "assetName")]
        public string AssetName { get; set; }
        [JsonProperty(PropertyName = "assetSymbol")]
        public string AssetSymbol { get; set; }
        [JsonProperty(PropertyName = "marketCap")]
        public long? MarketCap { get; set; }
    }
}