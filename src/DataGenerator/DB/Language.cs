using System;
using System.Collections.Generic;

namespace DataGenerator.DB;

public partial class Language
{
    public decimal Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
