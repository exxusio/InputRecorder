using InputRecorder.Models.Keyboard;
using InputRecorder.Utilities.Json;
using InputRecorder.Models.Mouse;

namespace InputRecorder.Utilities
{
    /// <summary>
    /// Provides methods for processing JSON data, including reading and writing.
    /// </summary>
    internal static class JsonProcessor
    {
        private static string _recordingPath = "RecorderActions.json";
        private static string _playbackPath = "RecorderActions.json";

        /// <summary>
        /// Sets the file path for recording and playback actions.
        /// </summary>
        /// <param name="actionPatch">The file path for recording and playback actions.</param>
        public static void SetActionPatch(string actionPatch)
        {
            _recordingPath = actionPatch;
            _playbackPath = actionPatch;
        }

        /// <summary>
        /// Sets the file paths for recording and playback actions.
        /// </summary>
        /// <param name="recordingPatch">The file path for recording actions.</param>
        /// <param name="playbackPatch">The file path for playback actions.</param>
        public static void SetActionPatch(string recordingPatch, string playbackPatch)
        {
            _recordingPath = recordingPatch;
            _playbackPath = playbackPatch;
        }

        /// <summary>
        /// Updates specific properties in an existing JSON file. If the file does not exist, it creates a new one.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="updatedData">The updated data to write.</param>
        public static void SaveToFile(object updatedData)
        {
            JsonWriter.UpdateFile(_recordingPath, updatedData);
        }

        /// <summary>
        /// Reads mouse actions from a JSON file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>A list of mouse actions.</returns>
        public static List<MouseAction> ReadMouseActions()
        {
            return JsonReader.ReadMouseActions(_playbackPath);
        }

        /// <summary>
        /// Reads keyboard actions from a JSON file.
        /// </summary>
        /// <param name="filePath">The path to the file.</param>
        /// <returns>A list of keyboard actions.</returns>
        public static List<KeyAction> ReadKeyActions()
        {
            return JsonReader.ReadKeyActions(_playbackPath);
        }
    }
}
