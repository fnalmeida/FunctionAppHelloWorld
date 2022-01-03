using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionAppTimeTrigger
{
    public class FunctionTimerTrigger
    {
        [FunctionName("FunctionTimerTrigger")]
        public void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string json = @"{
                          'name' : 'fabricio'
                        }";


            var result = ConnectService.PostData(json, "http://functionhelloworld.azurewebsites.net", "/api/FunctionHelloWorld");

            log.LogInformation($"Requisiçao: {result.StatusCode}");
        }
    }
}
