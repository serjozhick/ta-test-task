using System.Threading.Tasks;

namespace TATask.Contracts
{
    public interface IRemoteFile
    {
        Task<string> GetHash(string url);
    }
}