namespace PensionMCP.Engine
{
    /// <summary>
    /// Result of the pension pot value estimate calculation
    /// </summary>
    /// <param name="CurrentPotValue">The current pension pot value</param>
    /// <param name="MonthlyContribution">The monthly contribution amount</param>
    /// <param name="AnnualInterestRate">The assumed annual interest rate as a percentage</param>
    /// <param name="NumberOfMonths">The number of months until retirement</param>
    /// <param name="EstimatedPotValue">The estimated pension pot value at retirement</param>
    public record PensionPotEstimateResult(
        decimal CurrentPotValue,
        decimal MonthlyContribution,
        decimal AnnualInterestRate,
        int NumberOfMonths,
        decimal EstimatedPotValue);
}
