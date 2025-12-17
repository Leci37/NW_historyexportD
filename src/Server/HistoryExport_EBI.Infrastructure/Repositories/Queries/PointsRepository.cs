using AutoMapper.QueryableExtensions;
using HistoryExport_EBI.Application.Common.Queries;
using HistoryExport_EBI.Application.Dto;
using HistoryExport_EBI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace HistoryExport_EBI.Infrastructure.Repositories.Queries;

public class PointsRepository : IPointsQueries
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapperConfig;
    private readonly ILogger<PointsRepository> _logger;

    public PointsRepository(AppDbContext context, IMapper mapper, ILogger<PointsRepository> logger)
    {
        _context = context;
        _mapperConfig = mapper;
        _logger = logger;
    }
    public async Task<IReadOnlyList<PointDto>> GetPointsAsync(CancellationToken ct = default)
    {
        var points = await _context.Points
                                .AsNoTracking()
                                .ProjectTo<PointDto>(_mapperConfig.ConfigurationProvider)
                                .ToListAsync(ct);

        if (points.Count == 0)
        {
            _logger.LogWarning("⚠️ [DISCOVERY] No points found in repository.");
        }
        else
        {
            _logger.LogInformation("📊 [QUERY] Retrieved {Count} point(s) from repository.", points.Count);
        }

        return points;
    }
}
