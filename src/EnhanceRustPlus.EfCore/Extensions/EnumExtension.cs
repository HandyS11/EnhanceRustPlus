namespace EnhanceRustPlus.EfCore.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// Map an enum to an array of objects
        /// </summary>
        /// <typeparam name="TEnum">Enum type</typeparam>
        /// <typeparam name="TOutput">Object type</typeparam>
        /// <param name="selector">Selector function for specifying options</param>
        /// <returns>An array of object from an enum</returns>
        public static TOutput[] GetEnumAsArrayOutputs<TEnum, TOutput>(Func<string, TOutput> selector) where TEnum : Enum
        {
            return GetEnumAsStrings<TEnum>().Select(selector).ToArray();
        }

        private static IEnumerable<string> GetEnumAsStrings<TEnum>() where TEnum : Enum
        {
            return Enum.GetNames(typeof(TEnum));
        }

        public static IEnumerable<TEnum> GetEnumAsArray<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
