using WebApiAutomation.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebApiAutomation.Controllers
{
    public class EndpointController : Controller
    {
        // GET: Endpoint
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EndpointList()
        {
            return View();
        }
        public ActionResult SortList()
        {
            return View();
        }
        public JsonResult RequestClient(string url, List<string> ActionName, List<string> header, List<string> request, string key)
        {
            return Json(EndpointService.GetRequestClient(url, ActionName, header, request, key));
        }
        public JsonResult GetSortList(string sortName)
        {
            return Json(EndpointService.GetSortList(sortName));
        }
        public JsonResult PostSort(List<Guid> Id, string sortName)
        {
            return Json(EndpointService.PostSort(Id, sortName));
        }
        public JsonResult PostEndpoint(string ServiceName, string EndPoint, string Request, string Header, int RequestType, string Action)
        {
            return Json(EndpointService.PostEndpoint(ServiceName, EndPoint, Request, Header, RequestType, Action));
        }
        public JsonResult GetEndpoint(Guid id)
        {
            return Json(EndpointService.GetEndpoint(id));
        }
        public JsonResult GetResponseMessage(string key, Guid id)
        {
            return Json(EndpointService.GetResponseMessage(key, id));
        }
        public JsonResult GetRequestMessage(string key, Guid id)
        {
            return Json(EndpointService.GetRequestMessage(key, id));
        }
        public JsonResult DeleteEndpoint(Guid id)
        {
            return Json(EndpointService.PostDeleteEndpoint(id));
        }
        public JsonResult GetUpdateEndpoint(Guid id)
        {
            return Json(EndpointService.GetUpdateEndpoint(id));
        }
        public JsonResult UpdateEndpoint(Guid Id, string ServiceName, string EndPoint, string Request, string Header, int RequestType, string Action)
        {
            return Json(EndpointService.PostUpdateEndpoint(Id, ServiceName, EndPoint, Request, Header, RequestType, Action));
        }
        public JsonResult DeleteSort(string SortName)
        {
            return Json(EndpointService.PostDeleteSort(SortName));
        }
        public JsonResult GetUpdateSort(string SortName)
        {
            return Json(EndpointService.GetUpdateSort(SortName));
        }
        public JsonResult UpdateSort(List<Guid> Id, string sortName, string newSortName)
        {
            return Json(EndpointService.PostUpdateSort(Id, sortName, newSortName));
        }
    }
}