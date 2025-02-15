// IBackgroundJobService.cs
// Interface definition for the background job execution service
namespace HangfireWebService.Service;

public interface IBackgroundJobService
{
    // Method for executing a background job (e.g., sending an email)
    void EnqueueEmailSendingJob(string email);

    // Method for scheduling a recurring job using a Cron Expression
    void ScheduleRecurringEmailJob(string email, string cronExpression);

    // Method for executing Fire-and-Forget tasks (one-time, non-blocking operations)
    void FireAndForgetJob(string message);

    // Method for executing a task with a specified time delay
    void DelayedJob(string message, TimeSpan delay);
}