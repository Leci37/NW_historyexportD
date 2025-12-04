using System;
using System.Collections.Generic;

namespace HistoryExport_EBI.Domain.Entities;

public partial class Parameter
{
    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    public string? Comment { get; set; }
}
