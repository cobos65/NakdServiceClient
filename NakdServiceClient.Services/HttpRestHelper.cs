using NakdServiceClient.Business.Contracts.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace NakdServiceClient.Services
{
    public class HttpRestHelper : IHttpRestHelper
    {
        private IRestClient _restClient;

        public Uri BaseUrl
        {
            get { return _restClient.BaseUrl; }
            set { _restClient.BaseUrl = value; }
        }

        public HttpRestHelper(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<K> DoPostAsync<T, K>(string resource, T body, IList<Tuple<string, string>> urlSegments = null)
        {
            if (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(_restClient.BaseUrl?.AbsoluteUri))
            {
                throw new ApplicationException("RestClient not configuredd, please provide client baseUrl and resource to perform the request");
            }
            var request = new RestRequest(resource, Method.POST);

            if (urlSegments != null)
            {
                foreach (var param in urlSegments)
                {
                    request.AddUrlSegment(param.Item1, param.Item2);
                }
            }

            request.AddJsonBody(JsonSerializer.Serialize(body));
            var response = await _restClient.ExecuteAsync<T>(request);
            if (!response.IsSuccessful)
            {
                throw new ApplicationException(response.Content, response.ErrorException);
            }

            return JsonSerializer.Deserialize<K>(response.Content);
        }

        public async Task<bool> DoPostAsync<T>(string resource, T body, IList<Tuple<string, string>> urlSegments = null)
        {
            if (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(_restClient.BaseUrl?.AbsoluteUri))
            {
                throw new ApplicationException("RestClient not configuredd, please provide client baseUrl and resource to perform the request");
            }
            var request = new RestRequest(resource, Method.POST);

            if (urlSegments != null)
            {
                foreach (var param in urlSegments)
                {
                    request.AddUrlSegment(param.Item1, param.Item2);
                }
            }

            request.AddJsonBody(JsonSerializer.Serialize(body));
            var response = await _restClient.ExecuteAsync<T>(request);
            if (!response.IsSuccessful)
            {
                throw new ApplicationException(response.Content, response.ErrorException);
            }

            return true;
        }

        public async Task<bool> DoDeleteAsync<T>(string resource, IList<Tuple<string, string>> urlSegments)
        {
            if (string.IsNullOrEmpty(resource) || string.IsNullOrEmpty(_restClient.BaseUrl?.AbsoluteUri))
            {
                throw new ApplicationException("RestClient not configuredd, please provide client baseUrl and resource to perform the request");
            }
            var request = new RestRequest(resource, Method.DELETE);

            foreach (var param in urlSegments)
            {
                request.AddUrlSegment(param.Item1, param.Item2);
            }

            var response = await _restClient.ExecuteAsync<T>(request);
            if (!response.IsSuccessful)
            {
                throw new ApplicationException(response.Content, response.ErrorException);
            }
            return true;

        }
    }
}
