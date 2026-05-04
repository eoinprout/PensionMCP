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

        [Test]
        public void GetPensionLumpSumDetails_ReturnsCorrectValues()
        {
            string result = PensionCalculatorTools.GetPensionLumpSumDetails(1000000, 40);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("Pension fund value: 1000000"));
                Assert.That(result, Does.Contain("Gross lump sum: 250000"));
                Assert.That(result, Does.Contain("Tax free amount: 200000"));
                Assert.That(result, Does.Contain("Tax due: 10000"));
                Assert.That(result, Does.Contain("Nett lump sum: 240000"));
            });
        }

        [Test]
        public void GetPensionLumpSumDetails_NegativePensionFundValue_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<McpException>(() =>
            {
                PensionCalculatorTools.GetPensionLumpSumDetails(-1, 40);
            });
        }

        [Test]
        public void GetPensionLumpSumDetails_NegativeMarginalRate_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<McpException>(() =>
            {
                PensionCalculatorTools.GetPensionLumpSumDetails(100000, -1);
            });
        }

        [Test]
        public void GetStatePensionEntitlement_FullEntitlement_ReturnsCorrectValues()
        {
            string result = PensionCalculatorTools.GetStatePensionEntitlement(26, 0, 66);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("Current PRSI contributions: 0"));
                Assert.That(result, Does.Contain("Projected additional contributions: 2080"));
                Assert.That(result, Does.Contain("Total projected PRSI contributions at retirement: 2080"));
                Assert.That(result, Does.Contain("State Pension entitlement: Yes"));
                Assert.That(result, Does.Contain("Full entitlement: Yes"));
                Assert.That(result, Does.Contain("Estimated weekly State Pension: 299.30"));
                Assert.That(result, Does.Contain("Estimated annual State Pension: 15563.60"));
            });
        }

        [Test]
        public void GetStatePensionEntitlement_PartialEntitlement_ReturnsCorrectValues()
        {
            string result = PensionCalculatorTools.GetStatePensionEntitlement(60, 520, 66);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("Current PRSI contributions: 520"));
                Assert.That(result, Does.Contain("Total projected PRSI contributions at retirement: 832"));
                Assert.That(result, Does.Contain("State Pension entitlement: Yes"));
                Assert.That(result, Does.Contain("Full entitlement: No"));
                Assert.That(result, Does.Contain("Estimated weekly State Pension: 119.72"));
            });
        }

        [Test]
        public void GetStatePensionEntitlement_NotEntitled_ReturnsNotEntitled()
        {
            string result = PensionCalculatorTools.GetStatePensionEntitlement(66, 0, 66);
            Assert.Multiple(() =>
            {
                Assert.That(result, Does.Contain("State Pension entitlement: No"));
                Assert.That(result, Does.Contain("Estimated weekly State Pension: 0"));
            });
        }

        [Test]
        public void GetStatePensionEntitlement_NegativePrsiContributions_ThrowsMcpException()
        {
            Assert.Throws<McpException>(() =>
            {
                PensionCalculatorTools.GetStatePensionEntitlement(40, -1, 66);
            });
        }
    }
}
