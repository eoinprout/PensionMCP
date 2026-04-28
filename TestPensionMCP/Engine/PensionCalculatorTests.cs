using PensionMCP.Engine;

namespace TestPensionMCP.Engine
{
    internal sealed class PensionCalculatorTests
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
            Assert.That(() => PensionCalculator.GetMaxContribution(-1, 60000),
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
            Assert.That(() => PensionCalculator.CheckAnnualAllowance(-1, 60000, 1000),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("age"));
        }


        [TestCase(25, 30000, 300, false, 0, 20, 720)]
        [TestCase(45, 50000, 1000, false, 0, 40, 4800)]
        [TestCase(45, 50000, 2000, false, 0, 40, 5000)]
        [TestCase(52, 60000, 1000, true, 20000, 20, 2400)]
        [TestCase(52, 60000, 1000, true, 0, 40, 4800)]

        // As the spouses incomes increases, the lower rate threshold increases
        // meaning that at one point the couple will lose higher rate relief
        // on pension contributions.
        [TestCase(60, 60000, 500, true, 20000, 20, 1200)]
        [TestCase(60, 60000, 500, true, 0, 40, 2400)]
        [TestCase(60, 60000, 500, true, 6999, 40, 2400)]
        [TestCase(60, 60000, 500, true, 7000, 20, 1200)]
        [TestCase(60, 60000, 500, true, 7001, 20, 1200)]
        [TestCase(60, 60000, 500, false, 0, 40, 2400)]
        public void CalculateTaxRelief_ReturnsCorrectResult(
            int age, decimal earnings, decimal monthlyContribution, bool isMarried, decimal spouseIncome,
            decimal expectedRate, decimal expectedRelief)
        {
            var result = PensionCalculator.CalculateTaxRelief(age, earnings, monthlyContribution, isMarried, spouseIncome, false);
            Assert.Multiple(() =>
            {
                Assert.That(result.MarginalRate, Is.EqualTo(expectedRate));
                Assert.That(result.TaxRelief, Is.EqualTo(expectedRelief));
            });
        }

        [Test]
        public void CalculateTaxRelief_QualifyingSingleParent_ReturnsCorrectResult()
        {
            var result = PensionCalculator.CalculateTaxRelief(45, 50000, 2000, false, 0m, true);
            Assert.Multiple(() =>
            {
                Assert.That(result.MarginalRate, Is.EqualTo(40m));
                Assert.That(result.TaxRelief, Is.EqualTo(5000m));
            });
        }

        [Test]
        public void CalculateTaxRelief_NegativeAge_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => PensionCalculator.CalculateTaxRelief(-1, 60000, 1000, false, 0m, false),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("age"));
        }

        [Test]
        public void CalculateTaxRelief_BothMarriedAndQualifyingSingleParent_ThrowsArgumentException()
        {
            Assert.That(() => PensionCalculator.CalculateTaxRelief(45, 50000, 1000, true, 20000m, true),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void CalculateTaxRelief_QualifyingSingleParent_BelowCutOff_UsesStandardRate()
        {
            var result = PensionCalculator.CalculateTaxRelief(25, 44000, 300, false, 0m, true);

            Assert.That(result.MarginalRate, Is.EqualTo(20m));
        }

        [Test]
        public void CalculateTaxRelief_QualifyingSingleParent_AboveCutOff_UsesHigherRate()
        {
            var result = PensionCalculator.CalculateTaxRelief(25, 50000, 300, false, 0m, true);

            Assert.That(result.MarginalRate, Is.EqualTo(40m));
        }

        [TestCase(29, 50000, 500, false, 0, 1500, 600)]
        [TestCase(35, 50000, 500, false, 0, 4000, 1600)]
        [TestCase(45, 50000, 500, false, 0, 6500, 2600)]
        [TestCase(45, 50000, 1000, false, 0, 500, 200)]
        [TestCase(52, 60000, 500, true, 20000, 12000, 2400)]
        [TestCase(57, 50000, 500, false, 0, 11500, 4600)]
        [TestCase(60, 60000, 500, false, 0, 18000, 7200)]
        [TestCase(60, 60000, 500, true, 20000, 18000, 3600)]

        // Weird edge cases where spouse income increasing 
        // increases lower rate threshold and  ends up reducing pension tax relief
        [TestCase(60, 60000, 500, true, 0, 18000, 7200)]
        [TestCase(60, 60000, 500, true, 6999, 18000, 7200)]
        [TestCase(60, 60000, 500, true, 7000, 18000, 3600)]
        [TestCase(60, 60000, 500, true, 7001, 18000, 3600)]

        public void CalculateUnusedTaxRelief_ReturnsCorrectResult(
            int age, decimal earnings, decimal monthlyContribution, bool isMarried, decimal spouseIncome, decimal expectedUnusedRoom,
            decimal expectedUnusedRelief)
        {
            var result = PensionCalculator.CalculateUnusedTaxRelief(age, earnings, monthlyContribution, isMarried, spouseIncome, false);
            Assert.Multiple(() =>
            {
                Assert.That(result.UnusedContributionRoom, Is.EqualTo(expectedUnusedRoom));
                Assert.That(result.UnusedTaxRelief, Is.EqualTo(expectedUnusedRelief));
            });
        }

        [Test]
        public void CalculateUnusedTaxRelief_ContributionsExceedMax_ReturnsZero()
        {
            var result = PensionCalculator.CalculateUnusedTaxRelief(45, 50000, 2000, false, 0m, false);
            Assert.Multiple(() =>
            {
                Assert.That(result.UnusedContributionRoom, Is.EqualTo(0m));
                Assert.That(result.UnusedTaxRelief, Is.EqualTo(0m));
            });
        }

        [Test]
        public void CalculateUnusedTaxRelief_NegativeAge_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => PensionCalculator.CalculateUnusedTaxRelief(-1, 60000, 500, false, 0m, false),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("age"));
        }

        [Test]
        public void CalculateUnusedTaxRelief_BothMarriedAndQualifyingSingleParent_ThrowsArgumentException()
        {
            Assert.That(() => PensionCalculator.CalculateUnusedTaxRelief(45, 50000, 500, true, 20000m, true),
                Throws.TypeOf<ArgumentException>());
        }

        private static readonly DateOnly _AsOf = new DateOnly(2026, 4, 27);
        private static DateOnly Dob(int currentAge) => new DateOnly(2026 - currentAge, 4, 29);

        [TestCase(0, 500, 0, 30, 60, 180000)]
        [TestCase(10000, 0, 0, 30, 60, 10000)]
        [TestCase(10000, 500, 0, 30, 60, 190000)]
        [TestCase(5000, 0, 0, 50, 50, 5000)]
        [TestCase(0, 0, 5, 30, 60, 0)]
        [TestCase(10000, 0, 12, 30, 31, 11268)]
        [TestCase(0, 1000, 12, 30, 31, 12809)]
        [TestCase(10000, 1000, 12, 30, 31, 24077)]
        [TestCase(0, 500, 5, 30, 60, 417863)]
        [TestCase(10000, 500, 5, 30, 60, 462540)]
        public void EstimatePensionPot_ReturnsCorrectValue(
            decimal currentPotValue, decimal monthlyContribution, decimal annualInterestRate,
            int currentAge, int retirementAge, decimal expectedFV)
        {
            var result = PensionCalculator.EstimatePensionPot(currentPotValue, monthlyContribution, annualInterestRate,
                Dob(currentAge), retirementAge, _AsOf);
            Assert.That(result.EstimatedPotValue, Is.EqualTo(expectedFV));
        }

        [Test]
        public void EstimatePensionPot_ReturnsCorrectNumberOfMonths()
        {
            var result = PensionCalculator.EstimatePensionPot(0, 500, 5, Dob(30), 60, _AsOf);
            Assert.That(result.NumberOfMonths, Is.EqualTo(360));
        }

        [Test]
        public void EstimatePensionPot_FutureDateOfBirth_ThrowsArgumentOutOfRangeException()
        {
            var futureDob = _AsOf.AddDays(1);
            Assert.That(() => PensionCalculator.EstimatePensionPot(0, 500, 5, futureDob, 60, _AsOf),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo("dateOfBirth"));
        }

        [Test]
        public void EstimatePensionPot_RetirementInThePast_ReturnsZeroMonths()
        {
            var result = PensionCalculator.EstimatePensionPot(10000, 500, 5, Dob(60), 50, _AsOf);
            Assert.That(result.NumberOfMonths, Is.EqualTo(0));
            Assert.That(result.EstimatedPotValue, Is.EqualTo(10000));
        }

        [TestCase(-1, 500, 5, "currentPotValue")]
        [TestCase(0, -100, 5, "monthlyContribution")]
        [TestCase(0, 500, -1, "annualInterestRate")]
        public void EstimatePensionPot_InvalidInput_ThrowsArgumentOutOfRangeException(
            decimal pot, decimal contribution, decimal rate, string expectedParam)
        {
            Assert.That(() => PensionCalculator.EstimatePensionPot(pot, contribution, rate, Dob(30), 60, _AsOf),
                Throws.TypeOf<ArgumentOutOfRangeException>()
                      .With.Property("ParamName").EqualTo(expectedParam));
        }
    }
}
