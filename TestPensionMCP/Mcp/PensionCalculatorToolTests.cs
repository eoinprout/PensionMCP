using PensionMCP.Mcp;

namespace TestPensionMCP.Mcp
{
    public class PensionCalculatorToolTests
    {
        [Test]
        public void GetMaxContribution_ReturnsCorrectTaxRelief()
        {
            string result = PensionCalculatorTools.GetMaxContribution(45, 60000);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("relief: 15000"));
                Assert.That(result, Does.Contain("Age: 45"));
                Assert.That(result, Does.Contain("Earnings: 60000"));
                Assert.That(result, Does.Contain("Relief band: 25%"));
            });
        }
    }
}
