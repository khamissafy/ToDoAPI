using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Text;
using ToDoListAPI.Models.UserManagement.Custom_Models;
using MimeTypes;

namespace ToDoListAPI.Helpers
{
    public static class RegexValidations
    {
        public static bool Email_Validation(string email)
        {
            //string reg = "(?:[a-z0-9!#$%&'*+\\=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+\\=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";
            string reg = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,63}$";
            return Regex.IsMatch(email, reg);
        }

        public static bool Guid_Validation(string candidate)
        {
            if (string.IsNullOrWhiteSpace(candidate))
            {
                return false;
            }

            // Define the regular expression pattern for a GUID
            string guidPattern = @"[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$";

            // Check if the input string matches the pattern
            return Regex.IsMatch(candidate, guidPattern);
        }

        public static bool IPAddress_Validation(string ip, out string ipAddress)
        {
            var match = Regex.Match(ip, @"^((25[0-5]|(2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(25[0-5]|(2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9]))$");
            ipAddress = match.Value;
            return match.Success;
        }

        public static bool PhoneNumber_Validation(string candidate)
        {
            if (string.IsNullOrWhiteSpace(candidate))
            {
                return false;
            }

            // Define the regular expression pattern for a phone number
            // This pattern allows digits, spaces, hyphens, parentheses, and can start with a + sign
            string phonePattern = @"^\+?[0-9\s\-\(\)]{7,15}$";

            // Check if the input string matches the pattern
            return Regex.IsMatch(candidate, phonePattern);
        }
        public static bool Password_Validation(string candidate)
        {
            if (string.IsNullOrWhiteSpace(candidate) || candidate.Length < 8)
            {
                return false;
            }

            // Define the regular expression pattern for a valid password
            // This pattern checks for at least one alphabetic character and at least 8 characters in total
            string passwordPattern = @"^(?=.*[A-Za-z]).{8,}$";

            // Check if the input string matches the pattern
            return Regex.IsMatch(candidate, passwordPattern);
        }
        public static bool Base64_Validation(string candidate)
        {
            if (string.IsNullOrWhiteSpace(candidate))
            {
                return false;
            }

            // Split the string into parts by comma
            var parts = candidate.Split(new[] { ',' }, 2);

            // Ensure there are exactly two parts
            if (parts.Length != 2)
            {
                return false;
            }

            // Validate the media type and Base64 part
            string mediaType = parts[0];
            string base64Data = parts[1];

            // Check if the media type is valid for images
            if (!mediaType.StartsWith("data:image/") || !mediaType.Contains(";base64"))
            {
                return false;
            }

            // Ensure the Base64 data length is a multiple of 4
            if (base64Data.Length % 4 != 0)
            {
                return false;
            }

            try
            {
                // Try to convert the string from Base64 to byte array
                Convert.FromBase64String(base64Data);
                return true;
            }
            catch (FormatException)
            {
                // If conversion fails, it means the string is not valid Base64
                return false;
            }
        }


    }
}
