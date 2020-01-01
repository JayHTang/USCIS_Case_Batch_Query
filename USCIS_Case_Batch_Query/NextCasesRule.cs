using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace USCIS_Case_Batch_Query
{
    class NextCasesRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int nextCases = 0;
            try
            {
                nextCases = Int32.Parse((string)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if (nextCases < 0 || nextCases > MainWindow.RANGE)
            {
                return new ValidationResult(false, "Invalid next cases range");
            }

            return new ValidationResult(true, null);
        }
    }
}
