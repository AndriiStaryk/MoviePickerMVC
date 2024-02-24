using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class MoviesActor : Entity
{
    //public long Id { get; set; }

    public long MovieId { get; set; }

    public long ActorId { get; set; }

    public virtual Actor Actor { get; set; } = null!;

    public virtual Movie Movie { get; set; } = null!;
}
