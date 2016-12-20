using System.Collections.Generic;
using Zippy.Models;

namespace Zippy.Data.Contract
{
    public interface IZCache
    {
        ISet<Person> FindPersons(string zip);

        Person FindPerson(string name);

        /// If no person by person.Name exists in the cache, then
        /// add person to cache. Otherwise, updates the person 
        /// in the cache.
        void UpsertPerson(Person person);
    }
}