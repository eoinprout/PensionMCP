using PensionMCP;

namespace TestPensionMCP
{
    public class Tests
    {
        [TestCase("1974-12-12", "2026-04-18", "Age: 51 years")]
        [TestCase("1974-12-12", "2026-12-11", "Age: 51 years")]
        [TestCase("1974-12-12", "2026-12-12", "Age: 52 years")]
        [TestCase("1974-12-12", "2026-12-13", "Age: 52 years")]

        // Leap year test cases
        [TestCase("2004-02-29", "2025-02-28", "Age: 20 years")]
        [TestCase("2004-02-29", "2025-03-01", "Age: 21 years")]
        [TestCase("2004-02-29", "2024-02-29", "Age: 20 years")]
        [TestCase("2004-02-29", "2024-02-28", "Age: 19 years")]

        public void GetAgeAsOfDate_ReturnsCorrectAge(string dateOfBirth, string asOfDate, string expected)
        {
            string result = McpServer.GetAgeAsOfDate(dateOfBirth, asOfDate);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetToday_ReturnsCorrectTodayString()
        {
            string expected = $"Today is {DateTime.Today:dddd} the {DateTime.Today:yyyy-MM-dd}.";
            string result = McpServer.GetToday();
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
