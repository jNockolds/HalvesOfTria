using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Xna.Framework;

namespace Halves_of_Tria.Config
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
            Config.GravitationalAcceleration = newValue;

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
        public static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JSON", "Config.json");

        public static Vector2 GravitationalAcceleration { get; set; }
        public static string TestProperty { get; set; }
    }
}