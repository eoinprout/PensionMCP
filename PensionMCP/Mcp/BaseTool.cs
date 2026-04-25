using ModelContextProtocol;
using PensionMCP.Common;
using System.Text.Json;

namespace PensionMCP.Mcp
{
    public abstract class BaseTool
    {
        protected static DateOnly ParseDateParam(string date, string paramName = "date")
        {
            if (!DateOnly.TryParseExact(date, Constants.DefaultDateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateOnly dob))
                throw new McpException($"Invalid {paramName} format. Use " + Constants.DefaultDateFormat);
            return dob;
        }

        protected static void CheckRequired(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new McpException($"{paramName} is required and cannot be empty.");
        }

        protected static string ToJson(object value)
        {
            return JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
