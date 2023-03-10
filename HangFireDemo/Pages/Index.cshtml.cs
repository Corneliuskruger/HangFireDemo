using Hangfire;
using HangFireDemo.Models;
using HangFireDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;

namespace HangFireDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IBackgroundJobClient _backgroundJobs;

        public IndexModel(ILogger<IndexModel> logger, IServiceProvider serviceProvider, IConfiguration configuration, IBackgroundJobClient backgroundJobs)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _backgroundJobs = backgroundJobs;

        }

        public void OnGet()
        {
            var jobs = _configuration.GetRequiredSection("HangFireShedule").Get<List<JobDefinition>>();

            if (jobs != null)
            {
                foreach (var job in jobs)
                {
                    RecurringJob.AddOrUpdate(job.JobId, () => StartLongRunning(), "55 11 * * *"); // job.Schedule);
                }
            }
        }


        public void StartLongRunning()
        {
            var longrunning = _serviceProvider.GetRequiredService<ILongRunning>();
            longrunning.Run();
        }
    }
}