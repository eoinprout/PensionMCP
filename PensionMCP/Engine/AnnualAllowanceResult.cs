namespace PensionMCP.Engine
{
    public record AnnualAllowanceResult(
        decimal AnnualContributions,
        decimal MaxAllowance,
        bool ExceedsAllowance,
        decimal Overage,
        decimal Headroom);
}
