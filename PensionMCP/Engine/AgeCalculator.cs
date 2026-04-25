namespace PensionMCP.Engine
{
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

            return age;
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
    }
}
