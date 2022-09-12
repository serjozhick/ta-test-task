using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TATask.Contracts;

namespace TATask.Threading
{
    public class CommunicationThreadTask : IThreadTask
    {
        public async Task<TimeSpan> Execute(int itemsCount, int threadsCount)
        {
            var timer = new Stopwatch();
            timer.Start();
            var queue = new ConcurrentQueue<SomeData>();
            var cancellation = new CancellationTokenSource();
            var threads = Enumerable.Range(0, threadsCount).Select(_ => Consumer(queue, cancellation.Token)).ToArray();
            Producer(itemsCount, queue, cancellation);
            timer.Stop();
            await Task.WhenAll(threads);
            return timer.Elapsed;
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
                    await Task.Delay(100, cancel).ContinueWith(_ => { });
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