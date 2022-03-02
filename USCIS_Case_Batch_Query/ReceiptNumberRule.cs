using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace USCIS_Case_Batch_Query
{
    class ReceiptNumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string receiptNumber = "";
            try
            {
                receiptNumber = (string)value;
            }
            catch(Exception e)
            {
                return new ValidationResult(false, "Illegal characters or " + e.Message);
            }

            if(!Regex.Match(receiptNumber, @"((?!=^|;)([a-zA-Z]{3}\d{10}))+").Success)
            {
                return new ValidationResult(false, "Invalid receipt number");
            }

            return new ValidationResult(true, null);
        }
    }
}
