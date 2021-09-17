using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Threading.Tasks;

namespace DZ2_Queue_ChmilRV
{
	class Program
	{


		// We'll need a connection string to your Azure Storage account.
		static string connectionString = "";

		// Name of the queue we'll send messages to
		static string queueName = "";

		static QueueClient queueClient;




		static async Task Main(string[] args)
		{
			Console.WriteLine("Работа с очередью\n");

			AppSettings appSettings = new AppSettings();
			connectionString = appSettings.StorageConnectionString;
			queueName = appSettings.QueueName;

			// Instantiate a QueueClient which will be used to create and manipulate the queue
			queueClient = new QueueClient(connectionString, queueName);


			Console.WriteLine("Отправка в очередь...");
			await SendMessageQueue("Тестовое сообщение для проверки очереди");

			Console.WriteLine("\n\nЧтение из очереди...");
			await ReciveAndDeleteMessageQueue();

			appSettings = null;


			Console.ReadKey();

		}

		static async Task ReciveAndDeleteMessageQueue()
		{
			int counter = 1;
			var responceQueue = await queueClient.ReceiveMessagesAsync(maxMessages: 10);

			Console.WriteLine($"Получено сообщений: {responceQueue.Value.Length}");
			// Get the next messages from the queue
			foreach (QueueMessage message in responceQueue.Value)
			{
				// "Process" the message
				Console.WriteLine($"Сообщение №{counter++} id - {message.MessageId} : {message.Body}");

				// Let the service know we're finished with the message and
				// it can be safely deleted.
				//await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
			}
		}

		static async Task<bool> SendMessageQueue(string message)
		{
			try
			{
				// Create the queue
				await queueClient.CreateIfNotExistsAsync();

				if (queueClient.Exists())
				{
					// Send a message to the queue
					await queueClient.SendMessageAsync(message);
					Console.WriteLine($"Отправлено: {message}");
					return true;
				}
				else
				{
					Console.WriteLine($"Убедитесь что очередь существует и сервис доступен");
					return false;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Исключение: {ex.Message}\n\n");
				Console.WriteLine($"Убедитесь что очередь существует и сервис доступен.");
			}
			return false;
		}
	}
}
