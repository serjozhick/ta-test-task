using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TATask.Contracts;

namespace TATask.Threading
{
    public class CommunicationThreadTask : IThreadTask
    {
        public async Task Execute(int itemsCount, int threadsCount)
        {
            var queue = new ConcurrentQueue<SomeData>();
            var cancellation = new CancellationTokenSource();
            var threads = Enumerable.Range(0, threadsCount).Select(_ => Consumer(queue, cancellation.Token)).ToArray();
            Producer(itemsCount, queue, cancellation);
            await Task.WhenAll(threads);
        }

        private void Producer(int itemsCount, ConcurrentQueue<SomeData> queue, CancellationTokenSource cancellation) {
            for (int i = 0; i < itemsCount; i++) {
                queue.Enqueue(new SomeData{ Value = i+1 });
            }
            cancellation.Cancel();
        }

        private async Task Consumer(ConcurrentQueue<SomeData> dataQueue, CancellationToken cancel){
            while(!cancel.IsCancellationRequested || dataQueue.Count > 0) {
                if(dataQueue.TryDequeue(out var data)){ 
                    await FuncB(data);
                } else {
                    await Task.Delay(100, cancel);
                }
            }
        }

        protected virtual async Task<bool> FuncB(SomeData data)
        {
            await Task.Delay(100);
            return true;
        }
    }
}