using System;
using System.Threading.Tasks;

namespace TATask.Contracts
{
    public interface IThreadTask
    {
        Task<TimeSpan> Execute(int itemsCount, int threadsCount);
    }
}