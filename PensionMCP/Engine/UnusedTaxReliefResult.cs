namespace PensionMCP.Engine
{
    /// <summary>
    /// Clients unused tax relief details
    /// </summary>
    /// <param name="MaxAllowableContribution">The maximum amount of contributions on which tax relief woulfd apply</param>
    /// <param name="AnnualContributions"></param>
    /// <param name="UnusedContributionRoom">Additional pension contributions that could receive tax relief</param>
    /// <param name="MarginalRate">the clients marginal rate of tax</param>
    /// <param name="UnusedTaxRelief">The amount of unused tax relief in euros</param>
    public record UnusedTaxReliefResult(
        decimal MaxAllowableContribution,
        decimal AnnualContributions,
        decimal UnusedContributionRoom,
        decimal MarginalRate,
        decimal UnusedTaxRelief);
}
