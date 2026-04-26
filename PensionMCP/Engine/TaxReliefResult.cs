namespace PensionMCP.Engine
{
    public record TaxReliefResult(
       decimal AnnualContributions,
       decimal EligibleContributions,
       decimal MarginalRate,
       decimal TaxRelief);
}
