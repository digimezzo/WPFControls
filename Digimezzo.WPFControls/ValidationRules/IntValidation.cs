using System;
using System.Globalization;
using System.Windows.Controls;

namespace Digimezzo.WPFControls.ValidationRules
{
    public class IntValidation : ValidationRule
    {
        public int Maximum { get; set; }

        public int Minimum { get; set; } = 0;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var num = Convert.ToInt32((string) value);
                if (Minimum > num || num > Maximum)
                {
                    return new ValidationResult(false, value);
                }
            }
            catch
            {
                return new ValidationResult(false, value);
            }

            return new ValidationResult(true, value);
        }
    }
}