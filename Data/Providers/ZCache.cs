using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Zippy.Utils;
using Zippy.Models;
using Zippy.Data.Contract;

using ZipWiseCache = System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<Zippy.Models.Person>>;
using NameWiseCache = System.Collections.Generic.Dictionary<string, Zippy.Models.Person>;

namespace Zippy.Data.Providers
{
    public class ZCache : IZCache
    {
        #region Member Declarations

        private static readonly ISet<Person> NoPersonsFoundSet = new HashSet<Person>();

        private readonly ZipWiseCache zipWiseCache = new ZipWiseCache();
        private readonly NameWiseCache nameWiseCache = new NameWiseCache();
        private readonly ILogger<ZCache> logger;

        #endregion

        public ZCache(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<ZCache>();
        }

        #region IZCache Interface Implementation

        public ISet<Person> FindPersons(string zip)
        {
            Throw.IfBlank(zip, $"{nameof(zip)} cannot be blank");
            return zipWiseCache.ContainsKey(zip) ? zipWiseCache[zip] : NoPersonsFoundSet;
        }

        public Person FindPerson(string name)
        {
            Throw.IfBlank(name, $"{nameof(name)} cannot be blank");

            if (nameWiseCache.ContainsKey(name))
            {
                Debug.Assert(
                    FindPersonInZipCache(name) != null,
                    $"INTERNAL ERROR! {nameof(name)} does not exist in {nameof(nameWiseCache)}"
                );

                return nameWiseCache[name];
            }

            return null;
        }

        /// If no person by person.Name exists in the cache, then
        /// add person to cache. Otherwise, updates the person 
        /// in the cache.
        public void UpsertPerson(Person person)
        {
            Throw.IfNull(person, $"{nameof(person)} cannot be null");
            Throw.IfBlank(person.ZipCode, "Cannot add person without to zipcode to cache");

            if (!zipWiseCache.ContainsKey(person.ZipCode))
            {
                zipWiseCache.Add(person.ZipCode, new HashSet<Person>());
            }

            var nwcPerson = FindPerson(person.Name);

            if (nwcPerson != null)
            {
                var persons = zipWiseCache[nwcPerson.ZipCode];
                var removed = persons.RemoveWhere(p => p.Equals(nwcPerson));
            }

            // Update zipWiseCache
            zipWiseCache[person.ZipCode].Add(person);

            // Update nameWiseCache
            nameWiseCache[person.Name] = person;
        }

        #endregion

        #region Private and Helper Methods

        /// This method should be used only for Diagnostics purposes
        private Person FindPersonInZipCache(string name)
        {
            return zipWiseCache.Values
                .SelectMany(hs => hs.AsEnumerable())
                .Where(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();
        }

        #endregion
    }
}