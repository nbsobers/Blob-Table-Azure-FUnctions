//using System;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.WindowsAzure.Storage.Table;
//using Newtonsoft.Json;

//namespace Blob_Table
//{
//    public static class Function2
//    {
//        [FunctionName("Function2")]
//        public static void Run([QueueTrigger("testqueue", Connection = "QueueConnection")]string myQueueItem, TraceWriter log,
//             [Table("WeddingGuests", Connection = "TableConnection")]ICollector<WeddingGuest> table)
//        {
//            log.Info($"C# Queue trigger function processed: {myQueueItem}");

//            var weddingGuestList = JsonConvert.DeserializeObject<WeddingGuestRequest>(myQueueItem);

//            foreach(var guest in weddingGuestList.WeddingGuests)
//            {
//                guest.PartitionKey = guest.Id.ToString();
//                guest.RowKey = guest.Name;
//                table.Add(guest);
//            }
//        }
//    }

//    //public class WeddingGuest : TableEntity
//    //{
//    //    public string Name { get; set; }
//    //    public string Email { get; set; }
//    //    public bool Attending { get; set; }
//    //}
//}
