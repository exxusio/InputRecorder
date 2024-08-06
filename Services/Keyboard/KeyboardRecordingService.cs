using InputRecorder.Models.Keyboard;
using InputRecorder.Utilities;

namespace InputRecorder.Services.Keyboard
{
    /// <summary>
    /// Provides functionality to record keyboard actions and save them to a file.
    /// </summary>
    internal class KeyboardRecordingService
    {
        private List<KeyAction> _keyActions = new List<KeyAction>();
        private DateTime _recordingStartTime;

        /// <summary>
        /// Starts recording keyboard actions by initializing necessary variables.
        /// </summary>
        public void StartRecording()
        {
            _keyActions.Clear();
            _recordingStartTime = DateTime.Now;
        }

        /// <summary>
        /// Stops recording and saves the recorded actions to a file.
        /// </summary>
        public void StopRecording()
        {
            SaveActionsToFile();
        }

        /// <summary>
        /// Records a keyboard action based on whether the key is pressed or released.
        /// </summary>
        /// <param name="key">The key involved in the action.</param>
        /// <param name="isKeyDown">Indicates whether the key is pressed down.</param>
        public void RecordAction(Keys key, bool isKeyDown)
        {
            var now = DateTime.Now - _recordingStartTime;
            var lastKeyAction = GetLastAction(key);

            if (isKeyDown)
            {
                HandleDown(key, now, lastKeyAction);
            }
            else
            {
                HandleUp(now, lastKeyAction);
            }
        }

        /// <summary>
        /// Handles the logic when a key is pressed down.
        /// </summary>
        /// <param name="key">The key being pressed.</param>
        /// <param name="now">The current time relative to the recording start time.</param>
        /// <param name="lastKeyAction">The last recorded action for this key, if any.</param>
        private void HandleDown(Keys key, TimeSpan now, KeyAction? lastKeyAction)
        {
            if (lastKeyAction == null)
            {
                AddNewAction(key, now);
            }
            else if (lastKeyAction.StartTime == TimeSpan.Zero)
            {
                lastKeyAction.StartTime = now;
            }
            else if (lastKeyAction.EndTime != TimeSpan.Zero)
            {
                AddNewAction(key, now);
            }
        }

        /// <summary>
        /// Handles the logic when a key is released.
        /// </summary>
        /// <param name="now">The current time relative to the recording start time.</param>
        /// <param name="lastKeyAction">The last recorded action for this key, if any.</param>
        private void HandleUp(TimeSpan now, KeyAction? lastKeyAction)
        {
            if (lastKeyAction != null && lastKeyAction.EndTime == TimeSpan.Zero)
            {
                lastKeyAction.EndTime = now;
            }
        }

        /// <summary>
        /// Adds a new key action with the specified key and current time.
        /// </summary>
        /// <param name="key">The key being recorded.</param>
        /// <param name="now">The current time relative to the recording start time.</param>
        private void AddNewAction(Keys key, TimeSpan now)
        {
            _keyActions.Add(new KeyAction
            {
                Key = key,
                StartTime = now
            });
        }

        /// <summary>
        /// Retrieves the last action recorded for the specified key.
        /// </summary>
        /// <param name="key">The key for which to retrieve the last action.</param>
        /// <returns>The last recorded key action for the specified key, or null if none exists.</returns>
        private KeyAction? GetLastAction(Keys key)
        {
            return _keyActions
                .Where(action => action.Key == key)
                .LastOrDefault();
        }

        /// <summary>
        /// Saves the recorded key actions to a file in JSON format.
        /// </summary>
        private void SaveActionsToFile()
        {
            var recordData = new
            {
                RecordMs = Converter.TimeSpanToDouble(DateTime.Now - _recordingStartTime),
                Keys = _keyActions.Select(action => new
                {
                    Key = action.Key.ToString(),
                    StartTime = Converter.TimeSpanToDouble(action.StartTime),
                    EndTime = Converter.TimeSpanToDouble(action.EndTime)
                })
            };


            JsonProcessor.SaveToFile(recordData);
        }
    }
}
