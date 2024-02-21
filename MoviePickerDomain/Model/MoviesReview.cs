using System;
using System.Collections.Generic;

namespace MoviePickerDomain.Model;

public partial class MoviesReview : Entity
{
    public long Id { get; set; }

    public long MovieId { get; set; }

    public long ReviewId { get; set; }

    public virtual Movie IdNavigation { get; set; } = null!;

    public virtual Review Review { get; set; } = null!;
}
