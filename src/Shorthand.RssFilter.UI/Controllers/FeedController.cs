using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shorthand.RssFilter.Contracts;
using Shorthand.RssFilter.Extensions;
using Shorthand.RssFilter.Models;

namespace Shorthand.RssFilter.UI.Controllers {
    [Route("api/rss")]
    [ApiController]
    public class RssController : ControllerBase {
        private readonly ApplicationConfiguration _configuration;
        private readonly IRssService _rssService;
        private readonly IUrlHelper _urlHelper;

        public RssController(IOptions<ApplicationConfiguration> configuration,
                             IRssService rssService,
                             IUrlHelper urlHelper) {
            _configuration = configuration.Value;
            _rssService = rssService;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        public ActionResult GetFeeds() {
            var result = _configuration.Select(kvp => new { 
                name = kvp.Key,
                feedUrl = _urlHelper.AbsoluteAction("GetFilteredFeed", "Rss", new { name = kvp.Key }),
                sourceUrl = kvp.Value.Uri
            });

            return new JsonResult(result);
        }

        [HttpGet("{name}")]
        public ActionResult<string> GetFeedInformation(string name) {
            if(!_configuration.TryGetValue(name, out var feedConfiguration))
                return NotFound();

            return new JsonResult(feedConfiguration);
        }

        [HttpGet("{name}/feed")]
        public async Task<ActionResult<string>> GetFilteredFeed(string name) {
            if(!_configuration.TryGetValue(name, out var feedConfiguration))
                return NotFound();

            var document = await _rssService.GetFilteredFeed(feedConfiguration);
            
            return Content(document.ToString(), "application/rss+xml", Encoding.UTF8);
        }
    }
}
