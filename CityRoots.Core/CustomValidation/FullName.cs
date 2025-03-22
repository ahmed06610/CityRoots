using System.ComponentModel.DataAnnotations;

namespace CityRoots.Core.CustomValidation
{
    public class FullName : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("null value");
            }
            string name = value.ToString(); //"Ahmed alaraby" -> {Ahmed,Alaraby}
            string[] arr = name.Split(' ');
            if (arr.Length >= 3)
                return ValidationResult.Success;

            return new ValidationResult("يجب ان يكون الاسم علي الاقل ثلاثي");
        }
    }
}
