using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MoviePickerDomain.Model;

public abstract class Entity
{
    public int Id { get; set; }
}


public class PersonDataValidationAttribute : ValidationAttribute
{
    const int MinimumAmountOfYears = 7;
    const int MaximumAmountOfYears = 110;

    public override bool IsValid(object value)
    {
        var dt = (DateOnly)value;
       
        if (DateTime.Now.Year - dt.Year > MaximumAmountOfYears || DateTime.Now.Year - dt.Year < MinimumAmountOfYears)
        {
            return false;
        }

        return true;
    }
}

public class MovieDataValidationAttribute : ValidationAttribute
{
    const int MaximumAmountOfYears = 129;

    public override bool IsValid(object value)
    {
        var dt = (DateOnly)value;

        if (DateTime.Now.Year - dt.Year > MaximumAmountOfYears)
        {
            return false;
        }

        return true;
    }
}
