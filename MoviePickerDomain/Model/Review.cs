using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Review : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    [StringLength(30, ErrorMessage = "Назва не може бути довшою за 30 символів.")]
    public string Title { get; set; } = null!;

    [StringLength(100, ErrorMessage = "Текст не може бути довшим за 100 символів.")]
    public string? Text { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public double Rating { get; set; }

    public virtual ICollection<MoviesReview> MoviesReviews { get; set; } = new List<MoviesReview>();
}
