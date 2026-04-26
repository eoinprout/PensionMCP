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

        /// <summary>
        /// CALC002: Annual Allowance Check
        /// </summary>
        /// <param name="age"></param>
        /// <param name="earnings"></param>
        /// <param name="monthlyContribution"></param>
        /// <returns></returns>
        public static AnnualAllowanceResult CheckAnnualAllowance(int age, decimal earnings, decimal monthlyContribution)
        {
            CheckAge(age);

            var maxAllowance = GetMaxContribution(age, earnings);
            var annualContributions = monthlyContribution * 12;
            var exceedsAllowance = annualContributions > maxAllowance;
            var overage = exceedsAllowance ? annualContributions - maxAllowance : 0m;
            var headroom = exceedsAllowance ? 0m : maxAllowance - annualContributions;

            return new AnnualAllowanceResult(annualContributions, maxAllowance, exceedsAllowance, overage, headroom);
        }

        /// <summary>
        /// CALC003: Tax Relief Calculation
        /// </summary>
        /// <param name="age"></param>
        /// <param name="earnings"></param>
        /// <param name="monthlyContribution"></param>
        /// <param name="isMarried"></param>
        /// <param name="spouseIncome"></param>
        /// <returns></returns>
        public static TaxReliefResult CalculateTaxRelief(int age, decimal earnings, decimal monthlyContribution, bool isMarried, decimal spouseIncome, bool isQualifyingSingleParent)
        {
            CheckAge(age);

            var annualContributions = monthlyContribution * 12;
            var maxAllowable = GetMaxContribution(age, earnings);
            var eligibleContributions = Math.Min(annualContributions, maxAllowable);
            var marginalRate = IrishIncomeTaxRates.GetMarginalRate(earnings, isMarried, spouseIncome, isQualifyingSingleParent);
            var taxRelief = eligibleContributions * marginalRate / 100m;

            return new TaxReliefResult(annualContributions, eligibleContributions, marginalRate, taxRelief);
        }

        private static void CheckAge(int age)
        {
            if (age < 0)
                throw new ArgumentOutOfRangeException(nameof(age), "Age cannot be negative.");
        }
    }
}
