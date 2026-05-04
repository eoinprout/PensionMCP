using ModelContextProtocol;
using PensionMCP.Common;
using System.Text.Json;

namespace PensionMCP.Mcp
{
    public abstract class BaseTool
    {
        /// <summary>
        /// Ensure date parameters are in the expected format
        /// </summary>
        /// <param name="date"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        /// <exception cref="McpException"></exception>
        protected static DateOnly ParseDateParam(string date, string paramName = "date")
        {
            if (!DateOnly.TryParseExact(date, Constants.DefaultDateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateOnly dob))
                throw new McpException($"Invalid {paramName} format. Use " + Constants.DefaultDateFormat);
            return dob;
        }

        /// <summary>
        /// Check that the string param is not empty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <exception cref="McpException"></exception>
        protected static void CheckRequired(string value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new McpException($"{paramName} is required and cannot be empty.");
        }

        /// <summary>
        /// Value cannot be negative
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <exception cref="McpException"></exception>
        protected static void CheckNotNegative(decimal value, string paramName)
        {
            if (value < 0)
                throw new McpException($"{paramName} cannot be negative.");
        }

        /// <summary>
        /// Client cannot be both married and a qualifying single parent.
        /// </summary>
        /// <param name="isMarried"></param>
        /// <param name="isQualifyingSingleParent"></param>
        /// <exception cref="McpException"></exception>
        protected static void CheckMaritalStatus(bool isMarried, bool isQualifyingSingleParent)
        {
            if (isMarried && isQualifyingSingleParent)
                throw new McpException("A client cannot be both married and a qualifying single parent.");
        }

        /// <summary>
        /// Helper method to reduce boiler plate, serialise object to JSON
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static string ToJson(object value)
        {
            return JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
