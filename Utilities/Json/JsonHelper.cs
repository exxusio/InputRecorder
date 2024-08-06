using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InputRecorder.Utilities.Json
{
    /// <summary>
    /// Provides utility methods for handling JSON data, including serialization, validation, and conversion.
    /// </summary>
    internal static class JsonHelper
    {
        /// <summary>
        /// Serializes an object to a JSON string with indentation.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <returns>A JSON string representing the object.</returns>
        public static string SerializeToJson(object data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        /// <summary>
        /// Checks if the provided JSON string is null or empty.
        /// </summary>
        /// <param name="jsonData">The JSON string to check.</param>
        /// <returns>True if the JSON string is null or empty; otherwise, false.</returns>
        public static bool ParseJson(string jsonData)
        {
            return string.IsNullOrWhiteSpace(jsonData);
        }

        /// <summary>
        /// Checks if a specified property in a JObject is null or contains null or empty data.
        /// </summary>
        /// <param name="recordData">The JObject containing the property.</param>
        /// <param name="name">The name of the property to check.</param>
        /// <returns>True if the property is null, does not exist, or contains null/empty data; otherwise, false.</returns>
        public static bool IsNullData(JObject recordData, string name)
        {
            if (recordData == null || string.IsNullOrWhiteSpace(name) || !recordData.ContainsKey(name))
            {
                return true;
            }

            var token = recordData[name];
            if (token == null)
            {
                return true;
            }

            return token.Type switch
            {
                JTokenType.Array when ((JArray)token).Count == 0 => true,
                JTokenType.Float or JTokenType.Integer when Convert.ToDouble(token) == 0 => true,
                JTokenType.String when string.IsNullOrWhiteSpace((string?)token) => true,
                _ => false
            };
        }

        /// <summary>
        /// Converts a JToken to a JArray.
        /// </summary>
        /// <param name="token">The JToken to convert.</param>
        /// <returns>A JArray representing the token.</returns>
        public static JArray ToJArray(JToken? token)
        {
            return token as JArray ?? new JArray();
        }

        /// <summary>
        /// Converts a JToken to a TimeSpan.
        /// </summary>
        /// <param name="token">The JToken to convert.</param>
        /// <returns>A TimeSpan representing the token.</returns>
        public static TimeSpan JTokenToTimeSpan(JToken? token)
        {
            return TimeSpan.FromMilliseconds(Convert.ToDouble(token));
        }
    }
}