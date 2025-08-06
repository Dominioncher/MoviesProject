using System;
using System.Collections.Generic;

namespace DataGenerator.DB;

public partial class MoviesStatistic
{
    public decimal MovieId { get; set; }

    public decimal? ViewsCount { get; set; }

    public virtual Movie Movie { get; set; } = null!;
}
