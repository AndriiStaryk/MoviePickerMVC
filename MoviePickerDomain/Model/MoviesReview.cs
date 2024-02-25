using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class MoviesReview : Entity
{
    public int MovieId { get; set; }

    public int ReviewId { get; set; }

    public virtual Movie IdNavigation { get; set; } = null!;

    public virtual Review Review { get; set; } = null!;
}
