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

        /// <summary>
        /// CALC004: Unused Tax Relief Calculation
        /// </summary>
        /// <param name="age"></param>
        /// <param name="earnings"></param>
        /// <param name="monthlyContribution"></param>
        /// <param name="isMarried"></param>
        /// <param name="spouseIncome"></param>
        /// <param name="isQualifyingSingleParent"></param>
        /// <returns></returns>
        public static UnusedTaxReliefResult CalculateUnusedTaxRelief(int age, decimal earnings, decimal monthlyContribution, bool isMarried, decimal spouseIncome, bool isQualifyingSingleParent)
        {
            CheckAge(age);

            var annualContributions = monthlyContribution * 12;
            var maxAllowable = GetMaxContribution(age, earnings);
            var unusedRoom = Math.Max(maxAllowable - annualContributions, 0m);
            var marginalRate = IrishIncomeTaxRates.GetMarginalRate(earnings, isMarried, spouseIncome, isQualifyingSingleParent);
            var unusedTaxRelief = unusedRoom * marginalRate / 100m;

            return new UnusedTaxReliefResult(maxAllowable, annualContributions, unusedRoom, marginalRate, unusedTaxRelief);
        }

        /// <summary>
        /// CALC005: Pension Pot Value Estimate
        /// </summary>
        /// <param name="currentPotValue">Current pension pot value</param>
        /// <param name="monthlyContribution">Monthly contribution amount</param>
        /// <param name="annualInterestRate">Assumed annual interest rate as a percentage</param>
        /// <param name="dateOfBirth">Client's date of birth</param>
        /// <param name="retirementAge">Target retirement age</param>
        /// <param name="asOf">Reference date usally today</param>
        /// <returns>Estimated pension pot value at retirement</returns>
        public static PensionPotEstimateResult EstimatePensionPot(
            decimal currentPotValue,
            decimal monthlyContribution,
            decimal annualInterestRate,
            DateOnly dateOfBirth,
            int retirementAge,
            DateOnly asOf)
        {
            if (dateOfBirth > asOf)
                throw new ArgumentOutOfRangeException(nameof(dateOfBirth), "Date of birth cannot be in the future.");
            if (currentPotValue < 0)
                throw new ArgumentOutOfRangeException(nameof(currentPotValue), "Current pot value cannot be negative.");
            if (monthlyContribution < 0)
                throw new ArgumentOutOfRangeException(nameof(monthlyContribution), "Monthly contribution cannot be negative.");
            if (annualInterestRate < 0)
                throw new ArgumentOutOfRangeException(nameof(annualInterestRate), "Annual interest rate cannot be negative.");

            int n = AgeCalculator.MonthsToRetirement(dateOfBirth, retirementAge, asOf);
            decimal i = annualInterestRate / 100m / 12m;

            decimal estimatedPotValue;

            if (i == 0m)
            {
                estimatedPotValue = currentPotValue + monthlyContribution * n;
            }
            else
            {
                decimal growth = (decimal)Math.Pow((double)(1m + i), n);
                decimal potFV = currentPotValue * growth;
                decimal contributionFV = monthlyContribution * ((growth - 1m) / i) * (1m + i);
                estimatedPotValue = Math.Truncate(potFV + contributionFV); //Truncating as we are not interested in cents in pension pot estimates.
            }

            return new PensionPotEstimateResult(currentPotValue, monthlyContribution, annualInterestRate, n, estimatedPotValue);
        }

        public static decimal CalculateTaxFreeLumpSum(decimal grossLumpSum)
        {
            return Math.Min(grossLumpSum, LumpSumTaxBands.TaxFreeBandLimit);

        }
        public static decimal CalculateGrossLumpSum(decimal pensionFundValue)
        {
            return pensionFundValue * LumpSumTaxBands.LumpSumPercent / 100m;
        }

        public static decimal CalculateLumpSumTax(decimal grossLumpSum, decimal marginalRate)
        {
            decimal tax = 0m;

            // Band 1: up to €200,000 — tax-free
            if (grossLumpSum <= LumpSumTaxBands.TaxFreeBandLimit)
                return 0m;

            // Band 2: €200,001 to €500,000 - Lower Rate
            decimal taxableAtLowerRate = Math.Min(grossLumpSum, LumpSumTaxBands.LowerBandLimit) - LumpSumTaxBands.TaxFreeBandLimit;
            tax += taxableAtLowerRate * LumpSumTaxBands.LowerRate / 100m;

            // Band 3: above €500,000 — marginal rate
            if (grossLumpSum > LumpSumTaxBands.LowerBandLimit)
            {
                decimal taxableAtMarginal = grossLumpSum - LumpSumTaxBands.LowerBandLimit;
                tax += taxableAtMarginal * marginalRate / 100m;
            }

            return tax;
        }

        private static void CheckAge(int age)
        {
            if (age < 0)
                throw new ArgumentOutOfRangeException(nameof(age), "Age cannot be negative.");
        }
    }
}
