using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class MoviesGenre : Entity
{
    //public int Id { get; set; }

    public int MovieId { get; set; }

    public int GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
