using PensionMCP.Common;

namespace PensionMCP.Models
{
    public class Client
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public decimal NetRelevantIncome { get; set; } = Constants.AmountNotSet;
        public decimal CurrentPensionPotValue { get; set; } = Constants.AmountNotSet;
        public decimal CurrentMonthlyPensionContribution { get; set; } = Constants.AmountNotSet;
        public decimal CurrentMonthlyEmployersContribution { get; set; } = Constants.AmountNotSet;
        public bool IsMarried { get; set; }
        public decimal SpouseIncome { get; set; } = Constants.AmountNotSet;
        public bool IsQualifyingSingleParent { get; set; }

    }
}

