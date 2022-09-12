using System.Threading.Tasks;

namespace TATask.Contracts
{
    public interface IThreadTask
    {
        Task Execute(int itemsCount, int threadsCount);
    }
}