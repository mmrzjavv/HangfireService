using Hangfire;

namespace HangfireWebService.Service;

public class BackgroundJobService : IBackgroundJobService
{
    // Method to execute a Fire-and-Forget job: Sending an email
    public void EnqueueEmailSendingJob(string email)
    {
        // This method queues a background job in Hangfire
        BackgroundJob.Enqueue(() => SendEmail(email));
    }

    // Method to schedule a recurring job (e.g., sending emails with a Cron Expression)
    public void ScheduleRecurringEmailJob(string email, string cronExpression)
    {
        // Add or update a recurring job based on the Cron expression (e.g., daily, every minute)
        RecurringJob.AddOrUpdate(() => SendEmail(email), cronExpression);
    }

    // Method to execute a Fire-and-Forget job: Printing a message immediately
    public void FireAndForgetJob(string message)
    {
        // Enqueue a quick task to run in the background
        BackgroundJob.Enqueue(() => Console.WriteLine($"Fire-and-forget job: {message}"));
    }

    // Method to execute a delayed job
    public void DelayedJob(string message, TimeSpan delay)
    {
        // Schedule a job to run after the specified delay
        BackgroundJob.Schedule(() => Console.WriteLine($"Delayed job: {message}"), delay);
    }

    // Simulated method for sending email (Hangfire retries failed jobs by default)
    private void SendEmail(string email)
    {
        try
        {
            // Simulate sending an email
            Console.WriteLine($"Sending email to {email}...");
            
            // Test Retry: Throw an error if the email is a specific address
            if (email == "error@example.com")
            {
                throw new InvalidOperationException($"Failed to send email to {email}");
            }
            
            // Email sent successfully
            Console.WriteLine($"Email successfully sent to {email}!");
        }
        catch (Exception ex)
        {
            // Log the error; Hangfire will manage retry automatically for unhandled exceptions
            Console.WriteLine($"Error sending email to {email}: {ex.Message}");
            throw; // Rethrow the exception to trigger Hangfire retry mechanism
        }
    }
}
