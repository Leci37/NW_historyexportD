using HistoryExport_EBI.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using HistoryExport_EBI.Application.Common.Queries;
using HistoryExport_EBI.Infrastructure.Repositories.Queries;
using HistoryExport_EBI.Infrastructure.Repositories.Commands;
using HistoryExport_EBI.Application.Common.Commands;

namespace HistoryExport_EBI.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config,
        IHostEnvironment env)
    {
        var cs = config.GetConnectionString("SqlServer")
                 ?? throw new InvalidOperationException("Missing 'ConnectionStrings:SqlServer'");

        // DbContext (pooled) - lifetime: Scoped
        services.AddDbContextPool<AppDbContext>(options =>
        {
            options.UseSqlServer(cs, sql =>
            {
                sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                sql.CommandTimeout(60);
            });

            if (env.IsDevelopment())
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });


        // Repositorios / servicios de infraestructura
        services.AddScoped<IPointsQueries, PointsRepository>();
        services.AddScoped<IPointsCommands, PointsCommands>();
        // services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        // services.AddSingleton<ISystemClock, SystemClock>(); // ejemplos

        return services;
    }
}
