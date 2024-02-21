using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Actor : Entity
{
    public long ActorId { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public int BirthCountryId { get; set; }

    public virtual Country BirthCountry { get; set; } = null!;

    public virtual ICollection<MoviesActor> MoviesActors { get; set; } = new List<MoviesActor>();
}
