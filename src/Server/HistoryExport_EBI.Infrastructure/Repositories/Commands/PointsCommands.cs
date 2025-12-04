using HistoryExport_EBI.Application.Common.Commands;
using HistoryExport_EBI.Application.Dto;
using HistoryExport_EBI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryExport_EBI.Infrastructure.Repositories.Commands;

public class PointsCommands : IPointsCommands
{
    private readonly AppDbContext _db;
    public PointsCommands(AppDbContext db) => _db = db;
    public async Task<UpdatePointsResponseDto> UpdatePointsAsync(IEnumerable<UpdatePointsDto> items, CancellationToken ct = default)
    {
        var response = new UpdatePointsResponseDto();
        var list = items.ToList();

        var ids = list.Select(x => x.PointId).Distinct().ToArray();

        var current = await _db.Points
            .Where(p => ids.Contains(p.PointId))
            .ToDictionaryAsync(p => p.PointId, ct);

        foreach (var dto in list)
        {
            response.ProcessedCount++;

            if (!current.TryGetValue(dto.PointId, out var e))
            {
                response.FailedCount++;
                response.Results.Add(new ItemResult { PointId = dto.PointId, Status = "not_found", Message = "No existe en BD." });
                continue;
            }

            bool changed = false;
            var changedFields = new List<string>();

            if (!string.Equals(e.Description ?? "", dto.Description, StringComparison.Ordinal) && dto.Description is not null)
            {
                e.Description = dto.Description;
                _db.Entry(e).Property(x => x.Description).IsModified = true;
                changed = true; changedFields.Add(nameof(e.Description));
            }

            if (!string.Equals(e.Device ?? "", dto.Device, StringComparison.Ordinal) && dto.Device is not null)
            {
                e.Device = dto.Device;
                _db.Entry(e).Property(x => x.Device).IsModified = true;
                changed = true; changedFields.Add(nameof(e.Device));
            }

            if (e.HistoryFastArch != dto.HistoryFastArch && dto.HistoryFastArch is not null)
            {
                e.HistoryFastArch = dto.HistoryFastArch;
                _db.Entry(e).Property(x => x.HistoryFastArch).IsModified = true;
                changed = true; changedFields.Add(nameof(e.HistoryFastArch));
            }

            if (e.HistorySlowArch != dto.HistorySlowArch && dto.HistorySlowArch is not null)
            {
                e.HistorySlowArch = dto.HistorySlowArch;
                _db.Entry(e).Property(x => x.HistorySlowArch).IsModified = true;
                changed = true; changedFields.Add(nameof(e.HistorySlowArch));
            }

            if (e.HistoryExtdArch != dto.HistoryExtdArch && dto.HistoryExtdArch is not null)
            {
                e.HistoryExtdArch = dto.HistoryExtdArch;
                _db.Entry(e).Property(x => x.HistoryExtdArch).IsModified = true;
                changed = true; changedFields.Add(nameof(e.HistoryExtdArch));
            }

            if (changed)
            {
                response.UpdatedCount++;
                response.Results.Add(new ItemResult { PointId = dto.PointId, Status = "updated", ChangedFields = changedFields });
            }
            else
            {
                response.Results.Add(new ItemResult { PointId = dto.PointId, Status = "skipped" });
            }
        }

        try
        {
            await _db.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            foreach (var r in response.Results.Where(r => r.Status == "updated"))
            {
                r.Status = "error";
                r.Message = ex.GetBaseException().Message;
            }
            response.FailedCount = response.Results.Count(r => r.Status is "conflict" or "error" or "not_found");
            response.UpdatedCount = response.Results.Count(r => r.Status == "updated");
        }

        return response;
    }
}
