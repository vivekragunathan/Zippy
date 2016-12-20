using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zippy.Data.Contract;
using Zippy.Models;
using Zippy.Services.Contract;

namespace Zippy.Services
{
    public class ZCoreService : IZCoreService
    {
        private readonly IGeocodingServiceProvider gcsProvider;
        private readonly IZRepository repository;
        private readonly ILogger<ZCoreService> logger;

        public ZCoreService(IGeocodingServiceProvider gcsProvider,
                                IZRepository repository,
                                ILoggerFactory loggerFactory)
        {
            this.gcsProvider = gcsProvider;
            this.repository = repository;
            this.logger = loggerFactory.CreateLogger<ZCoreService>();
        }

        public async Task<Person> FindPerson(string name)
        {
            logger.LogInformation($"FindPerson({name}) ...");
            Validators.EnsureValidName(name);
            return await repository.FindPersonAsync(name);
        }

        public async Task<ISet<Person>> FindPersons(string zip)
        {
            logger.LogInformation($"FindPersons({zip}) ...");
            Validators.EnsureValidZip(zip);
            return await repository.FindPersons(zip);
        }

        public async Task<Person> LocatePerson(string name, string address)
        {
            logger.LogInformation($"LocatePerson({name}, {address}) ...");

            Validators.EnsureValidName(name);
            Validators.EnsureValidAddress(address);

            var location = await gcsProvider.ResolveAddressAsync(address);

            if (location == null)
            {
                logger.LogInformation($"GCS failed to resolve address ({address}");
                return null;
            }

            var person = new Person(name, location.FormattedAddress, location.ZipCode);
            await repository.UpsertAsync(person);
            return person;
        }
    }
}