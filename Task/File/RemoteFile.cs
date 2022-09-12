using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TATask.Contracts;

namespace TATask.File
{
    public class RemoteFile : IRemoteFile
    {
        public async Task<string> GetHash(string url)
        {
            using HttpClient client = new();
            var digest = await TryGetDigest(client, MakeHeadRequest(url));
            if (string.IsNullOrEmpty(digest))
            {
                digest = await TryGetDigest(client, MakeGetRequestWithoutBody(url));
            }

            if (!string.IsNullOrEmpty(digest))
            {
                return digest.Trim('"').Replace("-", "").ToUpper();
            }

            return digest;
        }

        private async Task<string> TryGetDigest(HttpClient client, HttpRequestMessage message)
        {
            try
            {
                var response = await client.SendAsync(message);
                response.EnsureSuccessStatusCode();
                var checkSumHeaders = response.Headers.GetValues("ETag");
                return checkSumHeaders.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        private HttpRequestMessage MakeHeadRequest(string url) => new (HttpMethod.Head, new Uri(url));

        private HttpRequestMessage MakeGetRequestWithoutBody(string url)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };
            request.Headers.Range = new RangeHeaderValue(0, 1);
            return request;
        }
    }
}