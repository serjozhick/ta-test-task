using System;
using System.Threading.Tasks;
using TATask.File;
using Xunit;

namespace TATaskTest
{
    public class FileHashTest
    {
        [Fact]
        public async Task SmallText_AlgorithmicInvert_ShouldInvert()
        {
            var remoteFile = new RemoteFile();

            var hash = await remoteFile.GetHash("https://speed.hetzner.de/100MB.bin");

            Assert.Equal("5253F0FD6400000", hash);
        }
    }
}