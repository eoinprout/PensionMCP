using ModelContextProtocol;
using PensionMCP.Tools;

namespace TestPensionMCP.Tools
{
    public class AgeToolsTests
    {
        [Test]
        public void GetAgeAsOfDate_InvalidDateOfBirth_ThrowsMcpException()
        {
            Assert.Throws<McpException>(() => AgeTools.GetAgeAsOfDate("not-a-date", "2026-04-19"));
        }

        [Test]
        public void GetAgeAsOfDate_InvalidAsOfDate_ThrowsMcpException()
        {
            Assert.Throws<McpException>(() => AgeTools.GetAgeAsOfDate("1974-12-12", "not-a-date"));
        }

        [Test]
        public void GetAgeAsOfDate_ValidDates_ReturnsFormattedAge()
        {
            string result = AgeTools.GetAgeAsOfDate("1974-12-12", "2026-04-19");
            Assert.That(result, Is.EqualTo("Age: 51 years"));
        }

        [Test]
        public void GetToday_ReturnsFormattedTodayString()
        {
            string expected = $"Today is {DateTime.Today:dddd} the {DateTime.Today:yyyy-MM-dd}.";
            Assert.That(AgeTools.GetToday(), Is.EqualTo(expected));
        }

        [Test]
        public void GetDateTurnsAge_InvalidDateOfBirth_ThrowsMcpException()
        {
            Assert.Throws<McpException>(() => AgeTools.GetDateTurnsAge("invalid date", 66));
        }

        [TestCase("1974-12-12", 66, "Date turns 66: 2040-12-12")]
        [TestCase("1974-12-12", 0, "Date turns 0: 1974-12-12")]
        [TestCase("2004-02-29", 21, "Date turns 21: 2025-02-28")]
        [TestCase("2004-02-29", 20, "Date turns 20: 2024-02-29")]
        public void GetDateTurnsAge_ValidDate_ReturnsFormattedResult(string dateOfBirth, int age, string expected)
        {
            Assert.That(AgeTools.GetDateTurnsAge(dateOfBirth, age), Is.EqualTo(expected));
        }
    }
}
