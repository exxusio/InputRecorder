using Newtonsoft.Json.Linq;

namespace InputRecorder.Utilities
{
    /// <summary>
    /// Provides utility methods for converting between different data types and JSON tokens.
    /// </summary>
    internal static class Converter
    {
        /// <summary>
        /// Converts a TimeSpan to a double representing the total milliseconds.
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to convert.</param>
        /// <returns>The total milliseconds as a double.</returns>
        public static double TimeSpanToDouble(TimeSpan timeSpan)
        {
            return timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// Converts a JToken to an enum of type TEnum.
        /// </summary>
        /// <typeparam name="TEnum">The type of enum to convert to.</typeparam>
        /// <param name="token">The JToken containing the enum value.</param>
        /// <returns>The enum value represented by the JToken.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the token is null.</exception>
        public static TEnum ToEnum<TEnum>(JToken? token) where TEnum : struct, Enum
        {
            if (token == null || token.Type == JTokenType.Null)
            {
                throw new ArgumentNullException(nameof(token), "Token cannot be null");
            }

            return (TEnum)Enum.Parse(typeof(TEnum), token.ToString());
        }
    }
}