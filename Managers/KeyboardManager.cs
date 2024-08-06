using InputRecorder.Services.Keyboard;

namespace InputRecorder.Managers
{
    /// <summary>
    /// Manages keyboard recording and playback services.
    /// </summary>
    internal class KeyboardManager
    {
        private KeyboardRecordingService _keyboardRecordingService;
        private KeyboardPlaybackService _keyboardPlaybackService;

        public KeyboardManager()
        {
            _keyboardRecordingService = new KeyboardRecordingService();
            _keyboardPlaybackService = new KeyboardPlaybackService();
        }

        /// <summary>
        /// Starts recording keyboard actions.
        /// </summary>
        public void StartRecording()
        {
            _keyboardRecordingService.StartRecording();
        }

        /// <summary>
        /// Stops recording keyboard actions.
        /// </summary>
        public void StopRecording()
        {
            _keyboardRecordingService.StopRecording();
        }

        /// <summary>
        /// Records a keyboard action.
        /// </summary>
        /// <param name="key">The key involved in the action.</param>
        /// <param name="isKeyDown">Indicates whether the key is pressed down.</param>
        public void RecordAction(Keys key, bool isKeyDown)
        {
            _keyboardRecordingService.RecordAction(key, isKeyDown);
        }

        /// <summary>
        /// Plays back recorded keyboard actions asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PlayActionsAsync()
        {
            await _keyboardPlaybackService.PlayActionsAsync();
        }
    }
}