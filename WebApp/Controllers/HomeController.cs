using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public static int RequestNumber = 0;

        public ActionResult Index()
        {
            HomeController.RequestNumber++;
            string url = Environment.GetEnvironmentVariable("ApiUrl");

            if (string.IsNullOrEmpty(url))
            {
                url = ConfigurationManager.AppSettings["ApiUrl"];
            }

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                MediaTypeWithQualityHeaderValue contentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.Accept.Add(contentType);
                HttpResponseMessage response = client.GetAsync("api/LocalMachine").Result;

                if (response.IsSuccessStatusCode)
                {
                    string stringData = response.Content.ReadAsStringAsync().Result;
                    WebApiResult data = JsonConvert.DeserializeObject<WebApiResult>(stringData);
                    ResultIp result = new ResultIp();
                    result.ApiIp = data.ApiIp;
                    result.ApiMachineName = data.ApiMachineName;
                    result.ApiOS = data.ApiOS;
                    result.ApiRequestNumber = data.RequestNumber;

                    IPHostEntry host;

                    host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (IPAddress ip in host.AddressList)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            result.AppIp = ip.ToString();
                            result.AppMachineName = Environment.MachineName;
                            result.AppOS = Environment.OSVersion.VersionString;
                            result.AppRequestNumber = HomeController.RequestNumber;
                        }
                    }

                    return View(result);
                }
                else
                {
                    ViewBag.Message = response.StatusCode + " : Message - " + response.ReasonPhrase;
                    return View();
                }
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string Values()
        {
            string result = string.Empty;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiUrl"]);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("api/values").Result;

            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                result = response.StatusCode + " : Message - " + response.ReasonPhrase;
            }

            Response.Write("</BR> Web Api Url : " + client.BaseAddress + "</BR>");
            Response.Write("Web App Host Name : " + Dns.GetHostName() + "</BR>");

            return result;
        }
    }
}