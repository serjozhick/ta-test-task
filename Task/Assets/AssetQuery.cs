using System.Collections.Generic;
using Microsoft.Extensions.Options;
using TATask.Assets.Interface;
using TATask.Configuration;
using TATask.Contracts;

namespace TATask.Assets
{
    public class AssetQuery : IAssetQuery
    {
        private IPricedAssetService AssetService { get; }
        private int PageSize { get; }
        
        public AssetQuery(IPricedAssetService assetService, IOptions<PageQuerySettings> options)
        {
            AssetService = assetService;
            PageSize = options.Value.PageSize;
        }
        public async IAsyncEnumerable<Asset> Execute(int limit)
        {
            var assetPages = AssetService.GetAssetPages(limit, PageSize);
            await foreach (var assetPage in assetPages)
            {
                foreach (var asset in assetPage)
                {
                    yield return asset;
                }
            }
        }
    }
}