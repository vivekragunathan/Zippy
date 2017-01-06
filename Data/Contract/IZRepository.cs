using System.Collections.Generic;
using System.Threading.Tasks;
using Zippy.Models;

namespace Zippy.Data.Contract
{
	/// <summary>
	/// The repository is the abstraction over data storage
	/// as whole. It combines the physical data storage
	/// and the associate caching mechanism.
	///
	/// Although it might appear similar, the repository
	/// interface/API is different from IZDbDriver
	/// and will expose extra and higher level APIs.
	/// </summary>
	public interface IZRepository
	{
		string GetApiKey();
		Task<Person> FindPersonAsync(string name);
		Task<ISet<Person>> FindPersons(string zip);
		Task<Person> UpsertAsync(string name, string address, string zip);
	}
}
