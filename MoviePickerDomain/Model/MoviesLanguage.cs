using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class MoviesLanguage : Entity
{
    public int MovieId { get; set; }

    public int LanguageId { get; set; }

    public virtual Language Language { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
