using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Genre : Entity 
{

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    [StringLength(50, ErrorMessage = "Назва не може бути довшою за 50 символів.")]
    public string Name { get; set; } = null!;

    public virtual ICollection<MoviesGenre> MoviesGenres { get; set; } = new List<MoviesGenre>();
}
