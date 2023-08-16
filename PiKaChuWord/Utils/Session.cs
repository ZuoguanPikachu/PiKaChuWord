using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiKaChuWord.Utils
{
    public class Session
    {
        private HttpClient client;

        public Session()
        {
            HttpClientHandler handler = new() { UseCookies = true };
            client = new HttpClient(handler);
        }

        public HttpContent Get(string url, Dictionary<string, string> headers)
        {
            client.DefaultRequestHeaders.Clear();
            foreach (KeyValuePair<string, string> item in headers)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            HttpResponseMessage response = client.GetAsync(url).Result;
            HttpContent content = response.Content;

            return content;
        }

        public HttpContent Post(string url, Dictionary<string, string> data, Dictionary<string, string> headers)
        {
            FormUrlEncodedContent data_ = new FormUrlEncodedContent(data);

            client.DefaultRequestHeaders.Clear();
            foreach (KeyValuePair<string, string> item in headers)
            {
                client.DefaultRequestHeaders.Add(item.Key, item.Value);
            }

            HttpResponseMessage response = client.PostAsync(url, data_).Result;
            HttpContent content = response.Content;

            return content;
        }
    }
}
