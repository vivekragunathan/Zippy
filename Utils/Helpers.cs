using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Zippy.Utils
{
    internal static class Helpers
    {
        public static readonly JsonSerializerSettings DefaultJsonSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static ILoggerFactory CreateLoggerFactory()
        {
            return new LoggerFactory()
                 .AddConsole()
                 .AddDebug();
        }

        public static async Task<string> ReadAllTextAsync(string filePath)
        {
            using (StreamReader reader = File.OpenText(filePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static async Task WriteAllTextAsync(string path, string content)
        {
            using (var stream = File.Open(path, FileMode.Create, FileAccess.ReadWrite))
            {
                var buffer = Encoding.UTF8.GetBytes(content);
                await stream.WriteAsync(buffer, 0, content.Length);
            }
        }

        public static ISet<T> DistinctOf<T>(IEnumerable<T> left, IEnumerable<T> right)
        {
            var result = new HashSet<T>();

            foreach (var item in left)
            {
                result.Add(item);
            }

            foreach (var item in right)
            {
                result.Add(item);
            }

            return result;
        }

        public static ISet<T> ToSet<T>(IEnumerable<T> source)
        {
            var set = new HashSet<T>();

            foreach (var item in source)
            {
                set.Add(item);
            }

            return set;
        }
    }
}
