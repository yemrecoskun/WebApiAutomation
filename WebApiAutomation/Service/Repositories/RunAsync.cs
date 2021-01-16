using WebApiAutomation.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApiAutomation.Service.Repositories
{
    public class RunAsync
    {
        public static HttpClient client { get; set; }
        public static FastpayToolsEntities fastpayTools { get; set; }
        public static async Task<string> Run(string key, Guid guid, string url, string request, List<Json> header, int requestType, string endpoint)
        {
            client = new HttpClient();
            fastpayTools = new FastpayToolsEntities();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            
            //client headers add($ parameters)
            HeaderAdd(header);

            if ((RequestType)requestType == RequestType.Post)
            {
                var result = await Request.PostRequest(key, guid, client, endpoint, request);
                return result;
            }
            else
            {
                var result = await Request.GetRequest(key, guid, client, endpoint);
                return result;
            }
        }

        public static void HeaderAdd(List<Json> header)
        {
            foreach (var item in header)
            {
                if (item.Value[0] == '$')
                {
                    var itemValue = item.Value.Substring(1);
                    List<string> itemList = new List<string>();
                    string value = "";
                    foreach (var item1 in itemValue)
                    {
                        if (item1 == '$')
                        {
                            itemList.Add(value);
                            value = "";
                        }
                        else
                        {
                            value = value + item1;
                        }
                    }
                    string action = itemList[0];
                    var endpointLinq = fastpayTools.EndpointTable.FirstOrDefault(i => i.Action == action);
                    string jsonString = "";
                    try
                    {
                        switch (itemList[1])
                        {
                            case "Request": jsonString = endpointLinq.Request; break;
                            case "Response": jsonString = endpointLinq.Response; break;
                            case "Header": jsonString = endpointLinq.Header; break;
                        }
                    }
                    catch
                    {
                        throw new Exception("NotFound");
                    }
                    var json = JsonConvert.DeserializeObject<dynamic>(jsonString);
                    string Key = item.Key;
                    string Value = json[itemList[2]];
                    client.DefaultRequestHeaders.Add(Key, Value);
                }
                else if (item.Value == "application/json")
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }
                else
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
        }
    }
}