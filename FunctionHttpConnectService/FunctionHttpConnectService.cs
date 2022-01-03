using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace FunctionHttpConnectService
{
    public static class FunctionHttpConnectService
    {
        [FunctionName("FunctionHttpConnectService")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string json = @"{
                          'name' : 'fabricio'
                        }";

            var result = ConnectService.PostData(json, "http://functionhelloworld.azurewebsites.net", "/api/FunctionHelloWorld");

            log.LogInformation($"Requisiçao: {result.StatusCode}");

            return new OkObjectResult($"Requisiçao: {result.StatusCode}");
        }
    }

    public static class ConnectService
    {

        public static HttpResponseMessage GetData(string server, string api)
        {
            using (var client = new HttpClient())
            {
                return client.GetAsync(server + api).Result;
            }
        }

        public static HttpResponseMessage GetDataBasicAuth(string url, string user, string password)
        {

            HttpClientHandler handle = new HttpClientHandler();
            var byteArray = Encoding.ASCII.GetBytes("" + user + ":" + password + "");
            var auth = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = auth;
                return client.GetAsync(url).Result;
            }
        }

        public static HttpResponseMessage PostData(Object obj, string server, string service)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(obj);
                var strContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(server);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                return client.PostAsync(service, strContent).Result;
            }
        }

        public static HttpResponseMessage PostData(string json, string server, string api)
        {
            using (var client = new HttpClient())
            {
                var strContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                client.BaseAddress = new Uri(server);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                return client.PostAsync(api, strContent).Result;
            }
        }

        public static HttpResponseMessage PutData(Object obj, string server, string service)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(obj);
                var strContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                client.BaseAddress = new Uri(server);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                return client.PutAsync(service, strContent).Result;
            }
        }

    }
}
