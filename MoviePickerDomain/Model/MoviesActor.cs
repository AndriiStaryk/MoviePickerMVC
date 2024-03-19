using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class MoviesActor : Entity
{

    public int MovieId { get; set; }

    public int ActorId { get; set; }

    public virtual Actor Actor { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
