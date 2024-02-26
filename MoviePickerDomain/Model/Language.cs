using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Language : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public string Name { get; set; } = null!;

    public virtual ICollection<MoviesLanguage> MoviesLanguages { get; set; } = new List<MoviesLanguage>();
}
