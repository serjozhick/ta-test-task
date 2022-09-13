using System.Threading.Tasks;

namespace TATask.Contracts
{
    public interface IAssetQuery
    {
        Task<Asset[]> Execute(int limit);
    }
}