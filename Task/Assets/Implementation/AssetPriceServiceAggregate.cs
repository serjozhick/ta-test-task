using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TATask.AssetApi;
using TATask.Assets.Interface;
using TATask.Contracts;

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
            foreach (var assetDto in assetDtos)
            {
                assetsPage.Add(Mapper.Map<Asset>(assetDto));
                if (assetsPage.Count >= pageSize)
                {
                    yield return await FillPrices(assetsPage);
                    assetsPage = new List<Asset>();
                }
            }
            if (assetsPage.Count > 0)
            {
                yield return await FillPrices(assetsPage);
            }
        }

        private async Task<Asset[]> FillPrices(List<Asset> assetPage)
        {
            var marketsPage = await PriceService.GetPrices(
                assetPage.Where(a => a.AssetSymbol != null).Select(a => a.AssetSymbol).ToArray());
            var assetPricesPage = marketsPage.GroupBy(m => m.BaseSymbol, m => Mapper.Map<AssetPrice>(m))
                .ToDictionary(g => g.Key, g => g.ToArray());
            foreach (var asset in assetPage)
            {
                if (assetPricesPage.TryGetValue(asset.AssetSymbol, out var prices))
                {
                    asset.Prices = prices;
                }
            }

            return assetPage.ToArray();
        }
    }
}