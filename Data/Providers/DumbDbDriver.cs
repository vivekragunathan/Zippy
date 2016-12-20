using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Zippy.Data.Contract;
using Zippy.Models;
using Zippy.Utils;

namespace Zippy.Data.Providers
{
    public class DumbDbDriver : IZDbDriver
    {
        private const string DUMB_DB_FILE_NAME = "zdb.json";
        private readonly ISet<Person> persons;

        public DumbDbDriver()
        {
            if (!File.Exists(DUMB_DB_FILE_NAME))
            {
                File.WriteAllText(DUMB_DB_FILE_NAME, "[]");
            }

            var records = LoadPersons();
            this.persons = new HashSet<Person>(records);
        }

        public IEnumerable<Person> Persons
        {
            get
            {
                return persons;
            }
        }

        #region IZDbDriver Interface Implementation

        public async Task<Person> FindPersonAsync(string name)
        {
            Throw.IfBlank(name, "Cannot query person(s) with blank name");

            await Task.Run(() => Thread.Sleep(1));

            foreach (var person in persons)
            {
                if (person.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return person;
                }
            }

            return null;
        }

        public async Task<ISet<Person>> FindPersonsAsync(string zip)
        {
            Throw.IfBlank(zip, "Cannot query person(s) with blank name");

            await Task.Run(() => Thread.Sleep(1));

            var records = persons.Where(p => p.ZipCode.Equals(zip));

            return Helpers.ToSet(records);
        }

        public async Task SaveAsync(Person person)
        {
            Throw.IfNull(person, "Cannot persist null person object");

            var task = FindPersonAsync(person.Name);
            var p2 = task.Result;

            if (p2 != null)
            {
                throw new Exception($"Person already exists ({person.Name})");
            }

            persons.Add(person);

            await PersistAllPersons();
        }

        public async Task<bool> UpdateAsync(Person person)
        {
            Throw.IfNull(person, "Cannot persist null person object");

            var p2 = await FindPersonAsync(person.Name);

            if (p2 == null)
            {
                throw new Exception($"Person does not exist ({person.Name})");
            }

            bool removed = persons.Remove(p2);

            persons.Add(person);

            await PersistAllPersons();

            return true;
        }

        public async Task UpsertAsync(Person person)
        {
            Throw.IfNull(person, "Cannot save/update null person");

            if (FindPersonAsync(person.Name) == null)
            {
                await SaveAsync(person);
            }
            else
            {
                await UpdateAsync(person);
            }
        }

        #endregion

        #region IDisposable Interface Implementation

        public void Dispose()
        {
        }

        #endregion

        #region Private and Helpers Methods

        private IEnumerable<Person> LoadPersons()
        {
            var content = File.ReadAllText(DUMB_DB_FILE_NAME);
            return (List<Person>)JsonConvert.DeserializeObject(content, typeof(List<Person>));
        }

        private async Task PersistAllPersons()
        {
            var data = JsonConvert.SerializeObject(persons);
            await Helpers.WriteAllTextAsync(DUMB_DB_FILE_NAME, data);
        }

        #endregion
    }
}
