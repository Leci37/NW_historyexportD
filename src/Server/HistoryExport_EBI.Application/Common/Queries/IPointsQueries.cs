using HistoryExport_EBI.Application.Dto;
using HistoryExport_EBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryExport_EBI.Application.Common.Queries
{
    public interface IPointsQueries
    {
        Task<IReadOnlyList<PointDto>> GetPointsAsync(CancellationToken ct = default);
    }
}
