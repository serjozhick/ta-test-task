using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;
using TATask.Contracts;

namespace TATask.GraphQl
{
    public class PricesQuerier
    {
        private const string ASSETS_ENDPOINT = "https://api.blocktap.io/graphql";

        private class AssetResponse
        {
            [JsonProperty(PropertyName = "assets")]
            public Asset[] Assets { get; set; }
        }

        public async Task<Asset[]> GetAssets(int limit)
        {
            using var graphQLClient = new GraphQLHttpClient(ASSETS_ENDPOINT, new NewtonsoftJsonSerializer());

            var personAndFilmsRequest = new GraphQLRequest
            {
                Query = @"
                    query PageAssets($limit: Int) {
                        assets(sort: [{marketCapRank: ASC}], page: { skip: 0, limit: $limit }) {
                            assetName
                            assetSymbol
                            marketCap
                        }
                    }",
                OperationName = "PageAssets",
                Variables = new
                {
                    limit
                }
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<AssetResponse>(personAndFilmsRequest);
            return graphQLResponse.Data.Assets;
        }

        public class Ticker
        {
            [JsonProperty(PropertyName = "lastPrice")]
            public decimal Price { get; set; }
        }
        public class Market
        {
            [JsonProperty(PropertyName = "baseSymbol")]
            public string BaseSymbol { get; set; }
            [JsonProperty(PropertyName = "marketSymbol")]
            public string Symbol { get; set; }
            [JsonProperty(PropertyName = "ticker")]
            public Ticker Ticker { get; set; }
        }
        private class PricesResponse
        {
            [JsonProperty(PropertyName = "markets")]
            public Market[] Markets { get; set; }
        }
        
        public async Task<Market[]> GetPrices(string[] assetSymbols)
        {
            using var graphQLClient = new GraphQLHttpClient(ASSETS_ENDPOINT, new NewtonsoftJsonSerializer());

            var personAndFilmsRequest = new GraphQLRequest
            {
                Query = @"
                    query price($symbols: [String]) {
                        markets(filter: { baseSymbol: {_in: $symbols}, quoteSymbol: {_eq: ""EUR""} }) {
                            baseSymbol
                            marketSymbol
                            ticker {
                                lastPrice
                            }
                        }
                    }",
                OperationName = "price",
                Variables = new
                {
                    symbols = assetSymbols
                }
            };

            var graphQLResponse = await graphQLClient.SendQueryAsync<PricesResponse>(personAndFilmsRequest);
            return graphQLResponse.Data.Markets;
        }
    }
}