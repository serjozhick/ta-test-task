using System.Collections.Generic;
using TATask.Contracts;

namespace TATask.Assets.Interface
{
    public interface IPricedAssetService
    {
        IAsyncEnumerable<Asset[]> GetAssetPages(int limit, int pageSize);
    }
}