using System;
using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace Lsw.Abp.BackgroundWorkers.Hangfire
{
    [DependsOn(
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpHangfireModule))]
    public class AbpBackgroundWorkerHangfireModule : AbpModule
    {
        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            var options = context.ServiceProvider.GetRequiredService<IOptions<AbpBackgroundWorkerOptions>>().Value;
            if (!options.IsEnabled)
            {
                var hangfireOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpHangfireOptions>>().Value;
                hangfireOptions.BackgroundJobServerFactory = CreateOnlyEnqueueJobServer;
            }
        }

        private BackgroundJobServer CreateOnlyEnqueueJobServer(IServiceProvider serviceProvider)
        {
            serviceProvider.GetRequiredService<JobStorage>();
            return null;
        }
    }
}
