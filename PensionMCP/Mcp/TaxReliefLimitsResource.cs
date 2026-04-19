using ModelContextProtocol.Server;
using PensionMCP.Rules;
using System.ComponentModel;
using System.Text.Json;

namespace PensionMCP.Mcp
{
    /// <summary>
    /// Resour
    /// </summary>
    [McpServerResourceType]
    public static class TaxReliefLimitsResource
    {
        /// <summary>
        /// Returns the Revenue age-based pension contribution relief limits
        /// </summary>
        /// <returns>JSON string</returns>
        [McpServerResource(UriTemplate = "pension://app/tax_relief_limits", Name = "Contribution tax relief limits", MimeType = "application/json")]
        [Description("Returns the Revenue age-based pension contribution relief limits as JSON")]
        public static string GetTaxReliefLimits()
        {
            return JsonSerializer.Serialize(TaxReliefLimits.ContributionLimits,
                new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
