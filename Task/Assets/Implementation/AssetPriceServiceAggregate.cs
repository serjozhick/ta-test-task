using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TATask.AssetApi;
using TATask.AssetApi.Dto;
using TATask.Assets.Interface;
using TATask.Contracts;
using Asset = TATask.Contracts.Asset;

namespace TATask.Assets.Implementation
{
    public class AssetPriceServiceAggregate : IPricedAssetService
    {
        private IAssetService AssetService { get; }
        private IPriceService PriceService { get; }
        private IMapper Mapper { get; }

        public AssetPriceServiceAggregate(
            IAssetService assetService,
            IPriceService priceService,
            IMapper mapper)
        {
            AssetService = assetService;
            PriceService = priceService;
            Mapper = mapper;
        }

        public async IAsyncEnumerable<Asset[]> GetAssetPages(int limit, int pageSize)
        {
            var assetDtos = await AssetService.GetAssets(limit);

            var assetsPage = new List<Asset>();
            Task<Asset[]> previousTask = null;
            foreach (var assetDto in assetDtos)
            {
                assetsPage.Add(Mapper.Map<Asset>(assetDto));
                if (assetsPage.Count >= pageSize)
                {
                    var fillTask = FillPrices(assetsPage.ToArray());
                    if (previousTask != null)
                    {
                        yield return await previousTask;
                    }

                    previousTask = fillTask;
                    assetsPage = new List<Asset>();
                }
            }
            if (assetsPage.Count > 0)
            {
                var fillTask = FillPrices(assetsPage);
                if (previousTask != null)
                {
                    yield return await previousTask;
                }

                previousTask = fillTask;
            }
            if (previousTask != null)
            {
                yield return await previousTask;
            }
        }

        private async Task<Asset[]> FillPrices(IEnumerable<Asset> assetPage)
        {
            var assets = assetPage as Asset[] ?? assetPage.ToArray();
            var marketsPage = await PriceService.GetPrices(
                assets.Where(a => a.AssetSymbol != null).Select(a => a.AssetSymbol).ToArray());
            var assetPricesPage = marketsPage.GroupBy(m => m.BaseSymbol, m => Mapper.Map<AssetPrice>(m))
                .ToDictionary(g => g.Key, g => g.ToArray());
            foreach (var asset in assets)
            {
                if (assetPricesPage.TryGetValue(asset.AssetSymbol, out var prices))
                {
                    asset.Prices = prices;
                }
            }

            return assets;
        }
    }
}