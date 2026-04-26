using PensionMCP.Engine;
using System.Globalization;

namespace TestPensionMCP.Engine
{
    internal sealed class AgeCalculatorTests
    {
        [TestCase("1974-12-12", "2026-04-18", 51)]
        [TestCase("1974-12-12", "2026-12-11", 51)]
        [TestCase("1974-12-12", "2026-12-12", 52)]
        [TestCase("1974-12-12", "2026-12-13", 52)]
        [TestCase("2004-02-29", "2025-02-28", 20)]
        [TestCase("2004-02-29", "2025-03-01", 21)]
        [TestCase("2004-02-29", "2024-02-29", 20)]
        [TestCase("2004-02-29", "2024-02-28", 19)]
        public void GetAgeAsOfDate_ReturnsCorrectAge(string dateOfBirth, string asOfDate, int expected)
        {
            int result = AgeCalculator.GetAgeAsOfDate(ParseDate(dateOfBirth), ParseDate(asOfDate));
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("1974-12-12", 66, "2040-12-12")]
        [TestCase("1974-12-12", 0, "1974-12-12")]
        [TestCase("2004-02-29", 21, "2025-02-28")]
        [TestCase("2004-02-29", 20, "2024-02-29")]
        public void GetDateTurnsAge_ReturnsCorrectDate(string dateOfBirth, int age, string expectedDate)
        {
            DateOnly result = AgeCalculator.GetDateTurnsAge(ParseDate(dateOfBirth), age);
            Assert.That(result, Is.EqualTo(ParseDate(expectedDate)));
        }

        private static DateOnly ParseDate(string s) => DateOnly.Parse(s, CultureInfo.InvariantCulture);
    }
}
