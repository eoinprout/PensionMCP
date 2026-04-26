namespace PensionMCP.Rules
{
    /// <summary>
    /// FACT003: Irish Income Tax Rates
    /// Source: Revenue Commissioners (Tax Year 2026)
    /// </summary>
    public static class IrishIncomeTaxRates
    {
        /// <summary>
        /// Standard rate of income tax
        /// </summary>
        public const decimal StandardRate = 20m;

        /// <summary>
        /// Higher rate of income tax
        /// </summary>
        public const decimal HigherRate = 40m;

        /// <summary>
        /// Standard rate cut-off point for a single person
        /// </summary>
        public const decimal SinglePersonCutOff = 42000m;

        /// <summary>
        /// Standard rate cut-off for a single/widowed person who qualifies for the Single Person Child Carer Credit
        /// </summary>
        public const decimal QualifyingSingleParentCutOff = 48000m;

        /// <summary>
        /// Base standard rate cut-off for a married couple (dual income)
        /// </summary>
        public const decimal MarriedDualIncomeCutOffBase = 53000m;

        /// <summary>
        /// Maximum transferable unused cut-off from spouse (dual income)
        /// </summary>
        public const decimal MaxSpouseCutOffTransfer = 35000m;

        public static decimal GetMarriedDualIncomeCutOff(decimal spouseIncome)
        {
            return MarriedDualIncomeCutOffBase + Math.Min(Math.Max(spouseIncome, 0), MaxSpouseCutOffTransfer);
        }

        public static decimal GetMarginalRate(decimal income, bool isMarried, decimal spouseIncome = 0m, bool isQualifyingSingleParent = false)
        {
            if (isMarried && isQualifyingSingleParent)
                throw new ArgumentException("A client cannot be both married and a qualifying single parent.");

            decimal cutOff;
            if (isMarried)
                cutOff = GetMarriedDualIncomeCutOff(spouseIncome);
            else if (isQualifyingSingleParent)
                cutOff = QualifyingSingleParentCutOff;
            else
                cutOff = SinglePersonCutOff;

            return income > cutOff ? HigherRate : StandardRate;
        }
    }
}
