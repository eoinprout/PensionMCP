using ModelContextProtocol.Server;
using PensionMCP.Rules;
using System.ComponentModel;

namespace PensionMCP.Mcp
{
    [McpServerToolType]
    public class TaxReliefLimitsTool : BaseTool
    {
        [McpServerTool(Title = "Pension Contribution Tax Relief Limits", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Returns the Revenue age-based pension contribution relief limits as JSON")]
        public static string GetTaxReliefLimits()
        {
            return ToJson(TaxReliefLimits.ContributionLimits);
        }
    }
}
