namespace PensionMCP.Rules
{
    /// <summary>
    /// FACT004 State Pension (Contributory) Entitlement Thresholds
    /// Source: Department of Social Protection
    /// </summary>
    public static class StatePensionLimits
    {
        /// <summary>
        /// Minimum number of PRSI contributions required for any State Pension entitlement.
        /// </summary>
        public const int MinContributionsForEntitlement = 520;

        /// <summary>
        /// Number of PRSI contributions required for the full Contributory State Pension.
        /// </summary>
        public const int FullEntitlementContributions = 2080;

        public const int StatePensionAge = 66;

        /// <summary>
        /// Full Contributory State Pension weekly rate.
        /// </summary>
        public const decimal FullWeeklyRate = 299.30m;

        public const int ContributionsPerYear = 52;
    }
}
