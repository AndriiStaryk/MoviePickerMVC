using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Review : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public string Title { get; set; } = null!;

    public string? Text { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public double Rating { get; set; }

    public virtual ICollection<MoviesReview> MoviesReviews { get; set; } = new List<MoviesReview>();
}
