using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Blob_Table
{
    public static class HttpFunction
    {
        [FunctionName("HttpFunction")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log,
              [DocumentDB("Wedding", "guestcollection", ConnectionStringSetting = "CosmosDBConnection")] IEnumerable<dynamic> documentDB)
        {
            log.Info("C# HTTP trigger function processed a request.");
            return req.CreateResponse(HttpStatusCode.OK, documentDB.ToList());
        }
    }
}
