using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Movie
{
    public long MovieId { get; set; }

    public string Title { get; set; } = null!;

    public DateOnly ReleaseDate { get; set; }

    public long DirectorId { get; set; }

    public long? Budget { get; set; }

    public long? BoxOfficeRevenue { get; set; }

    public int? Duration { get; set; }

    public double? Rating { get; set; }

    public virtual Director Director { get; set; } = null!;

    public virtual ICollection<MoviesActor> MoviesActors { get; set; } = new List<MoviesActor>();

    public virtual MoviesReview? MoviesReview { get; set; }
}
