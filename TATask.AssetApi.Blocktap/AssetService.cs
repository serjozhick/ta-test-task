using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using Microsoft.Extensions.Options;
using TATask.AssetApi.Dto;

namespace TATask.AssetApi.Blocktap
{
    public class AssetService : BaseGraphService, IAssetService
    {
        public AssetService(IOptions<ApiSettings> options) : base(options)
        {
        }

        public async Task<IEnumerable<Asset>> GetAssets(int limit)
        {
            var assetsRequest = new GraphQLRequest
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

            var assetResponse = await SendQueryAsync<AssetResponse>(assetsRequest);
            return assetResponse.Assets;
        }
    }
}