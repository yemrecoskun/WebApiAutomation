using WebApiAutomation.Cache.CacheManager;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApiAutomation.Service.Repositories
{
    public class Request
    {
        public static async Task<string> GetRequest(string key, Guid guid, HttpClient client, string url)
        {
            HttpResponseMessage response = client.GetAsync(url).Result;

            var result = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject(result).ToString();
            GetRequestResponseCacheInsert(key,guid, "", result);


            if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new System.Exception("NotFound");
            return result;
        }
        public static async Task<string> PostRequest(string key, Guid guid, HttpClient client, string url, string request)
        {
            var requestJson = JsonConvert.DeserializeObject(request);
            StringContent content = new StringContent(JsonConvert.SerializeObject(requestJson), Encoding.UTF8, "application/json");

            var response = client.PostAsync(url, content).Result;
            var result = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject(result).ToString();
            GetRequestResponseCacheInsert(key, guid, "", result);


            if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new System.Exception("NotFound");
            return result;
        }
        public static void GetRequestResponseCacheInsert(string key, Guid guid,string request, string response)
        {
            CacheManager.Insert(key, "request", guid, request);
            CacheManager.Insert(key, "response", guid, response);
        }
    }
}