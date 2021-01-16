using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace WebApiAutomation.Service.Repositories
{
    public class JsonData
    {
        public static List<Json> Fun(JObject obj)
        {
            List<Json> JsonDataList = new List<Json>();
            Json jsonData;
            foreach (var item in obj)
            {
                jsonData = new Json();
                if (item.Value is JObject jObject)
                {
                    var tmp = Fun(jObject);
                    JsonDataList.AddRange(tmp);
                }
                else if (item.Value is JArray jArray)
                {
                    foreach (var jItem in jArray)
                    {
                        if(jItem is JObject jChild)
                        {
                            var tmp = Fun(jChild);
                            JsonDataList.AddRange(tmp);
                        }
                        else
                        {
                            jsonData.Key = item.Key;
                            jsonData.Value = jItem.ToString();
                            JsonDataList.Add(jsonData);
                        }
                    }
                }
                else
                {
                    if (!(item.Value != null && item.Value.ToString().Trim().Equals("")))
                    {
                        jsonData.Key = item.Key;
                        jsonData.Value = item.Value.ToString();
                        JsonDataList.Add(jsonData);
                    }
                }
            }
            return JsonDataList;
        }
    }
    public class Json
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}