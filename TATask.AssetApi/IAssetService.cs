using System.Collections.Generic;
using System.Threading.Tasks;
using TATask.AssetApi.Dto;

namespace TATask.AssetApi
{
    public interface IAssetService
    {
        Task<IEnumerable<Asset>> GetAssets(int limit);
    }
}