using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var assetPagesEnumerator = assetPages.GetAsyncEnumerator();
            try
            {
                var nextTask = assetPagesEnumerator.MoveNextAsync();
                while (await nextTask)
                {
                    var currentPage = assetPagesEnumerator.Current.ToArray();
                    nextTask = assetPagesEnumerator.MoveNextAsync();
                    foreach (var asset in currentPage)
                    {
                        yield return asset;
                    }
                }
            }
            finally
            {
                await assetPagesEnumerator.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}