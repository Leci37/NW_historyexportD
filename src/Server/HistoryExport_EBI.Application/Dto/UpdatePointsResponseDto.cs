using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryExport_EBI.Application.Dto;

public sealed class UpdatePointsResponseDto
{
    public int ProcessedCount { get; set; }
    public int UpdatedCount { get; set; }
    public int FailedCount { get; set; }
    public List<ItemResult> Results { get; init; } = new();    
}

public sealed class ItemResult  
{
    public required int PointId { get; init; }
    public required string Status { get; set; } // updated | skipped | not_found | conflict | error
    public string? Message { get; set; }
    public List<string>? ChangedFields { get; set; }
}