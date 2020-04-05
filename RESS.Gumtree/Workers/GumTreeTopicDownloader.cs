using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using RESS.Gumtree.DTO;
using RESS.Gumtree.Infrastructure;
using RESS.Gumtree.Mongo;
using RESS.Gumtree.Mongo.Documents;
using RESS.Gumtree.Services;
using RESS.Gumtree.Workers.Generators;
using RESS.Shared.ExtensionMethods;

namespace RESS.Gumtree.Workers
{
    public class GumTreeTopicDownloader : IGumTreeTopicDownloader
    {
        private static ILogger<GumTreeTopicDownloader> _logger;
        private readonly IGumTreeService _gumTreeService;
        private readonly GumtreeOption _option;
        private int countFoundedTopic;

        public GumTreeTopicDownloader(ILogger<GumTreeTopicDownloader> logger, IGumTreeService gumTreeService, GumtreeOption option)
        {
            _logger = logger;
            _gumTreeService = gumTreeService;
            _option = option;
        }

        public void GetTopicContentSynch(IEnumerable<PageData> pagesIntervalData)
        {
            foreach (PageData pageIntervalData in pagesIntervalData)
            {
                if (!pageIntervalData.PagesWithTopicUrls.Any())
                {
                    continue;
                }

                int allTopicsCount = 1;
                countFoundedTopic = pageIntervalData.PagesWithTopicUrls.Values.Sum(list => list.Count);
                TickTime(() =>
                {
                    _logger.LogInformation($"Rozpoczynam pobieranie danych dla przedzialu" +
                                               $" {pageIntervalData.StartInterval} - {pageIntervalData.EndInterval}." +
                                               $" Liczba stron: {pageIntervalData.PagesWithTopicUrls.Count}." +
                                               $" Liczba znalezionych ogloszen: {countFoundedTopic}");

                    foreach (var pageWithTopic in pageIntervalData.PagesWithTopicUrls)
                    {
                        foreach (var topic in pageWithTopic.Value)
                        {
                            TickTime(async () =>
                                {
                                    HtmlDocument doc = new HtmlDocument();
                                    try
                                    {
                                        doc = new HtmlWeb().Load(topic);
                                        await CreateOrRetryIfFailure(topic, doc);
                                    }
                                    catch (Exception ex)
                                    {
                                        if (!await CreateOrRetryIfFailure(topic, doc))
                                        {
                                            _logger.LogError($"Nie udało się pobrać ogłoszenia: {topic}", ex.Message);
                                        }
                                    }
                                }, $"Nr. {allTopicsCount++} / {countFoundedTopic} {pageIntervalData.StartInterval} - {pageIntervalData.EndInterval}");
                        }
                    }
                }, $"Przedział: {pageIntervalData.StartInterval} - {pageIntervalData.EndInterval}");

                _logger.LogInformation($"Zakonczono pobieranie danych dla przedzialu {pageIntervalData.StartInterval} - {pageIntervalData.EndInterval} " +
                                       $"w liczbie: {allTopicsCount}");
            }
            _logger.LogInformation($"Łącznie pobrano {_gumTreeService.CountOfAllTopicAsync()} ");
        }

        private async Task<bool> CreateOrRetryIfFailure(string topicUrl, HtmlDocument doc)
        {
            try
            {
                if (string.IsNullOrEmpty(doc.ParsedText))
                {
                    Thread.Sleep(5000);
                    doc = new HtmlWeb().Load(topicUrl);
                    if (string.IsNullOrEmpty(doc.ParsedText))
                    {
                        return false;
                    }
                }

                await _gumTreeService.CreateAsync(CreateTopic(topicUrl, doc));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Nie udało się pobrać ogłoszenia próba 1: {topicUrl}", ex.Message);
                return false;
            }
        }

        private GumtreeTopicDto CreateTopic(string topicUrl, HtmlDocument doc)
        {
            return new GumtreeTopicDto()
            {
                Url = topicUrl,
                Title = doc.DocumentNode.SelectSingleNode(".//span[@class='myAdTitle']").InnerText.Replace("&nbsp;", ""),
                CreatedDate = doc.DocumentNode.SelectNodes(".//*[@class='vip-details']//ul//li//div")?.FirstOrDefault(x => x.InnerText.Contains("Data dodania"))?.LastChild.InnerText,
                SizeM2 = double.TryParse(doc.DocumentNode.SelectNodes(".//*[@class='vip-details']//ul//li//div")?.FirstOrDefault(x => x.InnerText.Contains("Wielkość (m2)"))
                    ?.LastChild.InnerText, out var sizeM2) ? sizeM2 : double.NaN,
                PropertyType = doc.DocumentNode.SelectNodes(".//*[@class='vip-details']//ul//li//div")?.FirstOrDefault(x => x.InnerText.Contains("Rodzaj nieruchomości"))?.LastChild.InnerText,
                Garage = doc.DocumentNode.SelectNodes(".//*[@class='vip-details']//ul//li//div")?.FirstOrDefault(x => x.InnerText.Contains("Parking"))?.LastChild?.InnerText,
                Price = double.TryParse(doc.DocumentNode.SelectNodes("//*[@class='price']")?.FirstOrDefault()?.InnerText?.Replace("&nbsp;", "")
                    .Replace(" ", "").Replace("\n", "").Replace("zł", "").Trim(), out var price) ? price : 0,
                City = doc.DocumentNode.SelectNodes(".//*[@class='vip-details']//ul//li//div")?.FirstOrDefault(x => x.InnerText.Contains("Lokalizacja"))?.LastChild?.InnerText,
                Province = doc.DocumentNode.SelectSingleNode(".//span[@class='microdata']")?.InnerText,
                Id = Guid.NewGuid(),
                Description = doc.DocumentNode.SelectSingleNode(".//span[@class='pre']")?.InnerText
            };
        }

        public static void TickTime(Action action, string message = null)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            action.Invoke();
            stopWatch.Stop();
            _logger.LogInformation($"Time: { stopWatch.Elapsed} {message}");
        }
    }
}