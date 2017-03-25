using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Threading.Tasks;
using System.Collections.Specialized;
using AnimeciBackend.Data;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AnimeciBackend
{
    [DisallowConcurrentExecution]
    public class FCheckup : IJob
    {
        ParseFrom _parseFrom;
        AnimeciDbContext _context;
        ILogger<FCheckup> _logger;

        public FCheckup(ParseFrom pf, ILogger<FCheckup> logger, AnimeciDbContext context)
        {
            _parseFrom = pf;
            _context = context;
            _logger = logger;
        }

        public virtual async Task Execute(IJobExecutionContext context)
        {
            var list = await _parseFrom.GetJSONList() as List<JAnime>;
            _logger.LogWarning(1, "Adding `FetchAnime` jobs @{listc}", list.Count);
            foreach (var j in list)
            {
                var qj = new QueJobs();
                qj.job_class = "FetchAnime";
                qj.args = JsonConvert.SerializeObject(j);
                _context.QueJobs.Add(qj);
            }
            var r = await _context.SaveChangesAsync();
            _logger.LogWarning(1, "Added `FetchAnime` jobs {r}/{listc}", r, list.Count);
        }
    }

    public static class QuartzHelper
    {
        public static void AddQuartz(this IServiceCollection sc)
        {
            Task.Run(async () =>
            {
                try
                {
                    System.Console.WriteLine("HTTP_PROXY : " + System.Environment.GetEnvironmentVariable("HTTP_PROXY"));

                    NameValueCollection cfg = new NameValueCollection();
                    cfg.Add("quartz.scheduler.instanceName", "AnimeciScheduler");
                    cfg.Add("quartz.jobStore.type", "Quartz.Simpl.RAMJobStore, Quartz");
                    cfg.Add("quartz.threadPool.threadCount", Environment.ProcessorCount.ToString());
                    StdSchedulerFactory factory = new StdSchedulerFactory();
                    factory.Initialize(cfg);
                    IScheduler scheduler = await factory.GetScheduler();
                    scheduler.JobFactory = new IntegrationJobFactory(sc);

                    // and start it off
                    await scheduler.Start();

                    IJobDetail fc = JobBuilder.Create<FCheckup>()
                        .WithIdentity("FCheckup", "AnimeciWorkers")
                        .Build();

                    ITrigger fct = TriggerBuilder.Create()
                        .WithIdentity("TwiceADay", "AnimeciWorkers")
                        // .StartNow()
                        .StartAt(DateTime.UtcNow.AddMinutes(5))
                        // .WithCronSchedule("0 0,4,8,12,16,20 * * *")
                        .WithSimpleSchedule(x => x
                            .WithIntervalInHours(8)
                            .RepeatForever())
                        .Build();

                    // Tell quartz to schedule the job using our trigger
                    await scheduler.ScheduleJob(fc, fct);
                }
                catch (SchedulerException se)
                {
                    Console.WriteLine(se);
                }
            });
        }
        internal sealed class IntegrationJobFactory : IJobFactory
        {
            private readonly IServiceProvider _container;

            public IntegrationJobFactory(IServiceCollection sc)
            {
                _container = sc.BuildServiceProvider();
            }

            public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            {
                var jobDetail = bundle.JobDetail;

                var job = (IJob)_container.GetRequiredService(jobDetail.JobType);
                return job;
            }

            public void ReturnJob(IJob job)
            {
            }
        }
    }
}
