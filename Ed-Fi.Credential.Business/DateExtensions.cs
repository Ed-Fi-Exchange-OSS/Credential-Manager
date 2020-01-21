using System;

namespace Ed_Fi.Credential.Business
{
    public static class DateExtensions
    {
 
        public static int GetSchoolYear(this DateTime date)
        {
            if (date.Month > 6)
            {
                return date.Year + 1;
            }
            return date.Year;
        }

       
    }
}
