using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Halves_of_Tria.Configuration
{
    internal static class JsonLoader
    {

        public static void Initialize()
        {
            LoadConfig();
        }

        /// <summary>
        /// Loads the configuration from the JSON config file and assigns values to <see cref="Config"/>'s properties.
        /// </summary>
        /// <remarks>When adding new properties to the config, make sure to update this method accordingly.</remarks>
        public static void LoadConfig()
        {
            string jsonString = File.ReadAllText(Config.filePath);

            ConfigJson coinfigJson = JsonSerializer.Deserialize<ConfigJson>(jsonString);


            // Assign values to Config properties:

            // Convert the dictionary to Vector2
            Dictionary<string, float> gravitationalAccelerationDict = coinfigJson.GravitationalAcceleration;
            Config.GravitationalAcceleration = new Vector2(
                gravitationalAccelerationDict["X"],
                gravitationalAccelerationDict["Y"]
            );

            Config.TestProperty = coinfigJson.TestProperty;
        }

        /// <summary>
        /// Sets a value in the JSON config file for the specified property name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        public static void SetJsonValue<T>(string propertyName, T newValue)
        {
            string jsonString = File.ReadAllText(Config.filePath);
            var jsonNode = JsonNode.Parse(jsonString);
            jsonNode[propertyName] = JsonValue.Create(newValue);
            File.WriteAllText(
                Config.filePath,
                JsonSerializer.Serialize(jsonNode, new JsonSerializerOptions { WriteIndented = true })
            );
        }

        private struct ConfigJson
        {
            public Dictionary<string, float> GravitationalAcceleration { get; set; }
            public string TestProperty { get; set; }
        }
    }

    /// <summary>
    /// Stores configuration properties loaded from the JSON config file.
    /// </summary>
    /// <remarks>When adding new properties to this class, make sure to update <see cref="JsonLoader.LoadConfig()"/> accordingly.</remarks>
    internal struct Config
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
                JsonLoader.SetJsonValue("GravitationalAcceleration", new Dictionary<string, float>
                {
                    { "X", value.X },
                    { "Y", value.Y }
                }
                );
            }
        }

        private static string _testProperty;
        public static string TestProperty
        {
            get => _testProperty;
            set
            {
                _testProperty = value;
                JsonLoader.SetJsonValue("TestProperty", value);
            }
        }
    }
}