namespace RankingEngine.DomainService.Abstractions
{
    public interface IJobTask
    {
        Task ExecuteAsync(CancellationToken token = default);
    }

}
