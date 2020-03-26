using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RESS.Gumtree.Infrastructure;
using RESS.Gumtree.Workers.Generators;

namespace RESS.Gumtree.Workers
{
    public class GumTreeHostedService : IHostedService
    {
        private readonly ILogger<GumTreeHostedService> _logger;
        private readonly GumtreeOption _option;
        private readonly IPagesGenerator _pagesGenerator;
        private readonly IGumTreeTopicDownloader _gumTreeTopicDownloader;

        public GumTreeHostedService(ILogger<GumTreeHostedService> logger, GumtreeOption option, IPagesGenerator pagesGenerator,
            IGumTreeTopicDownloader gumTreeTopicDownloader)
        {
            _logger = logger;
            _option = option;
            _pagesGenerator = pagesGenerator;
            _gumTreeTopicDownloader = gumTreeTopicDownloader;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            do
            {
                IEnumerable<PageData> result = _pagesGenerator.BuildPagesUrls();
                _gumTreeTopicDownloader.GetTopicContentSynch(result);

                await Task.Delay(10000, cancellationToken);
            } while (!cancellationToken.IsCancellationRequested);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(GumTreeHostedService)} Zatrzymano");
            return Task.CompletedTask;
        }
    }
}