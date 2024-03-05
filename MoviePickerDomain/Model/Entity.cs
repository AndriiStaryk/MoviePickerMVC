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

public class DateInThePastAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        var dt = (DateOnly)value;
        if (dt > DateOnly.FromDateTime(DateTime.Now.Date))
        {
            return false;
        }
        return true;
    }
}



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


