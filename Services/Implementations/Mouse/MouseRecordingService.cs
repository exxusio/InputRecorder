using InputRecorder.Services.Interfaces;
using InputRecorder.Models.Mouse;
using InputRecorder.Utilities;

namespace InputRecorder.Services.Implementations.Mouse
{
    /// <summary>
    /// Handles recording of mouse actions and saving them to a file.
    /// </summary>
    internal class MouseRecordingService : IRecordingService
    {
        private List<MouseAction> _mouseActions = new List<MouseAction>();
        private DateTime _recordingStartTime;
        private const int _minDistanceForRecording = 5;

        /// <summary>
        /// Starts the recording of mouse actions.
        /// </summary>
        public void StartRecording()
        {
            _mouseActions.Clear();
            _recordingStartTime = DateTime.Now;
        }

        /// <summary>
        /// Stops the recording of mouse actions and saves them to a file.
        /// </summary>
        public void StopRecording()
        {
            SaveActionsToFile();
        }

        /// <summary>
        /// Records a mouse action based on the event type, position, and whether the button is pressed or released.
        /// </summary>
        /// <param name="eventType">The type of mouse event.</param>
        /// <param name="position">The position of the mouse.</param>
        /// <param name="isDown">Indicates if the mouse button is pressed or released.</param>
        public void RecordAction(MouseEventType eventType, Point position, bool isDown)
        {
            var now = DateTime.Now - _recordingStartTime;
            var lastMouseAction = GetLastAction(eventType);

            if (eventType == MouseEventType.Move)
            {
                HandleMove(position, now, lastMouseAction);
            }
            else
            {
                if (isDown)
                {
                    HandleDown(eventType, position, now, lastMouseAction);
                }
                else
                {
                    HandleUp(now, lastMouseAction);
                }
            }
        }

        /// <summary>
        /// Handles the logic when the mouse is moved. Adds a new mouse action to the list if there's no previous action
        /// or if the mouse has moved a sufficient distance from the last recorded position.
        /// </summary>
        /// <param name="position">The new mouse position.</param>
        /// <param name="now">The current time.</param>
        /// <param name="lastMouseAction">The last recorded mouse action, if any.</param>
        private void HandleMove(Point position, TimeSpan now, MouseAction? lastMouseAction)
        {
            if (lastMouseAction == null || GetDistance(position, lastMouseAction.Position) >= _minDistanceForRecording)
            {
                _mouseActions.Add(new MouseAction
                {
                    EventType = MouseEventType.Move,
                    Position = position,
                    StartTime = now,
                    EndTime = now
                });
            }
        }

        /// <summary>
        /// Handles the logic when a mouse button is pressed down. Adds a new mouse action to the list if there's no previous action
        /// with the same event type or if the last recorded action has ended.
        /// </summary>
        /// <param name="eventType">The type of mouse event (e.g., left button down).</param>
        /// <param name="position">The position of the mouse when the button was pressed.</param>
        /// <param name="now">The current time.</param>
        /// <param name="lastMouseAction">The last recorded mouse action, if any.</param>
        private void HandleDown(MouseEventType eventType, Point position, TimeSpan now, MouseAction? lastMouseAction)
        {
            if (lastMouseAction == null || lastMouseAction.EventType != eventType || lastMouseAction.EndTime != TimeSpan.Zero)
            {
                _mouseActions.Add(new MouseAction
                {
                    EventType = eventType,
                    Position = position,
                    StartTime = now
                });
            }
            else
            {
                lastMouseAction.StartTime = now;
            }
        }

        /// <summary>
        /// Handles the logic when a mouse button is released. Updates the end time of the last recorded action if it matches
        /// the event type and hasn't been ended yet.
        /// </summary>
        /// <param name="now">The current time.</param>
        /// <param name="lastMouseAction">The last recorded mouse action, if any.</param>
        private void HandleUp(TimeSpan now, MouseAction? lastMouseAction)
        {
            if (lastMouseAction != null && lastMouseAction.EndTime == TimeSpan.Zero)
            {
                lastMouseAction.EndTime = now;
            }
        }

        /// <summary>
        /// Gets the last recorded mouse action from the list.
        /// </summary>
        /// <returns>The last recorded mouse action, or null if there are no actions recorded.</returns>
        private MouseAction? GetLastAction(MouseEventType eventType)
        {
            return _mouseActions
                .Where(action => action.EventType == eventType)
                .LastOrDefault();
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <returns>The distance between the two points.</returns>
        private double GetDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        /// <summary>
        /// Saves the recorded mouse actions to a file in JSON format.
        /// </summary>
        private void SaveActionsToFile()
        {
            var recordData = new
            {
                RecordMs = Converter.TimeSpanToDouble(DateTime.Now - _recordingStartTime),
                Mouse = _mouseActions.Select(action => new
                {
                    EventType = action.EventType.ToString(),
                    Position = new { X = action.Position.X, Y = action.Position.Y },
                    StartTime = Converter.TimeSpanToDouble(action.StartTime),
                    EndTime = Converter.TimeSpanToDouble(action.EndTime)
                })
            };

            JsonProcessor.SaveToFile(recordData);
        }
    }
}