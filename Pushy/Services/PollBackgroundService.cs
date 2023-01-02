using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Pushy.Services
{
    public class PollBackgroundService : BackgroundService
    {
        private readonly IPollActionAsync pollAction;
        private readonly TimeSpan pollDuration;
        public bool HasError { get; private set; }

        public PollBackgroundService(IPollActionAsync pollAction, TimeSpan pollDuration)
        {
            this.pollAction = pollAction;
            this.pollDuration = pollDuration;
        }

        public IPollActionAsync PollAction => pollAction;
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    // Console.WriteLine($"Poll action {pollAction.GetType().Name} {Thread.CurrentThread.ManagedThreadId}");                    
                    await pollAction.poll();

                    await Task.Delay(pollDuration, stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine($"Service stopped HOST for UseWindowsService={pollAction.GetType().Name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with service {pollAction.GetType().Name} message={ex.Message}", ex);
                HasError = true;
            }
        }
    }
}