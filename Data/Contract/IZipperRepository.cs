using System.Collections.Generic;
using System.Threading.Tasks;
using Zippy.Models;

namespace Zippy.Data.Contract
{
    /// Although it might appear similar, the repository 
    /// interface/API is different from IZDbDriver 
    /// and will expose extra and higher level APIs.
    public interface IZRepository
    {
        string GetApiKey();
        Task<Person> FindPersonAsync(string name);
        Task<ISet<Person>> FindPersons(string zip);
        Task UpsertAsync(Person person);
    }
}
