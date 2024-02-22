using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Language : Entity
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MoviesLanguage> MoviesLanguages { get; set; } = new List<MoviesLanguage>();
}
