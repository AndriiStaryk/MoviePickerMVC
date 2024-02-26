using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Director : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public DateOnly BirthDate { get; set; }

    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public int BirthCountryId { get; set; }

    public virtual Country? BirthCountry { get; set; }// = null!;

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
