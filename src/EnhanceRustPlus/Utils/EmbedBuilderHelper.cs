using Discord;

namespace EnhanceRustPlus.Utils
{
    public static class EmbedBuilderHelper
    {
        /// <summary>
        /// Creates an error embed with the specified exception.
        /// </summary>
        /// <param name="exception">The exception to include in the error embed.</param>
        /// <returns>An instance of EmbedBuilder representing the error embed.</returns>
        public static EmbedBuilder ErrorEmbed(Exception exception)
        {
            return new EmbedBuilder
            {
                Description = $"Error: {exception.Message}",
                Color = Color.Red
            };
        }
    }
}
