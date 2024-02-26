using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Movie : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public DateOnly ReleaseDate { get; set; }

    public int DirectorId { get; set; }

    public long? Budget { get; set; }

    public long? BoxOfficeRevenue { get; set; }

    public int? Duration { get; set; }

    public double? Rating { get; set; }

    public virtual Director? Director { get; set; } //= null!;

    public virtual ICollection<MoviesActor> MoviesActors { get; set; } = new List<MoviesActor>();

    public virtual ICollection<MoviesGenre> MoviesGenres { get; set; } = new List<MoviesGenre>();

    public virtual ICollection<MoviesLanguage> MoviesLanguages { get; set; } = new List<MoviesLanguage>();

    public virtual ICollection<MoviesReview> MoviesReviews { get; set; } = new List<MoviesReview>();
}
