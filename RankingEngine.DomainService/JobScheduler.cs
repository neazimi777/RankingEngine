using Hangfire;
using RankingEngine.DomainService.Abstractions;

namespace RankingEngine.DomainService
{
    public class JobScheduler : IJobScheduler
    {
        public void EnqueueJob<TJob>() where TJob : IJobTask
        {
            BackgroundJob.Enqueue<TJob>(job => job.ExecuteAsync(default));
        }
        public void ScheduleDelayedJob<TJob>(TimeSpan delay) where TJob : IJobTask
        {
            BackgroundJob.Schedule<TJob>(job => job.ExecuteAsync(default), delay);
        }
        public void ScheduleRecurringJob<TJob>(string jobId, string cronExpression) where TJob : IJobTask
        {
            RecurringJob.AddOrUpdate<TJob>(jobId, job => job.ExecuteAsync(default), cronExpression);
        }
        public void ChainJobs()
        {
            var parentId = BackgroundJob.Enqueue(() => Console.WriteLine("Parent Job executed"));
            BackgroundJob.ContinueWith(parentId, () => Console.WriteLine("Continuation Job executed"));
        }
    }
}

