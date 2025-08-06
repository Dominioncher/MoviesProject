using System;
using System.Collections.Generic;

namespace DataGenerator.DB;

public partial class Top10MoviesView
{
    public decimal Id { get; set; }

    public string? Title { get; set; }

    public byte[]? Photo { get; set; }

    public decimal? ViewsCount { get; set; }
}
