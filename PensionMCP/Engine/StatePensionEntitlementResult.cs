namespace PensionMCP.Engine
{
    public record StatePensionEntitlementResult(
        int CurrentContributions,
        int ProjectedAdditionalContributions,
        int TotalProjectedContributions,
        bool IsEntitled,
        bool HasFullEntitlement,
        decimal WeeklyStatePension,
        decimal AnnualStatePension);
}
