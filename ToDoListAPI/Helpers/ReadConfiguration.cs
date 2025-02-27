 using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
namespace ToDoListAPI.Helpers
{
public static class ReadConfiguration
    {
        private static IConfigurationRoot Configuration { get; set; }

        static ReadConfiguration()
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Base config
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Override by environment
                .Build();
        }

        // Method to get valid question types
        public static List<string> ReadListFromAppSettings(string key, string defaultValue = "")
        {
            return Configuration.GetSection(key).Get<List<string>>() ?? new List<string>();
        }

        // Method to retrieve a value as string
        public static string ReadStringFromAppSettings(string key, string defaultValue = "")
        {
            return Configuration[key] ?? defaultValue;
        }

        // Method to retrieve a value as Guid (UUID)
        public static Guid ReadGuidFromAppSettings(string key, Guid defaultValue = default(Guid))
        {
            string value = Configuration[key];
            if (Guid.TryParse(value, out Guid result))
            {
                return result;
            }
            return defaultValue;
        }

        // Method to retrieve a value as a generic type
        public static T ReadFromAppSettings<T>(string key, T defaultValue = default)
        {
            var value = Configuration[key];
            if (value != null)
            {
                try
                {
                    // Attempt to convert the value to the target type
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    // Conversion failed, return default value
                    return defaultValue;
                }
            }
            return defaultValue;
        }
    }

}
