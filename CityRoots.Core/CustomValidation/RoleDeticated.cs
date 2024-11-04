using CityRoots.Core.Const;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityRoots.Core.CustomValidation
{
    public class RoleDeticated : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("null value");
            }
            if (value.ToString() == Roles.Farmer.ToString() || value.ToString() == Roles.Investor.ToString() || value.ToString() == Roles.Merchant.ToString())
                return ValidationResult.Success;
            return new ValidationResult("should only Farmer or Investor or Merchant!");
        }

    }
}
