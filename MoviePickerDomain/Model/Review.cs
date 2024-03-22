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
    [Range(0.0, 10.0, ErrorMessage = "Рейтинг повинен бути в діапазоні від 0.0 до 10.0.")]
    public double Rating { get; set; }

    public int MovieId { get; set; }

    public DateTime? CreationTime { get; set; }

    public virtual Movie? Movie { get; set; } //= null!;

}
