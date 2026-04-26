using ModelContextProtocol;
using PensionMCP.Mcp;

namespace TestPensionMCP.Mcp
{
    // Exposes protected BaseTool methods for testing
    internal class TestableBaseTool : BaseTool
    {
        public static DateOnly CallParseDateParam(string date, string paramName = "date")
        {
            return ParseDateParam(date, paramName);
        }

        public static string CallToJson(object value)
        {
            return ToJson(value);
        }

        public static void CallCheckRequired(string value, string paramName)
        {
            CheckRequired(value, paramName);
        }
    }

    internal sealed class BaseToolTests
    {

        [Test]
        public void ParseDateParam_ValidDate_ReturnsDateOnly()
        {
            var result = TestableBaseTool.CallParseDateParam("1980-01-25", "dateOfBirth");

            Assert.That(result, Is.EqualTo(new DateOnly(1980, 1, 25)));
        }

        [Test]
        public void ParseDateParam_InvalidFormat_ThrowsMcpException()
        {
            var ex = Assert.Throws<McpException>(() =>
            {
                TestableBaseTool.CallParseDateParam("invalid date string", "dateOfBirth");
            });
            Assert.That(ex.Message, Does.Contain("dateOfBirth"));
        }

        [Test]
        public void ToJson_SerializesObject()
        {
            var obj = new { Name = "Spock", Age = 50 };

            var json = TestableBaseTool.CallToJson(obj);

            Assert.That(json, Does.Contain("Spock"));
            Assert.That(json, Does.Contain("50"));
        }

        [Test]
        public void CheckRequired_ValidValue_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => TestableBaseTool.CallCheckRequired("Kirk", "name"));
        }

        [Test]
        public void CheckRequired_EmptyString_ThrowsMcpException()
        {
            var ex = Assert.Throws<McpException>(() =>
            {
                TestableBaseTool.CallCheckRequired("", "name");
            });
            Assert.That(ex.Message, Does.Contain("name"));
        }

        [Test]
        public void CheckRequired_WhitespaceOnly_ThrowsMcpException()
        {
            Assert.Throws<McpException>(() =>
            {
                TestableBaseTool.CallCheckRequired("   ", "name");
            });
        }
    }
}
