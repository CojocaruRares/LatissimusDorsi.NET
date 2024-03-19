using MongoDB.Driver;
using Org.BouncyCastle.Asn1.BC;

namespace LatissimusDorsi.Server.Services
{
    public class TrainingSessionCleanupService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly TrainingSessionService _trainingSessionService;

        public TrainingSessionCleanupService(TrainingSessionService sessionCollection)
        {
            _trainingSessionService = sessionCollection;
        }

        //this will run at the start of the server and then every month
        public Task StartAsync(CancellationToken cancellationToken) {
            RunCleanup();
            TimeSpan timeUntilNextMonth = CalculateTimeUntilNextMonth();
            _timer = new Timer(RunCleanup, null, timeUntilNextMonth, TimeSpan.FromDays(30));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private  void RunCleanup(object? state = null)
        {
            _trainingSessionService.CleanupSessionsAsync().GetAwaiter().GetResult();
        }


        private TimeSpan CalculateTimeUntilNextMonth()
        {
            DateTime now = DateTime.UtcNow;
            DateTime nextMonth = now.AddMonths(1);
            DateTime firstDayOfNextMonth = new DateTime(nextMonth.Year, nextMonth.Month, 1);
            TimeSpan timeUntilNextMonth = firstDayOfNextMonth - now;

            return timeUntilNextMonth;
        }
    }
}
