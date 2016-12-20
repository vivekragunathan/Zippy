using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zippy.Models;
using Zippy.Utils;

/// <summary>
/// TODO: Work in Progress
/// </summary>
namespace Zippy.Services
{
    internal class GoogleGeoCodingService : IGeocodingServiceProvider
    {
        private const string ServiceName = "Google";
        private const string DEFAULT_API_KEY = "AIzaSyAF2dX9X2Mo4VpHwlgmaJgW4ps6saHAzNo";
        private const string ServiceUrlFmtSpec = "https://maps.googleapis.com/maps/api/geocode/json?address={0}&key=";

        private static readonly HttpClient Invoker = new HttpClient();

        private readonly string apiKey;
        private readonly string endPointUrlFmtSpec;
        private readonly ILogger<GoogleGeoCodingService> logger;

        public GoogleGeoCodingService(ILoggerFactory factory) : this(DEFAULT_API_KEY, factory)
        {
        }

        public GoogleGeoCodingService(string apiKey, ILoggerFactory factory)
        {
            Throw.IfBlank(apiKey, "API key cannot be null/empty/blank");
            this.apiKey = apiKey;
            this.endPointUrlFmtSpec = ServiceUrlFmtSpec + apiKey;
            this.logger = factory.CreateLogger<GoogleGeoCodingService>();
        }

        public async Task<LocationInfo> ResolveAddressAsync(string address)
        {
            logger.LogInformation($"[Google] ResolveAddressAsync({address})");
            var url = string.Format(endPointUrlFmtSpec, Uri.EscapeDataString(address));
            var response = await Invoker.GetStringAsync(url);
            Throw.IfBlank(response, "Failed to locate address. (No data received)");
            return new GoogleGeoCodingResponseParser(response).Parse();
        }

        public string Name => ServiceName;
    }
}