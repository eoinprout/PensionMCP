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

        [McpServerTool(Title = "Get Pension Lump Sum Details", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Calculates the clients pension lump sum, the tax free amount, the total gross, tax due and nett amount after tax ")]
        public static string GetPensionLumpSumDetails(decimal pensionFundValue, decimal marginalRate)
        {
            CheckNotNegative(pensionFundValue, nameof(pensionFundValue));
            CheckNotNegative(marginalRate, nameof(marginalRate));

            var grossLumpSum = PensionCalculator.CalculateGrossLumpSum(pensionFundValue);
            var taxFreeLumpSum = PensionCalculator.CalculateTaxFreeLumpSum(grossLumpSum);
            var taxDue = PensionCalculator.CalculateLumpSumTax(grossLumpSum, marginalRate);
            var nettLumpSum = grossLumpSum - taxDue;
            return $"""
                Pension fund value: {pensionFundValue}
                Gross lump sum: {grossLumpSum}
                Tax free amount: {taxFreeLumpSum}
                Tax due: {taxDue}
                Nett lump sum: {nettLumpSum}
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

        [McpServerTool(Title = "Check State Pension Entitlement", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Determines a client's likely State Pension (Contributory) entitlement based on their PRSI contributions to date and their expected retirement age. The State Pension is separate from any private pension the client may have.")]
        public static string GetStatePensionEntitlement(int currentAge, int prsiContributions, int retirementAge)
        {
            CheckNotNegative(prsiContributions, nameof(prsiContributions));

            var result = PensionCalculator.CheckStatePensionEntitlement(currentAge, prsiContributions, retirementAge);
            return $"""
                Current PRSI contributions: {result.CurrentContributions}
                Projected additional contributions: {result.ProjectedAdditionalContributions}
                Total projected PRSI contributions at retirement: {result.TotalProjectedContributions}
                State Pension entitlement: {(result.IsEntitled ? "Yes" : "No")}
                Full entitlement: {(result.HasFullEntitlement ? "Yes" : "No")}
                Estimated weekly State Pension: {result.WeeklyStatePension}
                Estimated annual State Pension: {result.AnnualStatePension}
                """;
        }

        [McpServerTool(Title = "Estimate Pension Pot Value", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Estimates the future pension pot value at retirement. annualInterestRate is an assumed growth rate provided as a percentage. monthlyContribution is the client's own monthly contribution only, not including employer contributions. dateOfBirth format: yyyy-MM-dd.")]
        public static string EstimatePensionPot(decimal currentPotValue, decimal monthlyContribution, decimal annualInterestRate, string dateOfBirth, int retirementAge)
        {
            var dob = ParseDateParam(dateOfBirth, nameof(dateOfBirth));
            var today = DateOnly.FromDateTime(DateTime.Today);
            var result = PensionCalculator.EstimatePensionPot(currentPotValue, monthlyContribution, annualInterestRate, dob, retirementAge, today);
            return $"""
                Current pot value: {result.CurrentPotValue}
                Monthly contribution: {result.MonthlyContribution}
                Annual interest rate: {result.AnnualInterestRate}%
                Number of months until retirement: {result.NumberOfMonths}
                Estimated pot value at retirement: {result.EstimatedPotValue}
                """;
        }
    }
}
