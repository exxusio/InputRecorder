namespace InputRecorder.Managers.Interfaces
{
    /// <summary>
    /// Defines the contract for managing recording and playback of input actions.
    /// </summary>
    internal interface IInputManager
    {
        /// <summary>
        /// Starts recording input actions.
        /// </summary>
        void StartRecording();

        /// <summary>
        /// Stops recording input actions.
        /// </summary>
        void StopRecording();

        /// <summary>
        /// Plays back recorded input actions asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PlayActionsAsync();
    }
}
