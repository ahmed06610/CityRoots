using CityRoots.Core.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.CustomValidation
{
    public class EnumValidationAttribute:ValidationAttribute
    {
        private readonly Type _enumType;
        public EnumValidationAttribute(Type EnumType)
        {
            if(!EnumType.IsEnum)
                throw new ArgumentException("Provided type must be an enum.");
_enumType = EnumType;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is null)
                 throw new Exception("The Value Cannot be null"); 
            var _enum=value.ToString();
            
            if(Enum.TryParse(_enumType,_enum,true,out _))
                { return ValidationResult.Success; }
            var validValues=string.Join(',',Enum.GetNames(_enumType));
            return new ValidationResult($"Invalid value '{_enum}'. Valid values are: {validValues}.");
        }

    }
}
