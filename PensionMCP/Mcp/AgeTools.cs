using ModelContextProtocol;
using ModelContextProtocol.Server;
using PensionMCP.Engine;
using System.ComponentModel;

namespace PensionMCP.Mcp
{
    [McpServerToolType]
    public class AgeTools : BaseTool
    {
        /// <summary>
        /// Returns the current age of someone on a particular day based on their date of birth
        /// </summary>
        /// <param name="dateOfBirth">Persons date of birth, date format: yyyy-MM-dd</param>
        /// <param name="asOfDate">The date on which the the methods checks the persons age, date format: yyyy-MM-dd </param>
        /// <returns>a formatted string e.g. Age 51 years</returns>
        /// <exception cref="McpException">Throws exception if date format is incorrect</exception>
        [McpServerTool(Title = "Return persons Age on particular Date", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Returns the current age of someone on a particular day based on their date of birth (date format: yyyy-MM-dd)")]
        public static string GetAgeAsOfDate(string dateOfBirth, string asOfDate)
        {
            DateOnly dob = ParseDateParam(dateOfBirth, "dateOfBirth");
            DateOnly asOf = ParseDateParam(asOfDate, "asOf");

            int age = AgeCalculator.GetAgeAsOfDate(dob, asOf);
            return $"Age: {age} years";
        }

        /// <summary>
        /// Get what is Today
        /// </summary>
        /// <returns>Returns a formatted string e.g. Today is the Sunday the 2024-04-19</returns>
        [McpServerTool(Title = "Get what Today is", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Returns todays date and the day of the week (date format: yyyy-MM-dd)")]
        public static string GetToday()
        {
            DateTime today = DateTime.Today;
            return $"Today is {today:dddd} the {today:yyyy-MM-dd}.";
        }

        /// <summary>
        /// Get the date a person turns the specified age.
        /// </summary>
        /// <param name="dateOfBirth">Persons date of birth, date format: yyyy-MM-dd</param>
        /// <param name="targetAge">The age that person turns on a specific date</param>
        /// <returns></returns>
        /// <exception cref="McpException">Throws exception if date format is incorrect</exception>
        [McpServerTool(Title = "Get  date that a person turns specified age", Destructive = false, ReadOnly = true, Idempotent = true, OpenWorld = false)]
        [Description("Returns the date on which a person turns a given age, based on their date of birth (date format: yyyy-MM-dd)")]
        public static string GetDateTurnsAge(string dateOfBirth, int targetAge)
        {
            DateOnly dob = ParseDateParam(dateOfBirth, "asOf");

            DateOnly result = AgeCalculator.GetDateTurnsAge(dob, targetAge);
            return $"Date turns {targetAge}: {result:yyyy-MM-dd}";
        }
    }
}
