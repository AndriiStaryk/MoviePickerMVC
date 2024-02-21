using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Genre
{
    public short GenreId { get; set; }

    public string Name { get; set; } = null!;
}
