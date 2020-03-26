using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using RESS.Gumtree.Infrastructure;

namespace RESS.Gumtree.Workers.Generators
{
    public class PagesGenerator : IPagesGenerator
    {
        private readonly ILogger<PagesGenerator> _logger;
        private readonly GumtreeOption _option;
        private int _countOfIntervals = 0;

		public PagesGenerator(ILogger<PagesGenerator> logger, GumtreeOption option)
        {
            _logger = logger;
            _option = option;
        }

        public IEnumerable<PageData> BuildPagesUrls()
		{
			int startCounter = _option.PriceFrom;
			int endCounter = _option.PriceFrom + _option.PriceInterval;
			_countOfIntervals = (_option.PriceTo - _option.PriceFrom) / _option.PriceInterval;

			_logger.LogInformation($"{Environment.NewLine}Rozpoczynam budowanie linkow z przedzialami w liczbie {_countOfIntervals}.");
			string firstBaseUrl = GetFirstPageUrlWithoutAttribiute();

			for (int interval = 0; interval < _countOfIntervals; interval++)
			{
				PageData pageData = InitializeDataProgress(startCounter, endCounter);
				bool isFirstExecutionForInterval = true;

				foreach (var page in MaxPagesGenerator.BuildUrlsWithMaxPagesNumber(_option.Url))
				{
					string createdUrl = CreatedUrl(page, startCounter, endCounter);
					HtmlDocument doc;
					try
					{
						doc = new HtmlWeb().Load(createdUrl);
					}
					catch (Exception ex)
					{
						Thread.Sleep(1000);
                        _logger.LogInformation($"Ponowiono próbę pobierania strony dla przedzialu {interval}: {startCounter} - {endCounter} Link: { createdUrl}", ex);
						doc = new HtmlWeb().Load(createdUrl);
					}

					if (IsLastPageInInterval(doc, firstBaseUrl, isFirstExecutionForInterval))
					{
						break;
					}

					var url = doc.DocumentNode.SelectNodes(".//*[@class='href-link tile-title-text']");
					if (url == null)
					{
						break;
					}

					var list = url.Select(x => x.GetAttributeValue("href", string.Empty).Insert(0, "https://www.gumtree.pl")).Distinct().ToList();

					pageData.PagesWithTopicUrls.Add(createdUrl, list);
                    _logger.LogInformation($"Dodano strone dla przedzialu {interval}: {startCounter} - {endCounter} Link: { createdUrl}");
					isFirstExecutionForInterval = false;
				}

				yield return pageData;

                _logger.LogInformation($"Przygotowano {pageData.PagesWithTopicUrls.Count} stron dla przedzialu {interval}: ({startCounter} - {endCounter})");
				FiftyMessage(pageData, startCounter, endCounter);
				var residual = Residual(interval);
				startCounter = startCounter + _option.PriceInterval + residual;
				endCounter = endCounter + _option.PriceInterval;
			}
		}

		private static int Residual(int interval)
		{
			int residual = interval == 0 ? 1 : 0;
			return residual;
		}

		private void FiftyMessage(PageData pageData, int startCounter, int endCounter)
		{
			if (pageData.PagesWithTopicUrls.Count == 50)
			{
                _logger.LogInformation($"OSTRZEZENIE!!!! Znaleziono 50 stron dla {startCounter} - {endCounter}!!! " +
										   $"Prawdopodobnie jest więcej wyników wyszukiwania." +
										   $"Jeżeli potrzebujesz więcej wyników musisz zmniejszych interwał.)!!!!!{Environment.NewLine}");
			}

		}

		private bool IsLastPageInInterval(HtmlDocument doc, string firstBaseUrl, bool isFirstExecutionForInterval)
		{
			string metaPropertyUrl = doc.DocumentNode.SelectSingleNode("//meta[@property='og:url']")?.GetAttributeValue("content", null);

			if ((metaPropertyUrl == firstBaseUrl || string.IsNullOrEmpty(metaPropertyUrl)) && !isFirstExecutionForInterval)
			{
				return true;
			}

			return false;
		}

		private string CreatedUrl(string page, int startCounter, int endCounter)
		{
			string @char = ResolveCharToBindingParameters(page);
			return $"{page}{@char}pr={startCounter},{endCounter}";
		}

		private string GetFirstPageUrlWithoutAttribiute()
		{
			string firstOriginal = _option.Url.Contains("?") ?
				_option.Url.Substring(0, _option.Url.IndexOf("?", StringComparison.Ordinal)) : _option.Url;
            _logger.LogInformation($"Pierwsza wzorcowa strona: {firstOriginal}");
			return firstOriginal;
		}

		private PageData InitializeDataProgress(int startCounter, int endCounter)
		{
			return new PageData()
			{
				StartInterval = startCounter,
				EndInterval = endCounter,
				PagesWithTopicUrls = new Dictionary<string, List<string>>()
			};
		}

		private string ResolveCharToBindingParameters(string page)
		{
			return page.Contains("?") ? "&" : "?";
		}
	}
}