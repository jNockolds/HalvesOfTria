using System;
using System.IO;
using System.Text.Json;

namespace Halves_of_Tria.Config
{
    internal static class JsonLoader
    {
        public static void LoadConfig()
        {
            string jsonString = File.ReadAllText(Config.filePath);

            ConfigJson physicsConfig = JsonSerializer.Deserialize<ConfigJson>(jsonString);

            Config.GravitationalAcceleration = physicsConfig.GravitationalAcceleration;
            Config.TestProperty = physicsConfig.TestProperty;
        }

        private class ConfigJson
        {
            public float GravitationalAcceleration { get; set; }
            public string TestProperty { get; set; }
        }
    }

    internal static class Config
    {
        public static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JSON", "Config.json");

        public static float GravitationalAcceleration { get; set; }
        public static string TestProperty { get; set; }
    }
}