﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moonglade.Core;
using Moonglade.Model.Settings;
using System.IO;
using System.Threading.Tasks;

namespace Moonglade.Web.Controllers
{
    public class SubscriptionController : MoongladeController
    {
        private readonly SyndicationService _syndicationFeedService;

        public SubscriptionController(
            ILogger<SubscriptionController> logger,
            IOptions<AppSettings> settings,
            SyndicationService syndicationFeedService)
            : base(logger, settings)
        {
            _syndicationFeedService = syndicationFeedService;
        }

        [Route("rss/{routeName?}")]
        public async Task<IActionResult> Rss(string routeName = null)
        {
            var rssDataFile = string.IsNullOrWhiteSpace(routeName) ?
                Path.Join($"{SiteDataDirectory}", "feed", "posts.xml") :
                Path.Join($"{SiteDataDirectory}", "feed", $"posts-category-{routeName}.xml");

            if (!System.IO.File.Exists(rssDataFile))
            {
                Logger.LogInformation($"RSS file not found, writing new file on {rssDataFile}");

                if (string.IsNullOrWhiteSpace(routeName))
                {
                    await _syndicationFeedService.RefreshFeedFileAsync(false);
                }
                else
                {
                    await _syndicationFeedService.RefreshRssFilesForCategoryAsync(routeName.ToLower());
                }

                if (!System.IO.File.Exists(rssDataFile))
                {
                    return NotFound();
                }
            }

            if (System.IO.File.Exists(rssDataFile))
            {
                return PhysicalFile(rssDataFile, "text/xml");
            }

            return NotFound();
        }

        [Route("atom")]
        public async Task<IActionResult> Atom()
        {
            var atomDataFile = Path.Join($"{SiteDataDirectory}", "feed", "posts-atom.xml");
            if (!System.IO.File.Exists(atomDataFile))
            {
                Logger.LogInformation($"Atom file not found, writing new file on {atomDataFile}");

                await _syndicationFeedService.RefreshFeedFileAsync(true);

                if (!System.IO.File.Exists(atomDataFile))
                {
                    return NotFound();
                }
            }

            if (System.IO.File.Exists(atomDataFile))
            {
                return PhysicalFile(atomDataFile, "text/xml");
            }

            return NotFound();
        }
    }
}