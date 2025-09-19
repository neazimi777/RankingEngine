namespace RankingEngine.DomainService.Abstractions
{
    public interface IJobScheduler
    {
        public void EnqueueJob<TJob>() where TJob : IJobTask;
        public void ScheduleDelayedJob<TJob>(TimeSpan delay) where TJob : IJobTask;
        public void ScheduleRecurringJob<TJob>(string jobId, string cronExpression) where TJob : IJobTask;
        public void ChainJobs();
    }
}
