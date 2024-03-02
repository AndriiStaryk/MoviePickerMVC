using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Movie : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    [StringLength(50, ErrorMessage = "Назва не може бути довшою за 50 символів.")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public DateOnly ReleaseDate { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public int DirectorId { get; set; }

    public long? Budget { get; set; }

    public long? BoxOfficeRevenue { get; set; }

    public int? Duration { get; set; }

    public double? Rating { get; set; }

    public virtual Director? Director { get; set; } //= null!;

    public virtual ICollection<MoviesActor> MoviesActors { get; set; } = new List<MoviesActor>();

    public virtual ICollection<MoviesGenre> MoviesGenres { get; set; } = new List<MoviesGenre>();

    public virtual ICollection<MoviesLanguage> MoviesLanguages { get; set; } = new List<MoviesLanguage>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
