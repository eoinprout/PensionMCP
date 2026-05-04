namespace PensionMCP.Rules
{

    /// <summary>
    /// /// FACT005: Tax-Free Lump Sum Limits at Retirement
    /// </summary>
    public static class LumpSumTaxBands
    {

        public const decimal LumpSumPercent = 25m;

        public const decimal TaxFreeBandLimit = 200000m;

        public const decimal LowerBandLimit = 500000m;

        public const decimal LowerRate = 20m;
    }


}
