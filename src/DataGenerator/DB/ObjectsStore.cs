using System;
using System.Collections.Generic;

namespace DataGenerator.DB;

public partial class ObjectsStore
{
    public Guid Id { get; set; }

    public byte[]? Obj { get; set; }
}
