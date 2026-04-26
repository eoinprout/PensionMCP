using PensionMCP.Engine;

namespace TestPensionMCP.Engine
{
    public class PensionCalculatorTests
    {

        [TestCase(0, 60000, 9000)]
        [TestCase(29, 60000, 9000)]
        [TestCase(30, 60000, 12000)]
        [TestCase(39, 60000, 12000)]
        [TestCase(40, 60000, 15000)]
        [TestCase(49, 60000, 15000)]
        [TestCase(50, 60000, 18000)]
        [TestCase(54, 60000, 18000)]
        [TestCase(55, 60000, 21000)]
        [TestCase(59, 60000, 21000)]
        [TestCase(60, 60000, 24000)]
        [TestCase(61, 60000, 24000)]
        [TestCase(45, 200000, 28750)]
        [TestCase(45, 0, 0)]
        [TestCase(45, 114999, 28749.75)]
        [TestCase(45, 115000, 28750)]
        [TestCase(45, 115001, 28750)]
        public void GetMaxContribution_ReturnsCorrectAmount(int age, decimal earnings, decimal expected)
        {
            var result = PensionCalculator.GetMaxContribution(age, earnings);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetMaxContribution_NegativeAge_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => PensionCalculator.GetMaxContribution(-1, 60_000),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("age"));
        }

        [Test]
        public void GetAgeBand_NegativeAge_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => PensionCalculator.GetAgeBand(-1),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("age"));
        }

        [TestCase(0, 15)]
        [TestCase(29, 15)]
        [TestCase(30, 20)]
        [TestCase(39, 20)]
        [TestCase(40, 25)]
        [TestCase(49, 25)]
        [TestCase(50, 30)]
        [TestCase(54, 30)]
        [TestCase(55, 35)]
        [TestCase(59, 35)]
        [TestCase(60, 40)]
        [TestCase(65, 40)]
        [TestCase(200, 40)] //just making sure that very high age works
        public void GetAgeBand_ReturnsCorrectReliefPercent(int age, int expectedPercent)
        {
            var band = PensionCalculator.GetAgeBand(age);
            Assert.That(band.ReliefPercent, Is.EqualTo(expectedPercent));
        }

        [TestCase(25, 60000, 700, false, 600, 0)]
        [TestCase(35, 60000, 1100, true, 0, 1200)]
        [TestCase(45, 60000, 1250, false, 0, 0)]
        [TestCase(52, 60000, 1000, false, 6000, 0)]
        [TestCase(57, 200000, 3500, true, 0, 1750)]
        [TestCase(62, 60000, 0, false, 24000, 0)]
        public void CheckAnnualAllowance_ReturnsCorrectResult(
            int age, decimal earnings, decimal monthlyContribution, bool expectedExceeds, decimal expectedHeadroom, decimal expectedOverage)
        {
            var result = PensionCalculator.CheckAnnualAllowance(age, earnings, monthlyContribution);
            Assert.Multiple(() =>
            {
                Assert.That(result.ExceedsAllowance, Is.EqualTo(expectedExceeds));
                Assert.That(result.Headroom, Is.EqualTo(expectedHeadroom));
                Assert.That(result.Overage, Is.EqualTo(expectedOverage));
            });
        }

        [Test]
        public void CheckAnnualAllowance_NegativeAge_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => PensionCalculator.CheckAnnualAllowance(-1, 60_000, 1000),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("age"));
        }
    }
}
