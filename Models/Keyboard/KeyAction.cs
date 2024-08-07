namespace InputRecorder.Models.Keyboard
{
    /// <summary>
    /// Represents an action involving a keyboard key, including the key and the time range during which it was pressed.
    /// </summary>
    internal class KeyAction : Action
    {
        /// <summary>
        /// The key that was pressed or released.
        /// </summary>
        public Keys Key { get; set; }
    }
}