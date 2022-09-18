using System.Threading.Tasks;
using TATask.File;
using Xunit;

namespace TATaskTest
{
    public class FileHashTest
    {
        [Fact]
        public async Task FileHashShouldMatch()
        {
            var remoteFile = new RemoteFile();
            var hash = await remoteFile.GetHash("https://speed.hetzner.de/100MB.bin");
            Assert.Equal("20492A4D0D84F8BEB1767F6616229F85D44C2827B64BDBFB260EE12FA1109E0E", hash);
        }
    }
}