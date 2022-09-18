using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TATask.AssetApi;
using TATask.AssetApi.Dto;
using TATask.Assets.Implementation;
using TATask.Mapping;
using Xunit;

namespace TATaskTest
{
    public class AssetTaskTest
    {
        private class MockAssetService : IAssetService
        {
            private Asset[] data =
            {
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

            public Task<IEnumerable<Asset>> GetAssets(int limit)
            {
                return Task.FromResult(data.Take(limit));
            }
        }

        private class MockPriceService : IPriceService
        {
            private Market[] data = 
            {
                new Market
                {
                    BaseSymbol = "S1",
                    Symbol = "S1",
                    Ticker = new Ticker
                    {
                        Price = 1
                    }
                },
                new Market
                {
                    BaseSymbol = "S4",
                    Symbol = "S4",
                    Ticker = new Ticker
                    {
                        Price = 4
                    }
                },
                new Market
                {
                    BaseSymbol = "S3",
                    Symbol = "S3",
                    Ticker = new Ticker
                    {
                        Price = 3
                    }
                },
                new Market
                {
                    BaseSymbol = "S4",
                    Symbol = "S42",
                    Ticker = new Ticker
                    {
                        Price = 42
                    }
                }
            };

            public Task<IEnumerable<Market>> GetPrices(string[] assetSymbols)
            {
                return Task.FromResult(data.Where(m => assetSymbols.Contains(m.BaseSymbol)));
            }
        }

        private IMapper Mapper { get; }

        public AssetTaskTest()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            Mapper = mockMapper.CreateMapper();
        }
        [Fact]
        public async Task AssetPriceServiceAggregate_GetPages_Test()
        {
            var query = new AssetPriceServiceAggregate(new MockAssetService(), new MockPriceService(), Mapper);

            var pages = await query.GetAssetPages(10, 2).ToListAsync();
            
            Assert.Equal(2, pages.Count);
            Assert.Equal(2, pages[0].Length);
            Assert.Single(pages[0][0].Prices);
            Assert.Null(pages[0][1].Prices);
            Assert.Single(pages[1]);
            Assert.Equal(2, pages[1][0].Prices.Length);
            Assert.Contains(pages[1][0].Prices, p => p.Market == "S42" && p.Price == 42);
        }
    }
}