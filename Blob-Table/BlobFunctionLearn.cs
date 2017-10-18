using System.Collections.Generic;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Blob_Table
{
    public static class BlobFunctionLearn
    {
        [FunctionName("BlobFunctionLearn")]
        public static void Run([BlobTrigger("learn-blob/{name}", Connection = "BlobConnection")]Stream myBlob, string name, TraceWriter log,
            [Table("WeddingGuests", Connection = "TableConnection")]ICollector<WeddingGuest> table)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
            var reader = new StreamReader(myBlob);
            var textAsString = reader.ReadToEnd();
            var weddingGuestList = JsonConvert.DeserializeObject<WeddingGuestRequest>(textAsString);

            foreach (var guest in weddingGuestList.WeddingGuests)
            {
                guest.PartitionKey = guest.Id.ToString();
                guest.RowKey = guest.Name;
                table.Add(guest);
            }
        }
    }


    public class WeddingGuest: TableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Attending { get; set; }
    }

    public class WeddingGuestRequest
    {
        public IEnumerable<WeddingGuest> WeddingGuests { get; set; }
    }
}
