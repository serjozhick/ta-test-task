using System.Linq;
using TATask.Contracts;
using TATask.GraphQl;
using Xunit;

namespace TATaskTest
{
    public class AssetQueryTest
    {
        [Fact]
        public void SplitTest_FullRanges()
        {
            var query = new AssetQuery();
            var assets = Enumerable.Range(0, 100).Select(i => new Asset { AssetName = i.ToString()}).ToArray();

            var pages = query.Split(assets, 20).ToArray();
            Assert.Equal(5, pages.Length);
            Assert.True(pages.All(page => 20 == page.Count()));
            Assert.Contains(pages[1], a => a.AssetName == "25");
        }

        [Fact]
        public void SplitTest_LastSmaller()
        {
            var query = new AssetQuery();
            var assets = Enumerable.Range(0, 30).Select(i => new Asset { AssetName = i.ToString()}).ToArray();

            var pages = query.Split(assets, 20).ToArray();
            Assert.Equal(2, pages.Length);
            Assert.Equal(20, pages[0].Count());
            Assert.Equal(10, pages[1].Count());
            Assert.Contains(pages[1], a => a.AssetName == "25");
        }

        [Fact]
        public void SplitTest_SingleItem()
        {
            var query = new AssetQuery();
            var assets = Enumerable.Range(0, 15).Select(i => new Asset { AssetName = i.ToString()}).ToArray();

            var pages = query.Split(assets, 20).ToArray();
            Assert.Single(pages);
            Assert.Equal(15, pages[0].Count());
        }

        [Fact]
        public void MergeTest()
        {
            var query = new AssetQuery();
            var assets = new[] {
                new Asset {
                    AssetName = "Name 1",
                    AssetSymbol = "S1"
                },
                new Asset
                {
                    AssetName = "Name 2",
                    AssetSymbol = "S2"
                },
                new Asset
                {
                    AssetName = "Name 4",
                    AssetSymbol = "S4"
                }
            };
            var markets = new[]
            {
                new PricesQuerier.Market
                {
                    BaseSymbol = "S1",
                    Symbol = "S1",
                    Ticker = new PricesQuerier.Ticker
                    {
                        Price = 1
                    }
                },
                new PricesQuerier.Market
                {
                    BaseSymbol = "S4",
                    Symbol = "S4",
                    Ticker = new PricesQuerier.Ticker
                    {
                        Price = 4
                    }
                },
                new PricesQuerier.Market
                {
                    BaseSymbol = "S3",
                    Symbol = "S3",
                    Ticker = new PricesQuerier.Ticker
                    {
                        Price = 3
                    }
                },
                new PricesQuerier.Market
                {
                    BaseSymbol = "S4",
                    Symbol = "S42",
                    Ticker = new PricesQuerier.Ticker
                    {
                        Price = 42
                    }
                }
            };

            query.Merge(assets, markets);
            
            Assert.Equal(3, assets.Length);
            Assert.Single(assets[0].Prices);
            Assert.Null(assets[1].Prices);
            Assert.Equal(2, assets[2].Prices.Length);
            Assert.Contains(assets[2].Prices, p => p.Market == "S42" && p.Price == 42);
        }
    }
}