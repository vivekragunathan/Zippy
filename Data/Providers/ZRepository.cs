using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zippy.Data.Contract;
using Zippy.Models;
using Zippy.Utils;

namespace Zippy.Data.Providers
{
	/// <summary>
	/// See IZRepository for more information
	/// </summary>
	public class ZRepository : IZRepository, IDisposable
	{
		private const string DEFAULT_API_KEY = "AIzaSyAF2dX9X2Mo4VpHwlgmaJgW4ps6saHAzNo";

		private readonly IZDbDriver db;
		private readonly IZCache cache;

		public ZRepository(IZDbDriver db, IZCache cache)
		{
			this.db = db;
			this.cache = cache;
		}

		#region IZRepository Interface Implementation

		public string GetApiKey()
		{
			// Read from database or appsettings file
			return DEFAULT_API_KEY;
		}

		public async Task<Person> FindPersonAsync(string name)
		{
			var person = cache.FindPerson(name);

			if (person != null)
			{
				return person;
			}

			person = await db.FindPersonAsync(name);

			if (person != null)
			{
				cache.UpsertPerson(person);
			}

			return person;
		}

		/// This is just a representational implementation. A
		/// realtime implementation would be guarded by and
		/// involve streaming rather then quering all records
		/// from the database.
		public async Task<ISet<Person>> FindPersons(string zip)
		{
			var fromCache = cache.FindPersons(zip);
			var fromDb = await db.FindPersonsAsync(zip);
			return Helpers.DistinctOf(fromCache, fromDb);
		}

		public async Task UpsertAsync(Person person)
		{
			Throw.IfNull(person, "Cannot save/update null person");

			var task = FindPersonAsync(person.Name);
			var p2 = task.Result;

			if (p2 == null)
			{
				await db.SaveAsync(person);
			}
			else
			{
				await db.UpdateAsync(person);
			}

			cache.UpsertPerson(person);
		}

		#endregion

		#region IDisposable Interface Implementation

		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					db.Dispose();
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