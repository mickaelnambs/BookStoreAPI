using Infrastructure.Jobs;
using Quartz;

namespace API.Extensions;

public static class QuartzExtensions
{
    public static IServiceCollection AddQuartzScheduler(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            ConfigureBookImportJob(q, configuration);

            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 10;
            });
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
            options.StartDelay = TimeSpan.FromSeconds(5);
        });

        return services;
    }

    private static void ConfigureBookImportJob(IServiceCollectionQuartzConfigurator quartz, IConfiguration configuration)
    {
        string cronSchedule = configuration["BookImport:CronSchedule"] ?? "0 0 2 * * ?";

        var jobKey = new JobKey("BookImportJob");

        quartz.AddJob<BookImportJob>(opts => opts
            .WithIdentity(jobKey)
            .StoreDurably()
            .RequestRecovery());

        quartz.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("BookImportTrigger")
            .WithCronSchedule(cronSchedule));
    }
}
