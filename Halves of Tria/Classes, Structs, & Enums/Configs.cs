using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Halves_of_Tria.Configuration
{
    // [TODO: use MonoGame.Extended's JSON serializer instead of System.Text.Json, since it has better support for MonoGame types like Vector2]

    // (*): Guide to adding a new config property:
    // 1. Add a new property to the JSON config file, with a unique name and appropriate type.
    //
    // 2. Add a matching property to the Config struct at the bottom of the page,
    //    with a getter and setter that calls JsonLoader.SetJsonValue() in the setter.
    //
    //     -  Note that it can be a different type than the property in (1) if necessary,
    //        as long as you handle the conversion the other way in step (4).
    //
    //        For example, since JSON doesn't support Vector2, you can represent it as a
    //        dictionary with "X" and "Y" keys in the JSON file, and then convert it from
    //        Vector2 to that dictionary format in the Config property setter.
    //
    // 3. Add a matching property to the ConfigJson struct in JsonLoader,
    //    with the same name and type as the property in (2).
    //
    // 4. Update the LoadConfig() method in JsonLoader to assign the value from the
    //    ConfigJson property to the Config property. Include any necessary conversions
    //    if the type is unavailable in JSON (e.g. Vector2).
    //
    // [TODO: Make this dynamic and scalable, so someone can add new properties easier.
    //  Probably use reflection** to loop through properties in Config and assign values from
    //  the JSON config file, instead of hardcoding each property assignment manually.
    // **https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/reflection-and-attributes/]
    internal static class JsonLoader
    {

        public static void Initialize()
        {
            LoadConfig();
        }

        /// <summary>
        /// Loads the configuration from the JSON config file and assigns values to <see cref="Config"/>'s properties.
        /// </summary>
        /// <remarks>When adding new properties to the config, make sure to Update this method accordingly.</remarks>
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

            Config.DefaultLinearDragCoefficient = coinfigJson.DefaultLinearDragCoefficient;

            Config.TestProperty = coinfigJson.TestProperty;

            // [Add new properties here when needed, following (*).]
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
            public float DefaultLinearDragCoefficient { get; set; }
            public string TestProperty { get; set; }

            // [Add new properties here when needed, following (*).]
        }
    }

    /// <summary>
    /// Stores configuration properties loaded from the JSON config file.
    /// </summary>
    /// <remarks>When adding new properties to this class, make sure to Update <see cref="JsonLoader.LoadConfig()"/> accordingly.</remarks>
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

        private static float _defaultLinearDragCoefficient;
        public static float DefaultLinearDragCoefficient
        {
            get => _defaultLinearDragCoefficient;
            set
            {
                _defaultLinearDragCoefficient = value;
                JsonLoader.SetJsonValue("DefaultLinearDragCoefficient", value);
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

        // [Add new properties here when needed, following (*).]
    }
}