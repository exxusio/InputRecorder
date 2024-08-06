namespace InputRecorder.Models.Mouse
{
    /// <summary>
    /// Event arguments for mouse events, including the event type, position, and whether the mouse button is pressed.
    /// </summary>
    internal class MouseEventArgs : EventArgs
    {
        /// <summary>
        /// The type of mouse event (e.g., click, move).
        /// </summary>
        public MouseEventType EventType { get; }

        /// <summary>
        /// The position of the mouse during the event.
        /// </summary>
        public Point Position { get; }

        /// <summary>
        /// Indicates whether the mouse button is pressed down or released.
        /// </summary>
        public bool IsDown { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseEventArgs"/> class.
        /// </summary>
        /// <param name="eventType">The type of mouse event.</param>
        /// <param name="position">The position of the mouse during the event.</param>
        /// <param name="isDown">Indicates whether the mouse button is pressed down or released.</param>
        public MouseEventArgs(MouseEventType eventType, Point position, bool isDown)
        {
            EventType = eventType;
            Position = position;
            IsDown = isDown;
        }
    }
}