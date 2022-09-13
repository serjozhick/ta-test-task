using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TATask.Contracts;

namespace TATask.GraphQl
{
    public class AssetQuery : IAssetQuery
    {
        private const int assetPageSize = 20;
        private PricesQuerier querier = new();
        public async Task<Asset[]> Execute(int limit)
        {
            var assets = await querier.GetAssets(limit);
            foreach (var assetsPage in Split(assets, assetPageSize))
            {
                var markets = await querier.GetPrices(
                    assets.Where(a => a.AssetSymbol != null).Select(a => a.AssetSymbol).ToArray());
                Merge(assets, markets);
            }

            return assets;
        }

        public IEnumerable<IEnumerable<Asset>> Split(Asset[] arr, int size)
        {
            return arr.Select((s, i) => arr.Skip(i * size).Take(size)).Where(a => a.Any());
        }

        public void Merge(Asset[] assets, PricesQuerier.Market[] markets)
        {
            var assetPrices = markets.GroupBy(m => m.BaseSymbol, m => new AssetPrice
            {
                Market = m.Symbol,
                Price = m.Ticker?.Price
            });
            foreach (var assetPrice in assetPrices)
            {
                var asset = assets.FirstOrDefault(a => a.AssetSymbol == assetPrice.Key);
                if (asset != null)
                {
                    asset.Prices = assetPrice.ToArray();
                }
            }
        } 
    }
}