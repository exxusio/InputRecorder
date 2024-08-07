using InputRecorder.Managers.Implementations;
using InputRecorder.Models.Mouse;
using InputRecorder.Utilities;
using InputRecorder.Configs;
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

        internal HashSet<Keys> _keysToRecord;
        internal Keys[] _bindKeys;
        internal bool _isMouseMove;
        internal bool _isMouseTracking;
        internal bool _isKeyboardTracking;

        /// <summary>
        /// Gets a value indicating whether the recorder is currently playing back actions.
        /// </summary>
        public bool IsPlaying { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the recorder is currently recording actions.
        /// </summary>
        public bool IsRecording { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Recorder"/> class.
        /// </summary>
        internal Recorder()
        {
            IsPlaying = false;
            IsRecording = false;

            _isKeyboardTracking = true;
            _isMouseTracking = true;
            _isMouseMove = true;

            _keyboardManager = new KeyboardManager();
            _mouseManager = new MouseManager();

            _keyboardHook = new KeyboardHook();
            _mouseHook = new MouseHook();

            _keysToRecord = new HashSet<Keys>();
            _bindKeys = new Keys[3];
        }

        /// <summary>
        /// Creates and configures a new instance of the <see cref="Recorder"/> class.
        /// </summary>
        /// <param name="configure">An action to configure the <see cref="RecorderConfigurator"/>.</param>
        /// <returns>A fully configured <see cref="Recorder"/> instance.</returns>
        public static Recorder Config(Action<IRecorderConfigurator> configure)
        {
            var recorder = new Recorder();
            var configurator = new RecorderConfigurator(recorder);
            configure(configurator);
            return recorder;
        }

        /// <summary>
        /// Starts recording keyboard and mouse actions. Throws an exception if no keys are specified for recording or binding.
        /// </summary>
        public void StartRecorder()
        {
            if (_bindKeys.All(key => key == Keys.None) || (_isKeyboardTracking && _keysToRecord.Count == 0))
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
        /// Adds a key to the list of keys to be recorded. Throws an exception if the key is already in the list.
        /// </summary>
        /// <param name="key">The key to add to the recording list.</param>
        internal void AddKeyToRecord(Keys key)
        {
            if (_keysToRecord.Contains(key))
                throw new InvalidOperationException("The key is already in the list.");
            if (_bindKeys.Contains(key))
                throw new InvalidOperationException("The key is in the list of bindings.");

            _keysToRecord.Add(key);
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
            else if (IsRecording && _isKeyboardTracking && _keysToRecord.Contains(e.KeyCode))
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
            if (IsRecording && _isMouseTracking)
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
            if (_isKeyboardTracking)
                tasks.Add(_keyboardManager.PlayActionsAsync());
            if (_isMouseTracking)
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
            if (_isMouseMove || eventType != MouseEventType.Move)
                _mouseManager.RecordAction(eventType, position, isDown);
        }
    }
}