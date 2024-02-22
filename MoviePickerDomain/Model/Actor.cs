using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Actor : Entity
{
    public long ActorId { get; set; }

    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    [Display(Name = "Дата народження")]
    public DateOnly BirthDate { get; set; }

    public long BirthCountryId { get; set; }

    [Display(Name = "Країна народження")]

    public virtual Country BirthCountry { get; set; } = null!;

    public virtual ICollection<MoviesActor> MoviesActors { get; set; } = new List<MoviesActor>();
}
