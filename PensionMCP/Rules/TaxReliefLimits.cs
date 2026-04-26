namespace PensionMCP.Rules
{
    /// <summary>
    /// Pension contribution tax relief limit
    /// </summary>
    /// <param name="AgeFrom">Start of the age band</param>
    /// <param name="AgeTo">End of the age band</param>
    /// <param name="ReliefPercent">Percentage relief that applies</param>
    public record TaxReliefLimit(int AgeFrom, int AgeTo, int ReliefPercent);

    /// <summary>
    /// Contains the pension contribution tax relief limits
    /// </summary>
    public static class TaxReliefLimits
    {
        /// <summary>
        /// Maximum earnings that qualify for pension tax relief
        /// </summary>
        public const decimal EarningsCap = 115_000m;

        /// <summary>
        /// Contribution tax relief limits table
        /// </summary>
        public static readonly IReadOnlyList<TaxReliefLimit> ContributionLimits =
        [
            new(0,  29, 15),
            new(30, 39, 20),
            new(40, 49, 25),
            new(50, 54, 30),
            new(55, 59, 35),
            new(60, int.MaxValue, 40),
        ];
    }
}

