using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class MoviesGenre
{
    public long Id { get; set; }

    public long MovieId { get; set; }

    public short GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
