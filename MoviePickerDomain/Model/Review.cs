using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Review : Entity
{
    public string Title { get; set; } = null!;

    public string? Text { get; set; }

    public int Rating { get; set; }

    public virtual ICollection<MoviesReview> MoviesReviews { get; set; } = new List<MoviesReview>();
}
