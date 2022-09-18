using System.Threading.Tasks;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.Extensions.Options;

namespace TATask.AssetApi.Blocktap
{
    public abstract class BaseGraphService
    {
        private string Endpoint { get; }

        protected BaseGraphService(IOptions<ApiSettings> options)
        {
            Endpoint = options.Value.Endpoint;
        }
        protected async Task<TResponse> SendQueryAsync<TResponse>(GraphQLRequest request)
        {
            using var graphQlClient = new GraphQLHttpClient(Endpoint, new NewtonsoftJsonSerializer());
            var response = await graphQlClient.SendQueryAsync<TResponse>(request);
            return response.Data;
        }
    }
}