namespace InputRecorder.Models.Keyboard
{
    /// <summary>
    /// Event arguments for keyboard events, including the key code and whether the key is down or up.
    /// </summary>
    internal class KeyEventArgs : EventArgs
    {
        /// <summary>
        /// The key code of the key event.
        /// </summary>
        public Keys KeyCode { get; }

        /// <summary>
        /// Indicates whether the key is pressed down or released.
        /// </summary>
        public bool IsKeyDown { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyEventArgs"/> class.
        /// </summary>
        /// <param name="keyCode">The key code of the key event.</param>
        /// <param name="isKeyDown">Indicates whether the key is pressed down or released.</param>
        public KeyEventArgs(Keys keyCode, bool isKeyDown)
        {
            KeyCode = keyCode;
            IsKeyDown = isKeyDown;
        }
    }
}