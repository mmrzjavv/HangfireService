using HangfireWebService.Service; // Ensure correct namespace for IBackgroundJobService
using Carter; // Include Carter for defining modular endpoints
using Microsoft.AspNetCore.Builder; // Required for endpoint routing
using Microsoft.AspNetCore.Http; // For handling HTTP requests/responses

namespace HangfireWebService.Service;

public class HangfireEndpoints : ICarterModule
{
    // Base route for all Hangfire-related APIs
    private const string BaseRoute = "api/v1/hangfire";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        // Grouping routes together with a base route and applying a tag for Swagger documentation
        var hangfire = app.MapGroup(BaseRoute).WithTags("Hangfire");

        // POST: Enqueue Email
        // Adds a fire-and-forget job to send an email to the specified email address.
        hangfire.MapPost("enqueue-email", (IBackgroundJobService backgroundJobService, string email) =>
        {
            backgroundJobService.EnqueueEmailSendingJob(email);
            return Task.FromResult(Results.Ok($"Email queued for sending to {email}."));
        })
        .WithName("EnqueueEmail"); // Assigns a name to the route for identification in Swagger and routing

        // POST: Schedule Recurring Email
        // Schedules a recurring job to send email to a specific address based on a cron expression.
        hangfire.MapPost("schedule-recurring-email", (IBackgroundJobService backgroundJobService, string email, string cronExpression) =>
        {
            backgroundJobService.ScheduleRecurringEmailJob(email, cronExpression);
            return Task.FromResult(Results.Ok($"Recurring email job scheduled for {email} with cron expression: {cronExpression}."));
        })
        .WithName("ScheduleRecurringEmail"); // Assigns a route name for recurring email jobs

        // POST: Fire-and-Forget Job
        // Creates a fire-and-forget background job to process a given message immediately.
        hangfire.MapPost("fire-and-forget", (IBackgroundJobService backgroundJobService, string message) =>
        {
            backgroundJobService.FireAndForgetJob(message);
            return Task.FromResult(Results.Ok($"Fire-and-forget job created with message: {message}."));
        })
        .WithName("FireAndForget"); // Assigns a name to the route for identification

        // POST: Delayed Job
        // Creates a background job that runs after a specified delay in seconds.
        hangfire.MapPost("delayed-job", (IBackgroundJobService backgroundJobService, string message, int delayInSeconds) =>
        {
            backgroundJobService.DelayedJob(message, TimeSpan.FromSeconds(delayInSeconds));
            return Task.FromResult(Results.Ok($"Delayed job created with a delay of {delayInSeconds} seconds. Message: {message}."));
        })
        .WithName("DelayedJob"); // Assigns a name to the delayed job route
    }
}
