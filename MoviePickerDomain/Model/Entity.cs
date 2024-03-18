using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviePickerDomain.Model;

public abstract class Entity
{
    public int Id { get; set; }
}


public class DataValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var dt = (DateOnly)value;
       
        if (DateTime.Now.Year - dt.Year > 110 || DateTime.Now.Year - dt.Year < 7)
        {
            return false;
        }

        return true;
    }
}

//public class DateInThePastAttribute : ValidationAttribute
//{
//    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//    {
//        if (value is DateTime)
//        {
//            DateTime date = (DateTime)value;
//            if (date > DateTime.Now)
//            {
//                return new ValidationResult("Date cannot be in the future");
//            }
//            else if (date < DateTime.Now.AddYears(-100))
//            {
//                return new ValidationResult("Date must not be more than 100 years ago");
//            }
//        }

//        return ValidationResult.Success;
//    }
//}

//public class DateVeryFarInPast : ValidationAttribute
//{
//    private int _maxYearsAgo = 100;
//    public override bool IsValid(object value)
//    {
//        if (value is not DateTime)
//        {
//            return false; // Not a DateTime object
//        }

//        var date = (DateTime)value;

//        // Calculate the minimum allowed date
//        var minDate = DateTime.Now.Date.AddYears(-_maxYearsAgo);

//        // Check if the provided date is not in the future and not more than _maxYearsAgo years ago
//        return date <= DateTime.Now.Date && date >= minDate;
//    }
//}


