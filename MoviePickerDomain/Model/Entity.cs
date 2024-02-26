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