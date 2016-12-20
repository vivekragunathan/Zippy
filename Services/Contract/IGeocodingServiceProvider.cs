using System.Threading.Tasks;
using Zippy.Models;

namespace Zippy.Services
{
    public interface IGeocodingServiceProvider
    {
        string Name { get; }
        Task<LocationInfo> ResolveAddressAsync(string address);
    }
}
