using System.ComponentModel.DataAnnotations;

namespace ProjectManagerApi.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            string password = value as string;
            bool upper = false;
            bool digit = false;
            foreach (var item in password)
            {
                if(Char.IsDigit(item))
                {
                    digit = true;
                }
                if(Char.IsUpper(item))
                {
                    upper = true;
                }
            }
            return upper && digit;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(IsValid(value))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Password must contain at least one upper letter and one digit");
            }
        }
    }
}
