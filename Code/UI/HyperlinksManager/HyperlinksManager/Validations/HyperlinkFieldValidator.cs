using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace HyperlinksManager.Validations
{
    public class HyperlinkFieldValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (Uri.IsWellFormedUriString((string)value, UriKind.Absolute))
                return new ValidationResult(true, null);
            else
                return new ValidationResult(false, "Field value must be supplied.");
        }
    }
}
