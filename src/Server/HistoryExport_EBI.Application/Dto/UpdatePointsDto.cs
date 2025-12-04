using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryExport_EBI.Application.Dto;

public sealed class UpdatePointsDto
{
    public int PointId { get; set; }
    public string? Description { get; set; }
    public string? Device { get; set; }
    public bool? HistoryFastArch { get; set; }
    public bool? HistorySlowArch { get; set; }
    public bool? HistoryExtdArch { get; set; }
}
