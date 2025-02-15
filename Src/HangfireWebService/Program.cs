using Hangfire;
using Hangfire.SqlServer;
using HangfireWebService.Service; // Custom Hangfire endpoints definition using Carter
using Carter; // For Carter integration

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCarter(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; 
});

// Add scoped services to the DI container
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();

// Fetch the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");

// Configure Hangfire using SQL Server
builder.Services.AddHangfire(config => 
    config.UseSqlServerStorage(connectionString, 
            new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.FromSeconds(15),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                DisableGlobalLocks = true
            })
        .WithJobExpirationTimeout(TimeSpan.FromDays(7))
        .UseFilter(new AutomaticRetryAttribute { Attempts = 3 })
);

builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseSwagger();                
app.UseSwaggerUI();             

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseHangfireDashboard("/hangfire");   

app.MapCarter(); 

app.Run();
