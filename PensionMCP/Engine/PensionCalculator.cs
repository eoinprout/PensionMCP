using PensionMCP.Rules;

namespace PensionMCP.Engine
{
    /// <summary>
    /// Contains the required pension calculations
    /// </summary>
    public static class PensionCalculator
    {
        /// <summary>
        /// Calculates the maximum pension contribution that tax relief can be claimed on
        /// </summary>
        /// <param name="age"></param>
        /// <param name="earnings"></param>
        /// <returns>The maximum pension contribution that tax relief can be claimed on</returns>
        /// <exception cref="ArgumentOutOfRangeException">Age cannot be negative</exception>
        /// <remarks>
        /// CALC001: Maximum Pension Contribution
        /// </remarks>
        public static decimal GetMaxContribution(int age, decimal earnings)
        {
            CheckAge(age);

            var limit = GetAgeBand(age);
            var cappedEarnings = Math.Min(earnings, TaxReliefLimits.EarningsCap);
            return cappedEarnings * limit.ReliefPercent / 100m;
        }

        public static TaxReliefLimit GetAgeBand(int age)
        {
            CheckAge(age);
                
            return TaxReliefLimits.ContributionLimits
                .First(l => age >= l.AgeFrom && age <= l.AgeTo);
        }

        private static void CheckAge(int age)
        {
            if (age < 0)
                throw new ArgumentOutOfRangeException(nameof(age), "Age cannot be negative.");
        }
    }
}
