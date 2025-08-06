using System;
using System.Collections.Generic;

namespace DataGenerator.DB;

public partial class MigrationHistory
{
    public string Migration { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool Status { get; set; }
}
