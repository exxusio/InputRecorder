using InputRecorder.Services.Interfaces;
using InputRecorder.Models.Mouse;
using InputRecorder.Utilities;
using WindowsInput;

namespace InputRecorder.Services.Implementations.Mouse
{
    /// <summary>
    /// Handles playback of recorded mouse actions from a specified file.
    /// </summary>
    internal class MousePlaybackService : IPlaybackService
    {
        private float _inaccuracyX = 34.2f;
        private float _inaccuracyY = 60.8f;

        /// <summary>
        /// Plays back mouse actions from the specified file asynchronously.
        /// </summary>
        public async Task PlayActionsAsync()
        {
            var mouseActions = JsonProcessor.ReadMouseActions();

            var startTime = DateTime.Now;

            var tasks = new List<Task>();
            foreach (var action in mouseActions)
            {
                tasks.Add(HandleAction(action, startTime));
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Handles a single mouse action asynchronously.
        /// </summary>
        /// <param name="action">The mouse action to handle.</param>
        /// <param name="startTime">The time when playback started.</param>
        private async Task HandleAction(MouseAction action, DateTime startTime)
        {
            var currentTime = DateTime.Now - startTime;
            var delay = action.StartTime - currentTime;

            await Task.Delay(delay);

            var actionDuration = action.EndTime - delay;

            switch (action.EventType)
            {
                case MouseEventType.ClickLeft:
                    HandleActionMove(action.Position.X * _inaccuracyX, action.Position.Y * _inaccuracyY);

                    await HandleActionLB(actionDuration);
                    break;

                case MouseEventType.ClickRight:
                    HandleActionMove(action.Position.X * _inaccuracyX, action.Position.Y * _inaccuracyY);

                    await HandleActionRB(actionDuration);
                    break;

                case MouseEventType.Move:
                    HandleActionMove(action.Position.X * _inaccuracyX, action.Position.Y * _inaccuracyY);
                    break;
            }
        }

        /// <summary>
        /// Moves the mouse to a specific position.
        /// </summary>
        /// <param name="x">The X coordinate.</param>
        /// <param name="y">The Y coordinate.</param>
        private void HandleActionMove(double x, double y)
        {
            var sim = new InputSimulator();
            sim.Mouse.MoveMouseTo(x, y);
        }

        /// <summary>
        /// Simulates a left mouse button click with a delay.
        /// </summary>
        /// <param name="delay">The delay before releasing the button.</param>
        private async Task HandleActionLB(TimeSpan delay)
        {
            var sim = new InputSimulator();

            sim.Mouse.LeftButtonDown();
            await Task.Delay(delay);
            sim.Mouse.LeftButtonUp();
        }

        /// <summary>
        /// Simulates a right mouse button click with a delay.
        /// </summary>
        /// <param name="delay">The delay before releasing the button.</param>
        private async Task HandleActionRB(TimeSpan delay)
        {
            var sim = new InputSimulator();

            sim.Mouse.RightButtonDown();
            await Task.Delay(delay);
            sim.Mouse.RightButtonUp();
        }
    }
}