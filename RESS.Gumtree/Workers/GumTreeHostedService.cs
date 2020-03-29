using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RESS.Gumtree.Workers.Generators;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RESS.Gumtree.Workers
{
    public class GumTreeHostedService : IHostedService
    {
        private readonly ILogger<GumTreeHostedService> _logger;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IPagesGenerator _pagesGenerator;
        private readonly IGumTreeTopicDownloader _gumTreeTopicDownloader;
        public GumTreeHostedService(ILogger<GumTreeHostedService> logger, IHostApplicationLifetime lifetime, IPagesGenerator pagesGenerator, IGumTreeTopicDownloader gumTreeTopicDownloader)
        {
            _logger = logger;
            _lifetime = lifetime;
            _pagesGenerator = pagesGenerator;
            _gumTreeTopicDownloader = gumTreeTopicDownloader;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            
            _lifetime.ApplicationStarted.Register(() => OnStarted(cancellationToken));
            _lifetime.ApplicationStopping.Register(() => OnStopping(cancellationToken));

        }

        private async Task OnStarted(CancellationToken cancellationToken)
        {
            do
            {
                IEnumerable<PageData> result = _pagesGenerator.BuildPagesUrls();
                _gumTreeTopicDownloader.GetTopicContentSynch(result);

                await Task.Delay(10000, cancellationToken);
            } while (!cancellationToken.IsCancellationRequested);
        }

        private async Task OnStopping(CancellationToken cancellationToken)
        {
            await StopAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GumTreeHostedService)} Zatrzymano");
            return Task.CompletedTask;
        }
    }
}