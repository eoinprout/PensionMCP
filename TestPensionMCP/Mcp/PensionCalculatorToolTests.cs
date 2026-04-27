using ModelContextProtocol;
using PensionMCP.Mcp;

namespace TestPensionMCP.Mcp
{
    internal sealed class PensionCalculatorToolTests
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
        [Test]
        public void CalculateTaxRelief_SingleHigherRate_ReturnsCorrectRelief()
        {
            string result = PensionCalculatorTools.CalculateTaxRelief(45, 50000, 1000, false, 0, false);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("Annual contributions: 12000"));
                Assert.That(result, Does.Contain("Eligible contributions: 12000"));
                Assert.That(result, Does.Contain("Marginal tax rate: 40%"));
                Assert.That(result, Does.Contain("Tax relief: 4800"));
            });
        }

        [Test]
        public void CalculateTaxRelief_ContributionsExceedMax_CapsEligible()
        {
            string result = PensionCalculatorTools.CalculateTaxRelief(45, 50000, 2000, false, 0, false);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("Annual contributions: 24000"));
                Assert.That(result, Does.Contain("Eligible contributions: 12500"));
                Assert.That(result, Does.Contain("Tax relief: 5000"));
            });
        }

        [Test]
        public void CalculateTaxRelief_BothMarriedAndQualifyingSingleParent_ThrowsMcpException()
        {
            Assert.Throws<McpException>(() =>
            {
                PensionCalculatorTools.CalculateTaxRelief(45, 50000, 1000, true, 20000, true);
            });
        }

        [Test]
        public void CalculateUnusedTaxRelief_WithUnusedRoom_ReturnsCorrectValues()
        {
            string result = PensionCalculatorTools.CalculateUnusedTaxRelief(45, 50000, 500, false, 0, false);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("Annual contributions: 6000"));
                Assert.That(result, Does.Contain("Maximum allowable contribution: 12500"));
                Assert.That(result, Does.Contain("Unused contribution room: 6500"));
                Assert.That(result, Does.Contain("Marginal tax rate: 40%"));
                Assert.That(result, Does.Contain("Unused tax relief: 2600"));
            });
        }

        [Test]
        public void CalculateUnusedTaxRelief_ContributionsExceedMax_ReturnsZero()
        {
            string result = PensionCalculatorTools.CalculateUnusedTaxRelief(45, 50000, 2000, false, 0, false);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("Unused contribution room: 0"));
                Assert.That(result, Does.Contain("Unused tax relief: 0"));
            });
        }

        [Test]
        public void CalculateUnusedTaxRelief_BothMarriedAndQualifyingSingleParent_ThrowsMcpException()
        {
            Assert.Throws<McpException>(() =>
            {
                PensionCalculatorTools.CalculateUnusedTaxRelief(45, 50000, 500, true, 20000, true);
            });
        }
    }
}
