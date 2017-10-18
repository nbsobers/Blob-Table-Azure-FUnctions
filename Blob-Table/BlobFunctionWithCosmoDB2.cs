using System;
using System.Configuration;
using System.IO;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Blob_Table
{
    public static class BlobFunctionWithCosmoDB2
    {
        static string endpoint = ConfigurationManager.AppSettings["Endpoint"];
        static string authKey = ConfigurationManager.AppSettings["AuthKey"];

        [FunctionName("BlobFunctionWithCosmoDB2")]
        public static async void Run([BlobTrigger("cosmosdb-blob2/{name}", Connection = "BlobConnection")]Stream myBlob, string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            var reader = new StreamReader(myBlob);
            var textAsString = reader.ReadToEnd();
            var weddingGuestList = JsonConvert.DeserializeObject<WeddingGuestRequest>(textAsString);

            using (DocumentClient client = new DocumentClient(new Uri(endpoint), authKey, new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp }))
            {
                // get a reference to the database the console app created
                Database database = await client.CreateDatabaseIfNotExistsAsync(new Database { Id = "graphdb" });

                // get an instance of the database's graph
                DocumentCollection graph = await client.CreateDocumentCollectionIfNotExistsAsync(
                    UriFactory.CreateDatabaseUri("graphdb"),
                    new DocumentCollection { Id = "graphcollz" },
                    new RequestOptions { OfferThroughput = 1000 });

                foreach (var guest in weddingGuestList.WeddingGuests)
                {
                    guest.PartitionKey = guest.Id.ToString();
                    guest.RowKey = guest.Name;
                    await client.CreateDocumentAsync(graph.DocumentsLink, guest);
                }
                              

            }
        }         
        
    }
}
