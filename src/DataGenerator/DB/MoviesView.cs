using System;
using System.Collections.Generic;

namespace DataGenerator.DB;

public partial class MoviesView
{
    public decimal Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public byte[]? Photo { get; set; }

    public decimal? Language { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public TimeSpan? RunTime { get; set; }
}
