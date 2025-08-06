using System;
using System.Collections.Generic;

namespace DataGenerator.DB;

public partial class Movie
{
    public decimal Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public byte[]? Photo { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public TimeSpan? RunTime { get; set; }

    public decimal? Language { get; set; }

    public DateTime? DeletedDate { get; set; }

    public string? DeletedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Language? LanguageNavigation { get; set; }

    public virtual MoviesStatistic? MoviesStatistic { get; set; }

    public virtual ICollection<Ganre> Ganres { get; set; } = new List<Ganre>();
}
