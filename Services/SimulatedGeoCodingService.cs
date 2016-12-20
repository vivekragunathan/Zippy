using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zippy.Models;
using Zippy.Utils;

namespace Zippy.Services
{
    internal class SimulatedGeoCodingService : IGeocodingServiceProvider
    {
        private const string ServiceName = "Simulator";

        private readonly ILogger<SimulatedGeoCodingService> logger;

        public SimulatedGeoCodingService(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<SimulatedGeoCodingService>();
        }

        public async Task<LocationInfo> ResolveAddressAsync(string address)
        {
            var jsonFilePath = Directory.GetCurrentDirectory() + "/Services/location.json";

            logger.LogWarning("[SIMULATION] Locating address ...");
            var response = await Helpers.ReadAllTextAsync(jsonFilePath);

            Throw.IfBlank(response, "[SIMULATION] Failed to locate address. (No data received)");
            logger.LogInformation("[SIMULATION] Parsing location service response ....");

            return new GoogleGeoCodingResponseParser(response).Parse();
        }

        public string Name => ServiceName;
    }
}
