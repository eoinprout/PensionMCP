using ModelContextProtocol.Server;
using PensionMCP.Engine;
using System.ComponentModel;

namespace PensionMCP.Mcp
{
    [McpServerToolType]
    public class PensionCalculatorTools : BaseTool
    {
        [McpServerTool(Title = "Get Maximum Pension Contribution", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Calculates the maximum pension contribution on which tax relief can be claimed.")]
        public static string GetMaxContribution(int age, decimal earnings)
        {
            var result = PensionCalculator.GetMaxContribution(age, earnings);
            var band = PensionCalculator.GetAgeBand(age);
            return $"""
                Maximum pension contribution eligible for tax relief: {result}
                Age: {age}
                Earnings: {earnings}
                Relief band: {band.ReliefPercent}%
                """;
        }

        [McpServerTool(Title = "Check Annual Allowance", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Checks whether a client's annual pension contributions exceed the maximum eligible for tax relief. monthlyContribution is the client's own monthly contribution only, not including employer contributions.")]
        public static string CheckAnnualAllowance(int age, decimal earnings, decimal monthlyContribution)
        {
            var result = PensionCalculator.CheckAnnualAllowance(age, earnings, monthlyContribution);
            var status = result.ExceedsAllowance
                ? $"EXCEEDS allowance by: {result.Overage}"
                : $"Within allowance. Headroom: {result.Headroom}";
            return $"""
                Annual contributions: {result.AnnualContributions}
                Maximum allowance: {result.MaxAllowance}
                Status: {status}
                """;
        }
        [McpServerTool(Title = "Calculate Tax Relief", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Calculates the annual pension tax relief for a client based on their income tax band. monthlyContribution is the client's own monthly contribution only, not including employer contributions. Relief is capped at the maximum allowable contribution for the client's age and earnings.")]
        public static string CalculateTaxRelief(int age, decimal earnings, decimal monthlyContribution, bool isMarried, decimal spouseIncome, bool isQualifyingSingleParent)
        {
            CheckMaritalStatus(isMarried, isQualifyingSingleParent);

            var result = PensionCalculator.CalculateTaxRelief(age, earnings, monthlyContribution, isMarried, spouseIncome, isQualifyingSingleParent);
            return $"""
                Annual contributions: {result.AnnualContributions}
                Eligible contributions: {result.EligibleContributions}
                Marginal tax rate: {result.MarginalRate}%
                Tax relief: {result.TaxRelief}
                """;
        }

        [McpServerTool(Title = "Calculate Unused Tax Relief", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Calculates the unused potential tax relief available to a client. This is the additional relief they could claim if they increased contributions to the maximum allowable for their age and earnings. monthlyContribution is the client's own monthly contribution only, not including employer contributions.")]
        public static string CalculateUnusedTaxRelief(int age, decimal earnings, decimal monthlyContribution, bool isMarried, decimal spouseIncome, bool isQualifyingSingleParent)
        {
            CheckMaritalStatus(isMarried, isQualifyingSingleParent);

            var result = PensionCalculator.CalculateUnusedTaxRelief(age, earnings, monthlyContribution, isMarried, spouseIncome, isQualifyingSingleParent);
            return $"""
                Annual contributions: {result.AnnualContributions}
                Maximum allowable contribution: {result.MaxAllowableContribution}
                Unused contribution room: {result.UnusedContributionRoom}
                Marginal tax rate: {result.MarginalRate}%
                Unused tax relief: {result.UnusedTaxRelief}
                """;
        }
    }
}
