namespace HistoryExport_EBI.Application.Dto;

public sealed class PointDto
{
    public int PointId { get; init; }
    public string? PointName { get; init; }
    public string? ParamName { get; init; }
    public string? Description { get; init; }
    public string? Device { get; init; }
    public bool? HistoryFast { get; init; }
    public bool? HistorySlow { get; init; }
    public bool? HistoryExtd { get; init; }
    public bool? HistoryFastArch { get; init; }
    public bool? HistorySlowArch { get; init; }
    public bool? HistoryExtdArch { get; init; }
}
