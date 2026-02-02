using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Halves_of_Tria.Configuration
{
    internal static class JsonLoader
    {
        public static void LoadConfig()
        {
            string jsonString = File.ReadAllText(Config.filePath);

            ConfigJson physicsConfig = JsonSerializer.Deserialize<ConfigJson>(jsonString);

            Config.GravitationalAcceleration = new Vector2(0, physicsConfig.GravitationalAcceleration);
            Config.TestProperty = physicsConfig.TestProperty;
        }

        public static void SetGravitationalAcceleration(Vector2 newValue)
        {
            string jsonString = File.ReadAllText(Config.filePath);

            var jsonNode = JsonNode.Parse(jsonString);

            jsonNode["GravitationalAcceleration"] = newValue.Y;

            File.WriteAllText(Config.filePath, jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }

        private class ConfigJson
        {
            public float GravitationalAcceleration { get; set; }
            public string TestProperty { get; set; }
        }
    }

    internal static class Config
    {
        // it steps back from bin/Debug/net8.0 to the project root directory using "..", "..", ".."
        public static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "JSON", "Config.json");

        private static Vector2 _gravitationalAcceleration;
        public static Vector2 GravitationalAcceleration
        {
            get => _gravitationalAcceleration;
            set
            {
                _gravitationalAcceleration = value;
                JsonLoader.SetGravitationalAcceleration(value);
            }
        }
        public static string TestProperty { get; set; }
    }
}