using InputRecorder.Models.Keyboard;
using InputRecorder.Models.Mouse;
using Newtonsoft.Json.Linq;

namespace InputRecorder.Utilities.Json
{
    /// <summary>
    /// Provides methods for reading mouse and keyboard actions from JSON files.
    /// </summary>
    internal static class JsonReader
    {
        /// <summary>
        /// Reads mouse actions from a JSON file.
        /// </summary>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <returns>A list of <see cref="MouseAction"/> instances read from the file.</returns>
        public static List<MouseAction> ReadMouseActions(string filePath)
        {
            PathValidator.ValidatePath(filePath);

            var jsonData = File.ReadAllText(filePath);

            if (JsonHelper.ParseJson(jsonData))
                return new List<MouseAction>();

            var recordData = JObject.Parse(jsonData);

            if (JsonHelper.IsNullData(recordData, "RecordMs") || JsonHelper.IsNullData(recordData, "Mouse"))
                return new List<MouseAction>();

            var mouseDataArray = JsonHelper.ToJArray(recordData["Mouse"]);

            var mouseActions = mouseDataArray
                .Select(action => new MouseAction
                {
                    EventType = Converter.ToEnum<MouseEventType>(action["EventType"]),
                    Position = new Point(
                        (int?)(long?)action["Position"]?["X"] ?? 0,
                        (int?)(long?)action["Position"]?["Y"] ?? 0
                    ),
                    StartTime = JsonHelper.JTokenToTimeSpan(action["StartTime"]),
                    EndTime = JsonHelper.JTokenToTimeSpan(action["EndTime"])
                })
                .OrderBy(a => a.StartTime)
                .ToList();

            return mouseActions;
        }

        /// <summary>
        /// Reads key actions from a JSON file.
        /// </summary>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <returns>A list of <see cref="KeyAction"/> instances read from the file.</returns>
        public static List<KeyAction> ReadKeyActions(string filePath)
        {
            PathValidator.ValidatePath(filePath);

            var jsonData = File.ReadAllText(filePath);

            if (JsonHelper.ParseJson(jsonData))
                return new List<KeyAction>();

            var recordData = JObject.Parse(jsonData);

            if (JsonHelper.IsNullData(recordData, "RecordMs") || JsonHelper.IsNullData(recordData, "Keys"))
                return new List<KeyAction>();

            var keyDataArray = JsonHelper.ToJArray(recordData["Keys"]);

            var keyActions = keyDataArray
                .Select(action => new KeyAction
                {
                    Key = Converter.ToEnum<Keys>(action["Key"]),
                    StartTime = JsonHelper.JTokenToTimeSpan(action["StartTime"]),
                    EndTime = JsonHelper.JTokenToTimeSpan(action["EndTime"])
                })
                .OrderBy(a => a.StartTime)
                .ToList();

            return keyActions;
        }
    }
}