using System.ComponentModel.DataAnnotations;

namespace SaveYourMoneyMVC.Validators
{
    public class ValidateYears : ValidationAttribute
    {
        private readonly DateTime _minValue = new DateTime(2015, 1, 1);
        private readonly DateTime _maxValue = new DateTime(2022, 12, 31);

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            DateTime val = (DateTime)value;
            return val >= _minValue && val <= _maxValue;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessage, _minValue.ToString("dd/MM/yyyy"), _maxValue.ToString("dd/MM/yyyy"));
        }
    }
}
