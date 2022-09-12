using System;
using System.Threading;
using System.Threading.Tasks;
using TATask.Threading;
using Xunit;

namespace TATaskTest
{
    public class MultithreadingTest
    {
        private class AwaitableThreadTestableTask : AwaitableThreadTask
        {
            private int executionsCount = 0;
            private int threadsCount = 0;
            public int ExecutionsCount => executionsCount;
            public int ParallelThreadsCount { get; private set; } = 0;

            protected override async Task<bool> FuncB(SomeData data)
            {
                Interlocked.Increment(ref threadsCount);
                ParallelThreadsCount = Math.Max(ParallelThreadsCount, threadsCount);
                var result = await base.FuncB(data);
                Interlocked.Increment(ref executionsCount);
                Interlocked.Decrement(ref threadsCount);
                return result;
            }
        }

        [Fact]
        public async Task AwaitableTask_SingleThreaded_CompletedAndWaitedAllB()
        {
            var parallelExecutor = new AwaitableThreadTestableTask();

            var task = parallelExecutor.Execute(20, 1);
            Assert.True(parallelExecutor.ExecutionsCount < 20);

            await task;
            Assert.Equal(20, parallelExecutor.ExecutionsCount);
            Assert.Equal(1, parallelExecutor.ParallelThreadsCount);
        }
  
        [Fact]
        public async Task AwaitableTask_MultiThreaded_CompletedAndWaitedAllB()
        {
            var parallelExecutor = new AwaitableThreadTestableTask();

            var task = parallelExecutor.Execute(100, 20);
            Assert.True(parallelExecutor.ExecutionsCount < 100);

            await task;
            Assert.Equal(100, parallelExecutor.ExecutionsCount);
            Assert.True(parallelExecutor.ParallelThreadsCount <= 20);
        }
        
        [Fact]
        public async Task AwaitableTask_MoreThreadsThanItems_CompletedAndWaitedAllB()
        {
            var parallelExecutor = new AwaitableThreadTestableTask();
            var task = parallelExecutor.Execute(20, 30);

            await task;
            Assert.Equal(20, parallelExecutor.ExecutionsCount);
            Assert.True(parallelExecutor.ParallelThreadsCount <= 30);
        }
        
        private class CommunicationThreadTestableTask : AwaitableThreadTask
        {
            private int executionsCount = 0;
            private int threadsCount = 0;
            public int ExecutionsCount => executionsCount;
            public int ParallelThreadsCount { get; private set; } = 0;

            protected override async Task<bool> FuncB(SomeData data)
            {
                Interlocked.Increment(ref threadsCount);
                ParallelThreadsCount = Math.Max(ParallelThreadsCount, threadsCount);
                var result = await base.FuncB(data);
                Interlocked.Increment(ref executionsCount);
                Interlocked.Decrement(ref threadsCount);
                return result;
            }
        }

        [Fact]
        public async Task CommunicationTask_SingleThreaded_CompletedAndWaitedAllB()
        {
            var parallelExecutor = new CommunicationThreadTestableTask();

            var task = parallelExecutor.Execute(20, 1);
            Assert.True(parallelExecutor.ExecutionsCount < 20);

            await task;
            Assert.Equal(20, parallelExecutor.ExecutionsCount);
            Assert.Equal(1, parallelExecutor.ParallelThreadsCount);
        }
  
        [Fact]
        public async Task CommunicationTask_MultiThreaded_CompletedAndWaitedAllB()
        {
            var parallelExecutor = new CommunicationThreadTestableTask();

            var task = parallelExecutor.Execute(100, 20);
            Assert.True(parallelExecutor.ExecutionsCount < 100);

            await task;
            Assert.Equal(100, parallelExecutor.ExecutionsCount);
            Assert.True(parallelExecutor.ParallelThreadsCount <= 20);
        }
        
        [Fact]
        public async Task CommunicationTask_MoreThreadsThanItems_CompletedAndWaitedAllB()
        {
            var parallelExecutor = new CommunicationThreadTestableTask();
            var task = parallelExecutor.Execute(20, 30);

            await task;
            Assert.Equal(20, parallelExecutor.ExecutionsCount);
            Assert.True(parallelExecutor.ParallelThreadsCount <= 30);
        }
    }
}