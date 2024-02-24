using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class MoviesLanguage : Entity
{
    //public long Id { get; set; }

    public long MovieId { get; set; }

    public long LanguageId { get; set; }

    public virtual Language Language { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
