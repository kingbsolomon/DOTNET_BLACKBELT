using System;
using System.ComponentModel.DataAnnotations;

namespace BeltExam.Validations
{
    public class PastDate: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(DateTime.Now > (DateTime)value)
            {
                return new ValidationResult("You can't schedule event in the past.");
            }
            return ValidationResult.Success;
        }
    }
}