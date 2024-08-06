using InputRecorder.Models.Mouse;
using InputRecorder.Services.Mouse;

namespace InputRecorder.Managers
{
    /// <summary>
    /// Manages mouse recording and playback services.
    /// </summary>
    internal class MouseManager
    {
        private MouseRecordingService _mouseRecordingService;
        private MousePlaybackService _mousePlaybackService;

        public MouseManager()
        {
            _mouseRecordingService = new MouseRecordingService();
            _mousePlaybackService = new MousePlaybackService();
        }

        /// <summary>
        /// Starts recording mouse actions.
        /// </summary>
        public void StartRecording()
        {
            _mouseRecordingService.StartRecording();
        }

        /// <summary>
        /// Stops recording mouse actions.
        /// </summary>
        public void StopRecording()
        {
            _mouseRecordingService.StopRecording();
        }

        /// <summary>
        /// Records a mouse action.
        /// </summary>
        /// <param name="eventType">The type of mouse event.</param>
        /// <param name="position">The position of the mouse.</param>
        /// <param name="isDown">Indicates whether the mouse button is pressed down.</param>
        public void RecordAction(MouseEventType eventType, Point position, bool isDown)
        {
            _mouseRecordingService.RecordAction(eventType, position, isDown);
        }

        /// <summary>
        /// Plays back recorded mouse actions asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task PlayActionsAsync()
        {
            await _mousePlaybackService.PlayActionsAsync();
        }
    }
}