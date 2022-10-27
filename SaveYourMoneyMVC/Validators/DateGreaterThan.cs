using System.ComponentModel.DataAnnotations;

namespace SaveYourMoneyMVC.Validators
{
    public class DateGreaterThan : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThan(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            ErrorMessage = ErrorMessageString;
            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            if(property.GetValue(validationContext.ObjectInstance) == null)
                return ValidationResult.Success;

            var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance);

            if (currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
