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

        [Test]
        public void CheckAnnualAllowance_WithinAllowance_ReturnsHeadroom()
        {
            string result = PensionCalculatorTools.CheckAnnualAllowance(45, 60000, 1000);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("Annual contributions: 12000"));
                Assert.That(result, Does.Contain("Maximum allowance: 15000"));
                Assert.That(result, Does.Contain("Headroom: 3000"));
            });
        }

        [Test]
        public void CheckAnnualAllowance_ExceedsAllowance_ReturnsOverage()
        {
            string result = PensionCalculatorTools.CheckAnnualAllowance(45, 60000, 1500);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("Annual contributions: 18000"));
                Assert.That(result, Does.Contain("Maximum allowance: 15000"));
                Assert.That(result, Does.Contain("EXCEEDS allowance by: 3000"));
            });
        }
    }
}
