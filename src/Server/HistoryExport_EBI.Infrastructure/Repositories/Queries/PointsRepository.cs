using AutoMapper.QueryableExtensions;
using HistoryExport_EBI.Application.Common.Queries;
using HistoryExport_EBI.Application.Dto;
using HistoryExport_EBI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace HistoryExport_EBI.Infrastructure.Repositories.Queries;

public class PointsRepository : IPointsQueries
{
    private readonly AppDbContext _context;

    private readonly IMapper _mapperConfig;

    public PointsRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapperConfig = mapper;
    }
    public async Task<IReadOnlyList<PointDto>> GetPointsAsync(CancellationToken ct = default)
    {
        var points = await _context.Points
                                .AsNoTracking()
                                .ProjectTo<PointDto>(_mapperConfig.ConfigurationProvider)
                                .ToListAsync(ct);

        return points;
    }
}
