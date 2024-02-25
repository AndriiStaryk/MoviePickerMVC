using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MoviePickerDomain.Model;

public partial class Actor : Entity, IEquatable<Actor>
{
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [Display(Name = "Дата народження")]
    public DateOnly BirthDate { get; set; }

    public int BirthCountryId { get; set; }

    [Display(Name = "Країна народження")]
    public virtual Country? BirthCountry { get; set; } //= null!;

    public virtual ICollection<MoviesActor> MoviesActors { get; set; } = new List<MoviesActor>();

    public bool Equals(Actor? other)
    {
        return this.GetHashCode() == other.GetHashCode();
        //return Name == other!.Name &&
        //       BirthDate == other.BirthDate &&
        //       BirthCountryId == other.BirthCountryId;
        //throw new NotImplementedException();
    }
    public override int GetHashCode()
    {
        unchecked
        {
            int result = (Name != null ? Name.GetHashCode() : 0);
            result = (result * 757) ^ BirthDate.GetHashCode();
            result = (result * 757) ^ BirthCountryId;
            return result;
        }
        //return HashCode.Combine(Name, BirthDate, BirthCountryId);
    }
}
