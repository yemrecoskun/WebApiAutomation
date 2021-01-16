using System;
using System.Collections.Generic;
using WebApiAutomation.Cache.CacheManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApiAutomation.Service.Repositories;
using System.Linq;
using WebApiAutomation.Models;

namespace WebApiAutomation.Service
{
    public class EndpointService
    {
        private static FastpayToolsEntities fastpayTools = new FastpayToolsEntities();
        public static List<string> GetRequestClient(string url, List<string> ActionName, List<string> header, List<string> request, string key)
        {
            GetCacheSortKeyClear(key);

            List<string> resultJson = new List<string>();
            for (int i = 0; i < ActionName.Count(); i++)
            {
                JObject headerJObject;
                List<Json> headerJson = new List<Json>();
                try
                {
                    headerJObject = JObject.Parse(header[i]);
                    headerJson = JsonData.Fun(headerJObject);

                    int requestType = fastpayTools.EndpointTable.Where(s => s.Action == ActionName[i]).Select(a => a.RequestType).FirstOrDefault();
                    string endpoint = fastpayTools.EndpointTable.Where(s => s.Action == ActionName[i]).Select(a => a.EndPoint).FirstOrDefault();
                    var guidID = fastpayTools.EndpointTable.FirstOrDefault(a => a.Action == ActionName[i]).Id;

                    var Response = JsonConvert.DeserializeObject(RunAsync.Run(key, guidID, url, request[i], headerJson, requestType, endpoint).GetAwaiter().GetResult());
                    var endpointUpdate = fastpayTools.EndpointTable.FirstOrDefault(s => s.Action == ActionName[i]);
                    endpointUpdate.Response = Response.ToString();
                    fastpayTools.SaveChanges();
                    resultJson.Add("Success");
                }
                catch (Exception ex)
                {
                    if (ex.Message == "NotFound")
                    {
                        resultJson.Add("Error");
                        for (int j = i + 1; j < ActionName.Count(); j++)
                        {
                            var guidID = fastpayTools.EndpointTable.FirstOrDefault(a => a.Action == ActionName[j]).Id;
                            resultJson.Add("Error");
                            Request.GetRequestResponseCacheInsert(key, guidID, "NotFound", "NotFound");
                        }
                        return resultJson;
                    }
                    resultJson.Add("Error");
                }
            }
            return resultJson;
        }
        public static List<EndpointModel> GetSortList(string sortName)
        {

            var endPointId = fastpayTools.Sort.Where(s => s.SortName == sortName).Select(i => i.EndpointID).ToList().Distinct();
            var endPointList = new List<EndpointTable>();
            foreach (var item in endPointId)
            {
                endPointList.Add(fastpayTools.EndpointTable.Where(i => i.Id == item).FirstOrDefault());
            }
            var resultEndPointList = new List<EndpointModel>();
            foreach (var item in endPointList)
            {
                resultEndPointList.Add(new EndpointModel
                {
                    Action = item.Action,
                    CreateDate = item.CreateDate,
                    EndPoint = item.EndPoint,
                    Header = item.Header,
                    Id = item.Id,
                    IsDeleted = item.IsDeleted,
                    ModifiedDate = item.ModifiedDate,
                    Request = item.Request,
                    RequestType = item.RequestType,
                    Response = item.Response,
                    ServiceName = item.ServiceName
                });
            }
            return resultEndPointList.ToList();
        }
        public static string PostSort(List<Guid> Id, string sortName)
        {

            try
            {
                if (sortName == "")
                {
                    throw new Exception("Sort Name is Null");
                }
                var IdArray = Id;
                if (IdArray.Count == 0)
                {
                    throw new Exception("Endpoint is null");
                }
                for (int i = 0; i < IdArray.Count; i++)
                {
                    fastpayTools.Sort.Add(new Sort { RequestSorts = i, EndpointID = IdArray[i], SortName = sortName });
                }
                fastpayTools.SaveChanges();

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static string PostEndpoint(string ServiceName, string EndPoint, string Request, string Header, int RequestType, string Action)
        {
            try
            {
                if (ServiceName == "" || EndPoint == "" || Action == "")
                {
                    throw new Exception("Parameters is null");
                }
                var control = JsonConvert.DeserializeObject(Request);
                control = JsonConvert.DeserializeObject(Header);
                EndpointTable table = new EndpointTable
                {
                    Id = Guid.NewGuid(),
                    ServiceName = ServiceName,
                    EndPoint = EndPoint,
                    Request = Request,
                    Header = Header,
                    RequestType = RequestType,
                    Action = Action,
                    CreateDate = DateTime.Now,
                    IsDeleted = "",
                    ModifiedDate = DateTime.Now,
                    Response = "",
                };
                fastpayTools.EndpointTable.Add(table);
                fastpayTools.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static EndpointModel GetEndpoint(Guid id)
        {
            var item = fastpayTools.EndpointTable.Where(i => i.Id == id).FirstOrDefault();
            var result = new EndpointModel() { Action = item.Action, CreateDate = item.CreateDate, EndPoint = item.EndPoint, Header = item.Header, Id = item.Id, IsDeleted = item.IsDeleted, ModifiedDate = item.ModifiedDate, Request = item.Request, RequestType = item.RequestType, Response = item.Response, ServiceName = item.ServiceName };
            return result;
        }
        public static string GetResponseMessage(string key, Guid Id)
        {

            try
            {
                return CacheManager.ReadList("response", key, Id);
            }
            catch
            {
                return "Hata";
            }
        }
        public static string GetRequestMessage(string key, Guid Id)
        {

            try
            {
                return CacheManager.ReadList("request", key, Id);
            }
            catch
            {
                return "Hata";
            }
        }
        public static string PostDeleteEndpoint(Guid Id)
        {

            try
            {
                var endpoint = fastpayTools.EndpointTable.FirstOrDefault(i => i.Id == Id);
                fastpayTools.EndpointTable.Remove(endpoint);
                fastpayTools.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static EndpointModel GetUpdateEndpoint(Guid Id)
        {
            var endpoint = fastpayTools.EndpointTable.FirstOrDefault(i => i.Id == Id);
            return new EndpointModel
            {
                Action = endpoint.Action,
                CreateDate = endpoint.CreateDate,
                EndPoint = endpoint.EndPoint,
                Header = endpoint.Header,
                Id = endpoint.Id,
                IsDeleted = endpoint.IsDeleted,
                ModifiedDate = endpoint.ModifiedDate,
                Request = endpoint.Request,
                RequestType = endpoint.RequestType,
                Response = endpoint.Response,
                ServiceName = endpoint.ServiceName
            };
        }
        public static string PostUpdateEndpoint(Guid Id, string ServiceName, string EndPoint, string Request, string Header, int RequestType, string Action)
        {

            try
            {
                var endpoint = fastpayTools.EndpointTable.FirstOrDefault(i => i.Id == Id);
                endpoint.ServiceName = ServiceName;
                endpoint.EndPoint = EndPoint;
                endpoint.Request = Request;
                endpoint.Header = Header;
                endpoint.RequestType = RequestType;
                endpoint.Action = Action;
                fastpayTools.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }
        public static string PostDeleteSort(string SortName)
        {
            try
            {
                var sort = fastpayTools.Sort.Where(i => i.SortName == SortName).ToList();
                foreach (var item in sort)
                {
                    fastpayTools.Sort.Remove(item);
                }
                fastpayTools.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static List<EndpointModel> GetUpdateSort(string SortName)
        {
            var sort = fastpayTools.Sort.Where(i => i.SortName == SortName).ToList();
            List<EndpointModel> model = new List<EndpointModel>();
            foreach (var item in sort)
            {
                var endpoint = fastpayTools.EndpointTable.Where(i => i.Id == item.EndpointID).FirstOrDefault();
                model.Add(new EndpointModel
                {
                    Id = endpoint.Id,
                    Action = endpoint.Action,
                    EndPoint = endpoint.EndPoint
                });
            }
            return model;
        }
        public static string PostUpdateSort(List<Guid> Id, string sortName, string newSortName)
        {
            try
            {
                if (Id == null) throw new Exception("No endpoints in the list");
                if (newSortName == null) throw new Exception("Sort name is null");
                var sortList = fastpayTools.Sort.Where(i => i.SortName == sortName).ToList();
                foreach (var item in sortList)
                {
                    fastpayTools.Sort.Remove(item);
                }
                for (int i = 0; i < Id.Count; i++)
                {
                    fastpayTools.Sort.Add(new Sort { EndpointID = Id[i], SortName = newSortName, RequestSorts = i });
                }
                fastpayTools.SaveChanges();
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        private static void GetCacheSortKeyClear(string key)
        {
            CacheManager.Clear(key);
        }
    }
}