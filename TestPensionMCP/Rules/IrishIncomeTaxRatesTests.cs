using PensionMCP.Rules;

namespace TestPensionMCP.Rules
{
    internal sealed class IrishIncomeTaxRatesTests
    {
        [TestCase(40000, false, 0, 20)]
        [TestCase(42000, false, 0, 20)]
        [TestCase(42001, false, 0, 40)]
        [TestCase(60000, false, 0, 40)]
        [TestCase(50000, true, 0, 20)]
        [TestCase(53000, true, 0, 20)]
        [TestCase(53001, true, 0, 40)]
        [TestCase(70000, true, 20000, 20)]
        [TestCase(75000, true, 20000, 40)]
        [TestCase(85000, true, 35000, 20)]
        [TestCase(90000, true, 35000, 40)]
        [TestCase(85000, true, 50000, 20)]
        [TestCase(90000, true, 50000, 40)]
        public void GetMarginalRate_ReturnsExpectedRate(decimal income, bool isMarried, decimal spouseIncome, decimal expectedRate)
        {
            var rate = IrishIncomeTaxRates.GetMarginalRate(income, isMarried, spouseIncome);

            Assert.That(rate, Is.EqualTo(expectedRate));
        }

        [TestCase(44000, 20)]
        [TestCase(48000, 20)]
        [TestCase(48001, 40)]
        [TestCase(60000, 40)]
        public void GetMarginalRate_QualifyingSingleParent_ReturnsExpectedRate(decimal income, decimal expectedRate)
        {
            var rate = IrishIncomeTaxRates.GetMarginalRate(income, false, 0, true);

            Assert.That(rate, Is.EqualTo(expectedRate));
        }

        [Test]
        public void GetMarginalRate_BothMarriedAndQualifyingSingleParent_ThrowsArgumentException()
        {
            Assert.That(() => IrishIncomeTaxRates.GetMarginalRate(50000, true, 10000, true),
                Throws.TypeOf<ArgumentException>());
        }

        [TestCase(0, 53000)]
        [TestCase(-1, 53000)]
        [TestCase(10000, 63000)]
        [TestCase(35000, 88000)]
        [TestCase(50000, 88000)]
        [TestCase(100000, 88000)]
        public void GetMarriedDualIncomeCutOff_ReturnsExpectedCutOff(decimal spouseIncome, decimal expectedCutOff)
        {
            var cutOff = IrishIncomeTaxRates.GetMarriedDualIncomeCutOff(spouseIncome);

            Assert.That(cutOff, Is.EqualTo(expectedCutOff));
        }
    }
}
