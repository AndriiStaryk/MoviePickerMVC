using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Language : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<MoviesLanguage> MoviesLanguages { get; set; } = new List<MoviesLanguage>();
}
