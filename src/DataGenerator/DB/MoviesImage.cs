using System;
using System.Collections.Generic;

namespace DataGenerator.DB;

public partial class MoviesImage
{
    public decimal? MovieId { get; set; }

    public Guid? ImageGuid { get; set; }

    public virtual Movie? Movie { get; set; }
}
