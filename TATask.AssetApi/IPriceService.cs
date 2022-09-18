using System.Collections.Generic;
using System.Threading.Tasks;
using TATask.AssetApi.Dto;

namespace TATask.AssetApi
{
    public interface IPriceService
    {
        Task<IEnumerable<Market>> GetPrices(string[] assetSymbols);
    }
}