using ModelContextProtocol.Server;
using PensionMCP.Rules;
using System.ComponentModel;

namespace PensionMCP.Mcp
{
    /// <summary>
    /// Resour
    /// </summary>
    [McpServerResourceType]
    public class TaxReliefLimitsResource : BaseTool
    {
        /// <summary>
        /// Returns the Revenue age-based pension contribution relief limits
        /// </summary>
        /// <returns>JSON string</returns>
        [McpServerResource(UriTemplate = "pension://app/tax_relief_limits", Name = "Contribution tax relief limits", MimeType = "application/json")]
        [Description("Returns the Revenue age-based pension contribution relief limits as JSON")]
        public static string GetTaxReliefLimits()
        {
            // TODO: Look at a better text format than JSON for this data, Human readable would be nice as its a resource.
            return ToJson(TaxReliefLimits.ContributionLimits);
        }
    }
}
