namespace PensionMCP.Engine
{
    /// <summary>
    /// Contains age based calculations
    /// </summary>
    public static class AgeCalculator
    {
        /// <summary>
        /// Returns the current age of someone on a particular day based on their date of birth
        /// </summary>
        /// <param name="dateOfBirth">Persons date of birth</param>
        /// <param name="asOfDate">The date on which to determine the persons age</param>
        /// <returns>Persons age on a particular date</returns>
        public static int GetAgeAsOfDate(DateOnly dateOfBirth, DateOnly asOfDate)
        {
            int age = asOfDate.Year - dateOfBirth.Year;

            if (dateOfBirth.Month > asOfDate.Month || (dateOfBirth.Month == asOfDate.Month && dateOfBirth.Day > asOfDate.Day))
                age--;

            return Math.Max(age, 0);
        }

        /// <summary>
        /// Returns the date on which a person turns a given age, based on their date of birth
        /// </summary>
        /// <param name="dateOfBirth">Persons date of birth</param>
        /// <param name="targetAge">The age that we want to know when the person reaches </param>
        /// <returns>Date person turns given age</returns>
        public static DateOnly GetDateTurnsAge(DateOnly dateOfBirth, int targetAge)
        {
            return dateOfBirth.AddYears(targetAge);
        }

        /// <summary>
        /// Returns the number of whole months from today until the clients retirement birthday.
        /// </summary>
        /// <param name="dateOfBirth">Clients date of birth</param>
        /// <param name="retirementAge">Target retirement age in years</param>
        /// <param name="asOf">The from date,normally today</param>
        /// <returns>Number of whole months until retirement</returns>
        public static int MonthsToRetirement(DateOnly dateOfBirth, int retirementAge, DateOnly asOf)
        {
            var retirementDate = GetDateTurnsAge(dateOfBirth, retirementAge);
            var months = ((retirementDate.Year - asOf.Year) * 12) + retirementDate.Month - asOf.Month;
            if (retirementDate.Day < asOf.Day)
                months--;
            return Math.Max(months, 0);
        }
    }
}
