using ModelContextProtocol.Server;
using System.ComponentModel;


namespace PensionMCP
{
    [McpServerToolType]
    public static class McpServer

    {

        [McpServerTool, Description("Returns the current age of someone on a particular day based on their date of birth (date format: yyyy-MM-dd)")]
        public static string GetAgeAsOfDate(string dateOfBirth, string asOfDate)
        {
            if (!DateTime.TryParse(dateOfBirth, out DateTime dob))
                return "Invalid date of birth format. Please use yyyy-MM-dd.";

            if (!DateTime.TryParse(asOfDate, out DateTime asOf))
                return "Invalid as of date format. Please use yyyy-MM-dd.";

            int age = asOf.Year - dob.Year;

            if (dob.Month > asOf.Month || (dob.Month == asOf.Month && dob.Day > asOf.Day))
                age--;

            return $"Age: {age} years";
        }

        [McpServerTool, Description("Returns todays date and the day of the week (date format: yyyy-MM-dd)")]
        public static string GetToday()
        {
            DateTime today = DateTime.Today;
            return $"Today is {today:dddd} the {today:yyyy-MM-dd}.";
        }
    }


}
