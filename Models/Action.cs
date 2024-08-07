namespace InputRecorder.Models
{
    /// <summary>
    /// Represents a base class for actions, including start and end times.
    /// </summary>
    internal abstract class Action
    {
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