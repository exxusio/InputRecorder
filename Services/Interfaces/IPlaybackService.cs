namespace InputRecorder.Services.Interfaces
{
    /// <summary>
    /// Defines the contract for playback services of recorded input actions.
    /// </summary>
    internal interface IPlaybackService
    {
        /// <summary>
        /// Plays back recorded input actions asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task PlayActionsAsync();
    }
}
