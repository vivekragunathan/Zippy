using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Zippy.Data.Contract;
using Zippy.Models;
using Zippy.Utils;

namespace Zippy.Data.Providers
{
	public class MongoDbDriver : IZDbDriver
	{
		private const string ZIPPY_DB_NAME = "zippy";
		private const string COLLECTIONS_PERSON = "persons";

		private static IMongoClient client;
		private static IMongoDatabase database;
		private IMongoCollection<Person> persons;
		private ILogger<MongoDbDriver> logger;

		static MongoDbDriver()
		{
			client = new MongoClient();
			database = client.GetDatabase(ZIPPY_DB_NAME);
		}

		public MongoDbDriver(ILoggerFactory factory)
		{
			persons = database.GetCollection<Person>(COLLECTIONS_PERSON);
			logger = factory.CreateLogger<MongoDbDriver>();
		}

		public async Task<Person> FindPersonAsync(string name)
		{
			logger.LogInformation($"FindPersonAsync({name}) ...");

			Throw.IfBlank(name, $"{nameof(name)} cannot be blank");

			return await Task.Run(() =>
			{
				var results = from person in persons.AsQueryable()
								  where person.Name.ToLower() == name.ToLower()
								  select person;

				return results.FirstOrDefault();
			});
		}

		public async Task<ISet<Person>> FindPersonsAsync(string zip)
		{
			logger.LogInformation($"FindPersonsAsync({zip}) ...");
			Throw.IfBlank(zip, $"{nameof(zip)} cannot be blank");
			var task = await persons.FindAsync(new BsonDocument { { Person.FIELD_ZIP, zip } });
			return Helpers.ToSet<Person>(task.ToEnumerable<Person>());
		}

		public async Task SaveAsync(Person person)
		{
			logger.LogInformation($"SaveAsync({person}) ...");
			await persons.InsertOneAsync(person);
		}

		public async Task<bool> UpdateAsync(Person person)
		{
			logger.LogInformation($"UpdateAsync({person}) ...");

			var result = await persons.ReplaceOneAsync(
				new BsonDocument(Person.FIELD_NAME, person.Name),
				person
			);

			return result.IsAcknowledged;
		}

		public async Task UpsertAsync(Person person)
		{
			logger.LogInformation($"UpsertAsync({person}) ...");

			await persons.ReplaceOneAsync(
				new BsonDocument(Person.FIELD_NAME, person.Name),
				person,
				new UpdateOptions { IsUpsert = true }
			);
		}

		#region IDisposable Support

		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}