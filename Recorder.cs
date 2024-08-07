using InputRecorder.Managers.Implementations;
using InputRecorder.Models.Mouse;
using InputRecorder.Utilities;
using InputRecorder.Hooks;

using KeyEventArgs = InputRecorder.Models.Keyboard.KeyEventArgs;
using MouseEventArgs = InputRecorder.Models.Mouse.MouseEventArgs;

namespace InputRecorder
{
    /// <summary>
    /// Provides functionality for recording and playing back keyboard and mouse actions.
    /// Manages hooks for capturing keyboard and mouse events, and handles recording and playback.
    /// </summary>
    public class Recorder
    {
        private KeyboardManager _keyboardManager;
        private MouseManager _mouseManager;

        private KeyboardHook _keyboardHook;
        private MouseHook _mouseHook;

        private HashSet<Keys> _keysToRecord;
        private Keys[] _bindKeys;

        /// <summary>
        /// Gets a value indicating whether the recorder is currently playing back actions.
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the recorder is currently recording actions.
        /// </summary>
        public bool IsRecording { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse movement is being recorded.
        /// </summary>
        public bool IsMouseMove { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether mouse actions are being tracked.
        /// </summary>
        public bool IsMouseTracking { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether keyboard actions are being tracked.
        /// </summary>
        public bool IsKeyboardTracking { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Recorder"/> class.
        /// </summary>
        public Recorder()
        {
            IsPlaying = false;
            IsRecording = false;

            IsKeyboardTracking = true;
            IsMouseTracking = true;
            IsMouseMove = true;

            _keyboardManager = new KeyboardManager();
            _mouseManager = new MouseManager();

            _keyboardHook = new KeyboardHook();
            _mouseHook = new MouseHook();

            _keysToRecord = new HashSet<Keys>();
            _bindKeys = new Keys[3];
        }

        /// <summary>
        /// Starts recording keyboard and mouse actions. Throws an exception if no keys are specified for recording or binding.
        /// </summary>
        public void StartRecorder()
        {
            if (_bindKeys.All(key => key == Keys.None) || (IsKeyboardTracking && _keysToRecord.Count == 0))
                throw new ArgumentNullException("No keys are specified for recording or binding.");

            _keyboardHook.KeyPressed += KeyTriggered;
            _mouseHook.MouseActionOccurred += MouseTriggered;

            NotificationHelper.ShowNotification("Recorder start");
        }

        /// <summary>
        /// Stops recording keyboard and mouse actions.
        /// </summary>
        public void StopRecorder()
        {
            _keyboardHook.KeyPressed -= KeyTriggered;
            _mouseHook.MouseActionOccurred -= MouseTriggered;

            NotificationHelper.ShowNotification("Recorder stop");
        }

        /// <summary>
        /// Sets the file path for recording and playback actions.
        /// </summary>
        /// <param name="actionPatch">The file path for recording and playback actions.</param>
        public void SetActionPatch(string actionPatch)
        {
            JsonProcessor.SetActionPatch(actionPatch);
            JsonProcessor.SetActionPatch(actionPatch);
        }

        /// <summary>
        /// Sets the file paths for recording and playback actions.
        /// </summary>
        /// <param name="recordingPatch">The file path for recording actions.</param>
        /// <param name="playbackPatch">The file path for playback actions.</param>
        public void SetActionPatch(string recordingPatch, string playbackPatch)
        {
            JsonProcessor.SetActionPatch(recordingPatch, playbackPatch);
            JsonProcessor.SetActionPatch(recordingPatch, playbackPatch);
        }

        /// <summary>
        /// Adds a key to the list of keys to be recorded. Throws an exception if the key is already in the list.
        /// </summary>
        /// <param name="key">The key to add to the recording list.</param>
        public void AddKeyToRecord(Keys key)
        {
            if (_keysToRecord.Contains(key))
                throw new InvalidOperationException("The key is already in the list.");
            if (_bindKeys.Contains(key))
                throw new InvalidOperationException("The key is in the list of bindings.");

            _keysToRecord.Add(key);
        }

        /// <summary>
        /// Adds a range of keys to the list of keys to be recorded.
        /// </summary>
        /// <param name="keys">The list of keys to add to the recording list.</param>
        public void AddRangeKeyToRecord(List<Keys> keys)
        {
            foreach (var key in keys)
            {
                AddKeyToRecord(key);
            }
        }

        /// <summary>
        /// Adds all keys to the list of keys to be recorded.
        /// </summary>
        public void AddAllKeysToRecord()
        {
            List<Keys> uniqueKeys = new List<Keys>();

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                uniqueKeys.Add(key);

            foreach (Keys key in uniqueKeys.Distinct().ToList())
                if (!_bindKeys.Contains(key))
                    AddKeyToRecord(key);
        }

        /// <summary>
        /// Sets the binding keys for starting, stopping recording, and playing actions.
        /// </summary>
        /// <param name="start">The key to start recording.</param>
        /// <param name="end">The key to stop recording.</param>
        /// <param name="record">The key to play recorded actions.</param>
        public void SwitchBindKey(Keys start, Keys end, Keys record)
        {
            if (_keysToRecord.Contains(start) || _keysToRecord.Contains(end) || _keysToRecord.Contains(record))
                throw new InvalidOperationException("The key is in the list of recording.");

            _bindKeys[0] = start;
            _bindKeys[1] = end;
            _bindKeys[2] = record;
        }

        /// <summary>
        /// Handles the keyboard hook event. Starts or stops recording or plays actions based on the pressed key.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments containing the key information.</param>
        private async void KeyTriggered(object? sender, KeyEventArgs e)
        {
            if (!IsRecording && !IsPlaying && e.KeyCode == _bindKeys[0])
            {
                StartRecording();
            }
            else if (IsRecording && !IsPlaying && e.KeyCode == _bindKeys[1])
            {
                StopRecording();
            }
            else if (!IsRecording && !IsPlaying && e.KeyCode == _bindKeys[2])
            {
                await PlayActionsAsync();
            }
            else if (IsRecording && IsKeyboardTracking && _keysToRecord.Contains(e.KeyCode))
            {
                RecordKeyAction(e.KeyCode, e.IsKeyDown);
            }
        }

        /// <summary>
        /// Handles the mouse hook event and records mouse actions if recording is enabled.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments containing the mouse event information.</param>
        private void MouseTriggered(object? sender, MouseEventArgs e)
        {
            if (IsRecording && IsMouseTracking)
            {
                RecordMouseAction(e.EventType, e.Position, e.IsDown);
            }
        }

        /// <summary>
        /// Starts recording keyboard and mouse actions and shows a notification.
        /// </summary>
        private void StartRecording()
        {
            IsRecording = true;

            _keyboardManager.StartRecording();
            _mouseManager.StartRecording();

            NotificationHelper.ShowNotification("Recording started...");
        }

        /// <summary>
        /// Stops recording keyboard and mouse actions and shows a notification.
        /// </summary>
        private void StopRecording()
        {
            IsRecording = false;

            _keyboardManager.StopRecording();
            _mouseManager.StopRecording();

            NotificationHelper.ShowNotification("Recording stopped and saved.");
        }

        /// <summary>
        /// Plays back the recorded actions and shows notifications before and after playback.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task PlayActionsAsync()
        {
            NotificationHelper.ShowNotification("Playing actions...");
            IsPlaying = true;

            var tasks = new List<Task>();
            if (IsKeyboardTracking)
                tasks.Add(_keyboardManager.PlayActionsAsync());
            if (IsMouseTracking)
                tasks.Add(_mouseManager.PlayActionsAsync());
            await Task.WhenAll(tasks);

            IsPlaying = false;
            NotificationHelper.ShowNotification("Playing actions completed!");
        }

        /// <summary>
        /// Records a keyboard action.
        /// </summary>
        /// <param name="key">The key involved in the action.</param>
        /// <param name="isKeyDown">A value indicating whether the key is pressed down.</param>
        private void RecordKeyAction(Keys key, bool isKeyDown)
        {
            _keyboardManager.RecordAction(key, isKeyDown);
        }

        /// <summary>
        /// Records a mouse action, if mouse movement recording is enabled or the event is not a mouse movement.
        /// </summary>
        /// <param name="eventType">The type of mouse event (e.g., click, move).</param>
        /// <param name="position">The position of the mouse.</param>
        /// <param name="isDown">A value indicating whether the mouse button is pressed down.</param>
        private void RecordMouseAction(MouseEventType eventType, Point position, bool isDown)
        {
            if (IsMouseMove || eventType != MouseEventType.Move)
                _mouseManager.RecordAction(eventType, position, isDown);
        }
    }
}