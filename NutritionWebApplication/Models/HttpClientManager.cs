using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nutrition.Utilities
{
    public class HttpClientManager : HttpClient
    {

        /// <summary>
        /// Class Constructor
        /// </summary>
        public HttpClientManager() : base()
        {
            DefaultRequestHeaders.Clear();
        }

        /// <summary>
        /// Posts a message asynchronously using HttpClient
        /// </summary>
        /// <param name="address">Address (i.e. URL)</param>
        /// <param name="messageContent">JSON Serialized Message Content</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostMessage(string address, string messageContent)
        {
            return await PostAsync(new Uri(address), new StringContent(messageContent, Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> Get(string address)
        {
            return await GetAsync(new Uri(address));
        }
    }
}
