using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NakdServiceClient.Business.Contracts.Services
{
    public interface IHttpRestHelper
    {
        Task<K> DoPostAsync<T, K>(string resource, T body, IList<Tuple<string, string>> requestParams = null);

        Task<bool> DoPostAsync<T>(string resource, T body, IList<Tuple<string, string>> urlSegments = null);

        Task<bool> DoDeleteAsync<T>(string resource, IList<Tuple<string, string>> urlSegments);
    }
}
