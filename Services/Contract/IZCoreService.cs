using System.Collections.Generic;
using System.Threading.Tasks;
using Zippy.Models;

namespace Zippy.Services.Contract
{
    public interface IZCoreService
    {
        Task<Person> LocatePerson(string name, string address);

        Task<Person> FindPerson(string name);

        Task<ISet<Person>> FindPersons(string zip);
    }
}