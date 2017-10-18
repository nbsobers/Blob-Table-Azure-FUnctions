using System.IO;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Blob_Table
{
    public static class BlobFunctionWithCosmosdb
    {
        [FunctionName("BlobFunctionWithCosmosdb")]
        public static void Run([BlobTrigger("cosmosdb-blob/{name}", Connection = "BlobConnection")]Stream myBlob, string name, TraceWriter log,
            [DocumentDB("Wedding","guestcollection",ConnectionStringSetting = "CosmosDBConnection")] ICollector<WeddingGuest> documentDB)
        {
            //https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-class-library

            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            var reader = new StreamReader(myBlob);
            var textAsString = reader.ReadToEnd();
            var weddingGuestList = JsonConvert.DeserializeObject<WeddingGuestRequest>(textAsString);

            foreach (var guest in weddingGuestList.WeddingGuests)
            {
                guest.PartitionKey = guest.Id.ToString();
                guest.RowKey = guest.Name;
                documentDB.Add(guest);
            }
        }
    }
}
