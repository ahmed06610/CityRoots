using CityRoots.Core.Interfaces;
using CityRoots.Core.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using CityRoots.Core.DTOs.Crop;

namespace CityRoots.Core.CustomValidation
{
    public class UniqueNameCropAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
          
            if (value is null)
                return new ValidationResult("Name cannot be null.");

            var unitOfWork = (IUnitOfWork)validationContext.GetService(typeof(IUnitOfWork));
            if (unitOfWork is null)
            {
                return new ValidationResult("UnitOfWork is not available.");
            }
            var id = 0;
            if (validationContext.ObjectInstance is AddCropDto)
            {
                id = 0;
            }
            else if (validationContext.ObjectInstance is UpdateCropDto updateCropDto)
            {
                id = updateCropDto.CropId;
            }
          ;

            
           
            string name = value.ToString()?.ToLower();

            var existingCrop = unitOfWork.Crop
                .FindTWithExpression<Crop>(x => x.Name.ToLower() == name && x.CropId != id)
                .GetAwaiter()
                .GetResult();

            if (existingCrop is not null)
            {
                return new ValidationResult($"The name '{name}' is already in use.");
            }

            return ValidationResult.Success;
        }
    }
}
