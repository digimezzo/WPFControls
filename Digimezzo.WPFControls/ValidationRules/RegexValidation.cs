using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Digimezzo.WPFControls.ValidationRules
{
    public class RegexValidation : ValidationRule
    {
        public string Pattern
        {
            get => _pattern;
            set
            {
                _pattern = value;
                _isRegexChanged = true;
            }
        }

        private string _pattern;
        private bool _isRegexChanged = false;
        private Regex _regex;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (_isRegexChanged)
            {
                _regex = new Regex(_pattern);
                _isRegexChanged = false;
            }

            return new ValidationResult(_regex.IsMatch((string) value), value);
        }
    }
}