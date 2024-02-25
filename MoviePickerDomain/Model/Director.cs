using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Director : Entity
{
    public string Name { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public int BirthCountryId { get; set; }

    public virtual Country? BirthCountry { get; set; }// = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
