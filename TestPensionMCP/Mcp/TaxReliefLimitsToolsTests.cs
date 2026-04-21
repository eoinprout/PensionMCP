using PensionMCP.Mcp;
using PensionMCP.Rules;
using System.Text.Json;

namespace TestPensionMCP.Mcp
{
    [TestFixture]
    public class TaxReliefLimitsToolsTests
    {
        [Test]
        public void GetTaxReliefLimits_ReturnsExpectedReliefBands()
        {
            string json = TaxReliefLimitsTool.GetTaxReliefLimits();
            List<TaxReliefLimit>? limits = JsonSerializer.Deserialize<List<TaxReliefLimit>>(json);

            Assert.That(limits, Is.Not.Null);
            Assert.That(limits.Count, Is.EqualTo(6));
            Assert.That(limits[0], Is.EqualTo(new TaxReliefLimit(0, 29, 15)));
            Assert.That(limits[5], Is.EqualTo(new TaxReliefLimit(60, int.MaxValue, 40)));
        }
    }
}
