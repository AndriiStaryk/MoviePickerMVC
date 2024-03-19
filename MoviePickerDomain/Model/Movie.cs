using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MoviePickerDomain.Model;

public partial class Movie : Entity
{

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    [StringLength(50, ErrorMessage = "Назва не може бути довшою за 50 символів.")]

    public string Title { get; set; } = null!;

    [MovieDataValidation(ErrorMessage = "Рік релізу не валідний. Фільму не може бути більше за 129 років")]
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public DateOnly ReleaseDate { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public int DirectorId { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Бюджет повинен бути більше або рівним 0.")]
    public long? Budget { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Дохід від каси повинен бути більше або рівним 0.")]
    public long? BoxOfficeRevenue { get; set; }

    [Range(0, 1200, ErrorMessage = "Тривалість повинна бути більше або рівною 0.")]
    public int? Duration { get; set; }

    [Range(0.0, 10.0, ErrorMessage = "Рейтинг повинен бути в діапазоні від 0.0 до 10.0.")]
    public double Rating { get; set; }

    public byte[]? MovieImage { get; set; }

    public string? Description { get; set; }

    public virtual Director? Director { get; set; } //= null!;

    public virtual ICollection<MoviesActor> MoviesActors { get; set; } = new List<MoviesActor>();

    public virtual ICollection<MoviesGenre> MoviesGenres { get; set; } = new List<MoviesGenre>();

    public virtual ICollection<MoviesLanguage> MoviesLanguages { get; set; } = new List<MoviesLanguage>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public double ShowCash(long? cash)
    {
        if (cash != null)
        {
            return Math.Round((double)(cash) / 1_000_000.0, 2);
        }

        return 0;
    }
}
