using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Review : Entity
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public string? Text { get; set; }

    public int? Rating { get; set; }

    public virtual ICollection<MoviesReview> MoviesReviews { get; set; } = new List<MoviesReview>();
}
