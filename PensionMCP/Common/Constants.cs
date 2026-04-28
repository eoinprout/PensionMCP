namespace PensionMCP.Common
{
    /// <summary>
    /// Stores the application constants
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// A value used to indicate that an amount field has not been set.
        /// Using zero would casue confusion as amounts could be set to 0.
        /// so -1 is used.
        /// </summary>
        public const decimal AmountNotSet = -1m;

        public const string DefaultDateFormat = "yyyy-MM-dd";

        public const int StandardRetirementAge = 66;

    }
}
