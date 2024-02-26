using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Director : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    [StringLength(50, ErrorMessage = "Ім'я не може бути довшим за 50 символів.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    [DateInThePast(ErrorMessage = "Рік народження не може бути в майбутньому.")]
    public DateOnly BirthDate { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public int BirthCountryId { get; set; }

    public virtual Country? BirthCountry { get; set; }// = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
