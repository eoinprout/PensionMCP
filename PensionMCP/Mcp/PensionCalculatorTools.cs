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
    }
}
