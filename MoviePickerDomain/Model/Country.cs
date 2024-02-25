using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class Country : Entity
{
    public string Name { get; set; } = null!;

    public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();

    public virtual ICollection<Director> Directors { get; set; } = new List<Director>();
}
