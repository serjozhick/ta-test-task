using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TATask.Contracts;

namespace TATask.File
{
    public class RemoteFile : IRemoteFile
    {
        public async Task<string> GetHash(string url)
        {
            using SHA256 sha256Algorithm = SHA256.Create();
            using HttpClient client = new HttpClient();
            using HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            await using Stream remoteFileStream = await response.Content.ReadAsStreamAsync();
            // buffering does not provide any performance benefits
            // await using Stream remoteFileStream = new BufferedStream(await response.Content.ReadAsStreamAsync(), 4096 * 1024) ;
            
            byte[] hashValue = await sha256Algorithm.ComputeHashAsync(remoteFileStream);
            return BitConverter.ToString(hashValue).Replace("-","");
        }
    }
}