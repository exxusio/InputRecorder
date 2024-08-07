namespace InputRecorder.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for recording services of input actions.
    /// </summary>
    internal interface IRecordingService
    {
        /// <summary>
        /// Starts the recording of input actions.
        /// </summary>
        void StartRecording();

        /// <summary>
        /// Stops the recording of input actions and saves them to a file.
        /// </summary>
        void StopRecording();
    }
}
