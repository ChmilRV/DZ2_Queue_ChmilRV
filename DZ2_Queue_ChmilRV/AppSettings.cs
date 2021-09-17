using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace DZ2_Queue_ChmilRV
{
	public class AppSettings
	{
      public string StorageConnectionString { get; set; }

      public string QueueName { get; set; }

      public AppSettings()
      {
         var json = File.ReadAllText("AppSetting.json");
         var appSettings = JsonDocument.Parse(json, new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip });
         StorageConnectionString = appSettings.RootElement.GetProperty("StorageConnectionString").GetString();
         QueueName = appSettings.RootElement.GetProperty("QueueName").GetString();
      }


   }
}
