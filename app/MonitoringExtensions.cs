using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
public static class MonitoringExtensions
{
    public static IServiceCollection AddMonitoring(this IServiceCollection services)
    {
        return services.AddSingleton<IMonitoringService, MonitoringService>();
    }

    public static IApplicationBuilder UseMonitoring(this IApplicationBuilder builder)
    {
        return builder
            .UseMetricServer()
            .UseMiddleware<ResponseTimeMiddleware>();
    }
}
