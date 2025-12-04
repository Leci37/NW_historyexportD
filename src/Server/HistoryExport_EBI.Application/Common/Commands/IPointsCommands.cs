using HistoryExport_EBI.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HistoryExport_EBI.Application.Common.Commands
{
    public interface IPointsCommands
    {
        Task<UpdatePointsResponseDto> UpdatePointsAsync(IEnumerable<UpdatePointsDto> items, CancellationToken ct = default);
    }
}
