namespace InputRecorder.Utilities
{
    /// <summary>
    /// Provides methods for validating file and directory paths.
    /// </summary>
    internal static class PathValidator
    {
        /// <summary>
        /// Validates that the specified path is not null, empty, or invalid, and that the directory exists.
        /// </summary>
        /// <param name="path">The path to validate.</param>
        /// <exception cref="ArgumentException">Thrown when the path is null, empty, or invalid.</exception>
        /// <exception cref="DirectoryNotFoundException">Thrown when the directory does not exist.</exception>
        public static void ValidatePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("The path cannot be null or empty.");
            }

            string? directoryPath = Path.GetDirectoryName(path);
            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"The directory '{directoryPath}' does not exist.");
            }

            try
            {
                string fullPath = Path.GetFullPath(path); // Verify that the path is valid
            }
            catch (Exception ex)
            {
                throw new ArgumentException("The file path is invalid.", ex);
            }
        }
    }
}