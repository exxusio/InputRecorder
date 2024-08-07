using InputRecorder.Utilities;

namespace InputRecorder.Configs
{
    /// <summary>
    /// Configures the settings for the Recorder instance, including file paths, key bindings, and recording options.
    /// </summary>
    internal class RecorderConfigurator : IRecorderConfigurator
    {
        private readonly Recorder _recorder;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecorderConfigurator"/> class.
        /// </summary>
        /// <param name="recorder">The <see cref="Recorder"/> instance to configure.</param>
        internal RecorderConfigurator(Recorder recorder)
        {
            _recorder = recorder;
        }

        /// <summary>
        /// Creates and configures a new instance of the <see cref="Recorder"/> class.
        /// </summary>
        /// <param name="configure">An action to configure the <see cref="IRecorderConfigurator"/>.</param>
        /// <returns>A fully configured <see cref="Recorder"/> instance.</returns>
        public static Recorder Config(Action<IRecorderConfigurator> configure)
        {
            var recorder = new Recorder();
            var configurator = new RecorderConfigurator(recorder);
            configure(configurator);
            return recorder;
        }

        /// <summary>
        /// Sets the file path for both recording and playback actions.
        /// </summary>
        /// <param name="actionPatch">The file path for recording and playback actions.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        public IRecorderConfigurator SetActionPatch(string actionPatch)
        {
            JsonProcessor.SetActionPatch(actionPatch);
            return this;
        }

        /// <summary>
        /// Sets the file paths for recording and playback actions separately.
        /// </summary>
        /// <param name="recordingPatch">The file path for recording actions.</param>
        /// <param name="playbackPatch">The file path for playback actions.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        public IRecorderConfigurator SetActionPatch(string recordingPatch, string playbackPatch)
        {
            JsonProcessor.SetActionPatch(recordingPatch, playbackPatch);
            return this;
        }

        /// <summary>
        /// Sets the binding keys for starting, stopping recording, and playing actions.
        /// </summary>
        /// <param name="start">The key to start recording.</param>
        /// <param name="end">The key to stop recording.</param>
        /// <param name="record">The key to play recorded actions.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if any of the provided keys are already in the list of keys to be recorded.</exception>
        public IRecorderConfigurator SwitchBindKey(Keys start, Keys end, Keys record)
        {
            if (_recorder._keysToRecord.Contains(start) || _recorder._keysToRecord.Contains(end) || _recorder._keysToRecord.Contains(record))
                throw new InvalidOperationException("The key is in the list of recording.");

            _recorder._bindKeys[0] = start;
            _recorder._bindKeys[1] = end;
            _recorder._bindKeys[2] = record;
            return this;
        }

        /// <summary>
        /// Adds a range of keys to the list of keys to be recorded.
        /// </summary>
        /// <param name="keys">The list of keys to add to the recording list.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        public IRecorderConfigurator AddRangeKeyToRecord(List<Keys> keys)
        {
            _recorder._keysToRecord.Clear();

            foreach (var key in keys)
            {
                _recorder.AddKeyToRecord(key);
            }
            return this;
        }

        /// <summary>
        /// Adds all available keys to the list of keys to be recorded, excluding the bind keys.
        /// </summary>
        public void AddAllKeysToRecord()
        {
            List<Keys> uniqueKeys = new List<Keys>();

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
                uniqueKeys.Add(key);

            _recorder._keysToRecord.Clear();

            foreach (Keys key in uniqueKeys.Distinct().ToList())
                if (!_recorder._bindKeys.Contains(key))
                    _recorder.AddKeyToRecord(key);
        }

        /// <summary>
        /// Configures whether mouse movements should be recorded.
        /// </summary>
        /// <param name="isMouseMove">True to enable mouse movement recording; false otherwise.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        public IRecorderConfigurator IsMouseMove(bool isMouseMove)
        {
            _recorder._isMouseMove = isMouseMove;
            return this;
        }

        /// <summary>
        /// Configures whether mouse tracking should be enabled.
        /// </summary>
        /// <param name="isMouseTracking">True to enable mouse tracking; false otherwise.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        public IRecorderConfigurator IsMouseTracking(bool isMouseTracking)
        {
            _recorder._isMouseTracking = isMouseTracking;
            return this;
        }

        /// <summary>
        /// Configures whether keyboard tracking should be enabled.
        /// </summary>
        /// <param name="isKeyboardTracking">True to enable keyboard tracking; false otherwise.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        public IRecorderConfigurator IsKeyboardTracking(bool isKeyboardTracking)
        {
            _recorder._isKeyboardTracking = isKeyboardTracking;
            return this;
        }
    }
}