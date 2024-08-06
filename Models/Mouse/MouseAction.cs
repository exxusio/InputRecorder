namespace InputRecorder.Models.Mouse
{
    /// <summary>
    /// Represents an action involving the mouse, including the event type, position, and the time range during which the action occurred.
    /// </summary>
    internal class MouseAction
    {
        /// <summary>
        /// The type of mouse event (e.g., click, move).
        /// </summary>
        public MouseEventType EventType { get; set; }

        /// <summary>
        /// The position of the mouse during the event.
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// The time when the mouse action started.
        /// </summary>
        public TimeSpan StartTime { get; set; }

        /// <summary>
        /// The time when the mouse action ended.
        /// </summary>
        public TimeSpan EndTime { get; set; }
    }
}