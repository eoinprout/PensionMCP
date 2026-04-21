using ModelContextProtocol.Server;
using PensionMCP.Rules;
using System.ComponentModel;
using System.Text.Json;

namespace PensionMCP.Mcp
{
    [McpServerToolType]
    public static class TaxReliefLimitsTool
    {
        [McpServerTool(Title = "Pension Contribution Tax Relief Limits", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Returns the Revenue age-based pension contribution relief limits as JSON")]
        public static string GetTaxReliefLimits()
        {
            return JsonSerializer.Serialize(TaxReliefLimits.ContributionLimits,
                new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
