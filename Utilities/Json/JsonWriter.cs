using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InputRecorder.Utilities.Json
{
    /// <summary>
    /// Provides methods for writing and updating JSON files.
    /// </summary>
    internal static class JsonWriter
    {
        /// <summary>
        /// Updates a JSON file with new data. Existing data is preserved, and new data is merged.
        /// </summary>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <param name="updatedData">The data to update or merge into the file.</param>
        public static void UpdateFile(string filePath, object updatedData)
        {
            PathValidator.ValidatePath(filePath);

            JObject existingData;

            if (File.Exists(filePath))
            {
                var existingJson = File.ReadAllText(filePath);
                existingData = string.IsNullOrWhiteSpace(existingJson)
                    ? new JObject()
                    : JObject.Parse(existingJson);
            }
            else
            {
                existingData = new JObject();
            }

            var updatedJson = JToken.Parse(JsonHelper.SerializeToJson(updatedData)) as JObject;

            if (updatedJson != null)
            {
                foreach (var property in updatedJson.Properties())
                {
                    existingData[property.Name] = property.Value;
                }
            }

            File.WriteAllText(filePath, existingData.ToString(Formatting.Indented));
        }
    }
}
