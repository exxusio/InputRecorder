using InputRecorder.Services.Interfaces;
using InputRecorder.Models.Keyboard;
using InputRecorder.Utilities;
using WindowsInput.Native;
using WindowsInput;


namespace InputRecorder.Services.Implementations.Keyboard
{
    /// <summary>
    /// Provides functionality to playback recorded keyboard actions.
    /// </summary>
    internal class KeyboardPlaybackService : IPlaybackService
    {
        /// <summary>
        /// Plays back the recorded keyboard actions asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task PlayActionsAsync()
        {
            var keyActions = JsonProcessor.ReadKeyActions();

            var startTime = DateTime.Now;

            var tasks = new List<Task>();
            foreach (var action in keyActions)
            {
                tasks.Add(HandleAction(action, startTime));
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Handles a key action asynchronously by simulating key press and release.
        /// </summary>
        /// <param name="action">The key action to handle.</param>
        /// <param name="startTime">The time when playback started.</param>
        private async Task HandleAction(KeyAction action, DateTime startTime)
        {
            var currentTime = DateTime.Now - startTime;
            var delay = action.StartTime - currentTime;

            await Task.Delay(delay);

            HandleActionDown((VirtualKeyCode)action.Key);

            var actionDuration = action.EndTime - delay;
            await Task.Delay(actionDuration);

            HandleActionUp((VirtualKeyCode)action.Key);
        }

        /// <summary>
        /// Simulates a key press event.
        /// </summary>
        /// <param name="keyCode">The virtual key code of the key to press.</param>
        private void HandleActionDown(VirtualKeyCode keyCode)
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(keyCode);
        }

        /// <summary>
        /// Simulates a key release event.
        /// </summary>
        /// <param name="keyCode">The virtual key code of the key to release.</param>
        private void HandleActionUp(VirtualKeyCode keyCode)
        {
            var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyUp(keyCode);
        }
    }
}
