using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Genre : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public string Name { get; set; } = null!;

    public virtual ICollection<MoviesGenre> MoviesGenres { get; set; } = new List<MoviesGenre>();
}
