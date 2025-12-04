using System;
using System.Collections.Generic;

namespace HistoryExport_EBI.Domain.Entities;

public partial class History5sec
{
    public int PointId { get; set; }

    public DateTime Usttimestamp { get; set; }

    public DateTime? Timestamp { get; set; }

    public double? Value { get; set; }

    public virtual Point Point { get; set; } = null!;
}
