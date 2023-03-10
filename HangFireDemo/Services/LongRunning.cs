namespace HangFireDemo.Services
{
    public interface ILongRunning
    {
        Task<bool> Run();
    }

    public class LongRunning : ILongRunning
    {
        private ILogger<LongRunning> _logger;

        public LongRunning(ILogger<LongRunning> logger)
        {
            _logger = logger;
        }

        public async Task<bool> Run()
        {
            await Task.Delay(10000);

            _logger.LogError($"Test Logger: { DateTime.Now }");
            //Console.Write("Test Console");

            return true;
        }
    }
}
