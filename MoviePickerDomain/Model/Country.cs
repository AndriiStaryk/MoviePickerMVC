using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviePickerDomain.Model;

public partial class Country : Entity
{
    [Required(ErrorMessage = "Поле не повинно бути  порожнім")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public virtual ICollection<Director> Directors { get; set; } = new List<Director>();
}
