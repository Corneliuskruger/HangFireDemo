using Hangfire;
using Hangfire.SqlServer;
using HangFireDemo.Services;
using Microsoft.Extensions.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        builder.Services.AddScoped<ILongRunning, LongRunning>();

        builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));


        builder.Services.AddHangfireServer();

        var app = builder.Build();

        GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_110)
            .UseColouredConsoleLogProvider()
            .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(app.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true
            });

        


        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }


        
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseHangfireDashboard();
        //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHangfireDashboard();
        });

        app.MapRazorPages();

        app.Run();
    }

}