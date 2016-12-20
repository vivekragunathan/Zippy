using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Zippy.Models;
using Zippy.Utils;

namespace Zippy.Services
{
    public class GoogleGeoCodingResponseParser
    {
        private readonly dynamic results;

        public GoogleGeoCodingResponseParser(string response)
        {
            Throw.IfBlank(response, "Cannot parse blank response text");
            ResponseText = response;

            dynamic jsonObj = JObject.Parse(ResponseText);
            EnsureStatusOK(jsonObj);
            this.results = ParseResults(jsonObj);
        }

        public String ResponseText { get; set; }

        public LocationInfo Parse()
        {
            var location = results[0];

            var formattedAddress = location.formatted_address?.ToString();

            Throw.IfBlank(formattedAddress, "Failed to parse response. Formatted address not does not exist.");

            var components = location.address_components;

            Throw.IfNullOrEmpty(components, "Failed to parse response. No address components in result.");

            var result = ParseAddress(components);

            result.FormattedAddress = formattedAddress;

            return result;
        }

        private static LocationInfo ParseAddress(dynamic addressComponents)
        {
            Throw.IfNullOrEmpty(addressComponents, "Failed to parse response. Address components missing!");

            string houseNo = null;
            string streetName = null;
            string city = null;
            string state = null;
            string county = null;
            string country = null;
            string zip = null;

            foreach (dynamic component in addressComponents)
            {
                if (component == null) { continue; }

                AddressComponentType type = DeduceComponentType(component.types);
                if (type == null) { continue; }

                if (type == AddressComponentType.HouseNo)
                {
                    houseNo = component.long_name;
                }
                else if (type == AddressComponentType.StreetName)
                {
                    streetName = component.long_name;
                }
                else if (type == AddressComponentType.City)
                {
                    city = component.long_name;
                }
                else if (type == AddressComponentType.County)
                {
                    county = component.long_name;
                }
                else if (type == AddressComponentType.State)
                {
                    state = component.long_name;
                }
                else if (type == AddressComponentType.Country)
                {
                    country = component.long_name;
                }
                else if (type == AddressComponentType.ZipCode)
                {
                    zip = component.long_name;
                }
            }

            return new LocationInfo()
            {
                HouseNo = houseNo,
                StreetName = streetName,
                City = city,
                State = state,
                County = county,
                Country = country,
                ZipCode = zip
            };
        }

        private static dynamic ParseResults(dynamic jsonObj)
        {
            var results = jsonObj.results;
            Throw.IfNullOrEmpty(results, "Failed to parse response. No results in response.");
            return results;
        }

        private static void EnsureStatusOK(dynamic jsonObj)
        {
            Throw.IfFalse(
                "OK".Equals(jsonObj?.status?.ToString(), StringComparison.CurrentCultureIgnoreCase),
                "Failed to parse response. Status not OK!"
            );
        }

        private static AddressComponentType DeduceComponentType(dynamic types)
        {
            foreach (string type in types)
            {
                var componentType = (AddressComponentType)type;
                if (type != null)
                {
                    return componentType;
                }
            }

            return null;
        }
    }

    internal sealed class AddressComponentType
    {
        private static readonly Dictionary<string, AddressComponentType> cache = new Dictionary<string, AddressComponentType>();

        public static readonly AddressComponentType HouseNo = new AddressComponentType("street_number");
        public static readonly AddressComponentType StreetName = new AddressComponentType("route");
        public static readonly AddressComponentType City = new AddressComponentType("locality");
        public static readonly AddressComponentType County = new AddressComponentType("administrative_area_level_2");
        public static readonly AddressComponentType State = new AddressComponentType("administrative_area_level_1");
        public static readonly AddressComponentType Country = new AddressComponentType("country");
        public static readonly AddressComponentType ZipCode = new AddressComponentType("postal_code");

        private AddressComponentType(string value)
        {
            Value = value;
            cache[value] = this;
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        public static explicit operator AddressComponentType(string value)
        {
            AddressComponentType result;
            return cache.TryGetValue(value, out result) ? result : null;
        }
    }
}
