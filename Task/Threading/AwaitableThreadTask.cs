using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TATask.Contracts;

namespace TATask.Threading
{
    public class AwaitableThreadTask : IThreadTask
    {
        public async Task Execute(int itemsCount, int threadsCount)
        {
            var threadLimit = new SemaphoreSlim(threadsCount);
            var threads = await FuncA(itemsCount, threadLimit);
            await Task.WhenAll(threads);
        }

        private async Task<Task[]> FuncA(int itemsCount, SemaphoreSlim threadLimit) {
            var funcBCalls = new List<Task>();
            for (int i = 0; i < itemsCount; i++)
            {
                var call = ThreadLimitedFuncB(new SomeData{ Value = i + 1 }, threadLimit);
                funcBCalls.Add(call);
            }
            return funcBCalls.ToArray();
        }

        private async Task<bool> ThreadLimitedFuncB(SomeData data, SemaphoreSlim threadLimit)
        {
            await threadLimit.WaitAsync();
            try
            {
                return await FuncB(data);
            }
            finally
            {
                threadLimit.Release();
            }
        }

        protected virtual async Task<bool> FuncB(SomeData data)
        {
            await Task.Delay(100);
            return true;
        }
    }
}