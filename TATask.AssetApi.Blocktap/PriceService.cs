using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.Extensions.Options;
using TATask.AssetApi.Dto;

namespace TATask.AssetApi.Blocktap
{
    public class PriceService : BaseGraphService, IPriceService
    {
        public PriceService(IOptions<ApiSettings> options) : base(options)
        {
        }

        public async Task<IEnumerable<Market>> GetPrices(string[] assetSymbols)
        {
            var pricesRequest = new GraphQLRequest
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

            var pricesResponse = await SendQueryAsync<PricesResponse>(pricesRequest);
            return pricesResponse.Markets;
        }
    }
}