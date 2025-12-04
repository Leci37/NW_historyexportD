using System;
using System.Collections.Generic;

namespace HistoryExport_EBI.Domain.Entities;

public partial class Point
{
    public int PointId { get; set; }

    public string? PointName { get; set; }

    public string? ParamName { get; set; }

    public string? Description { get; set; }

    public string? Device { get; set; }

    public bool? HistoryFast { get; set; }

    public bool? HistorySlow { get; set; }

    public bool? HistoryExtd { get; set; }

    public bool? HistoryFastArch { get; set; }

    public bool? HistorySlowArch { get; set; }

    public bool? HistoryExtdArch { get; set; }

    public virtual ICollection<History1hour> History1hours { get; set; } = new List<History1hour>();

    public virtual ICollection<History1min> History1mins { get; set; } = new List<History1min>();

    public virtual ICollection<History5sec> History5secs { get; set; } = new List<History5sec>();
}
