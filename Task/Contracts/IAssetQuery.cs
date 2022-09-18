using System.Collections.Generic;

namespace TATask.Contracts
{
    public interface IAssetQuery
    {
        IAsyncEnumerable<Asset> Execute(int limit);
    }
}