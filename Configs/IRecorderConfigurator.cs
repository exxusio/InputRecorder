namespace InputRecorder.Configs
{
    /// <summary>
    /// Interface for configuring a <see cref="Recorder"/> instance.
    /// Provides methods to set file paths, key bindings, and recording options.
    /// </summary>
    public interface IRecorderConfigurator
    {
        /// <summary>
        /// Sets the file path for both recording and playback actions.
        /// </summary>
        /// <param name="actionPatch">The file path for recording and playback actions.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        IRecorderConfigurator SetActionPatch(string actionPatch);

        /// <summary>
        /// Sets the file paths for recording and playback actions separately.
        /// </summary>
        /// <param name="recordingPatch">The file path for recording actions.</param>
        /// <param name="playbackPatch">The file path for playback actions.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        IRecorderConfigurator SetActionPatch(string recordingPatch, string playbackPatch);

        /// <summary>
        /// Sets the binding keys for starting, stopping recording, and playing actions.
        /// </summary>
        /// <param name="start">The key to start recording.</param>
        /// <param name="end">The key to stop recording.</param>
        /// <param name="record">The key to play recorded actions.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        /// <exception cref="InvalidOperationException">Thrown if any of the provided keys are already in the list of keys to be recorded.</exception>
        IRecorderConfigurator SwitchBindKey(Keys start, Keys end, Keys record);

        /// <summary>
        /// Adds a range of keys to the list of keys to be recorded.
        /// </summary>
        /// <param name="keys">The list of keys to add to the recording list.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        IRecorderConfigurator AddRangeKeyToRecord(List<Keys> keys);

        /// <summary>
        /// Adds all available keys to the list of keys to be recorded, excluding the bind keys.
        /// </summary>
        void AddAllKeysToRecord();

        /// <summary>
        /// Configures whether mouse movements should be recorded.
        /// </summary>
        /// <param name="isMouseMove">True to enable mouse movement recording; false otherwise.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        IRecorderConfigurator IsMouseMove(bool isMouseMove);

        /// <summary>
        /// Configures whether mouse tracking should be enabled.
        /// </summary>
        /// <param name="isMouseTracking">True to enable mouse tracking; false otherwise.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        IRecorderConfigurator IsMouseTracking(bool isMouseTracking);

        /// <summary>
        /// Configures whether keyboard tracking should be enabled.
        /// </summary>
        /// <param name="isKeyboardTracking">True to enable keyboard tracking; false otherwise.</param>
        /// <returns>The <see cref="IRecorderConfigurator"/> instance for method chaining.</returns>
        IRecorderConfigurator IsKeyboardTracking(bool isKeyboardTracking);
    }
}