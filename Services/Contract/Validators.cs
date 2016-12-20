using System;
using System.Text.RegularExpressions;
using Zippy.Utils;

namespace Zippy.Services.Contract
{
    public static class Validators
    {
        public static void EnsureValidAddress(string address)
        {
            Throw.IfFalse(IsValidAddress(address), "Invalid address");
        }

        public static bool IsValidAddress(string address)
        {
            return !string.IsNullOrWhiteSpace(address);
        }

        public static void EnsureValidName(string name)
        {
            Throw.IfFalse(IsValidName(name), "Invalid name");
        }

        public static bool IsValidName(string name)
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        public static void EnsureValidZip(string zip)
        {
            string error;
            if (!IsValidZip(zip, out error))
            {
                throw new ArgumentException(error ?? "Invalid Zip");
            }
        }

        public static bool IsValidZip(string zip)
        {
            string error;
            return IsValidZip(zip, out error);

        }

        public static bool IsValidZip(string zip, out string error)
        {
            if (string.IsNullOrWhiteSpace(zip))
            {
                error = "Zip cannot be blank";
                return false;
            }

            if (zip.Length != 5 || !Regex.IsMatch(zip, "\\d{5}"))
            {
                error = "Zip has to be a 5 digit number eg. 94043";
                return false;
            }

            error = null;

            return true;
        }
    }
}