using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shorthand.RssFilter.Contracts;
using Shorthand.RssFilter.Models;

namespace Shorthand.RssFilter.Controllers {
    [Route("api/feeds")]
    [ApiController]
    public class ApiController : ControllerBase {
        private readonly ApplicationConfiguration _configuration;
        private readonly IRssService _rssService;
        private readonly IUrlHelper _urlHelper;

        public ApiController(IOptions<ApplicationConfiguration> configuration,
                             IRssService rssService) {
            _configuration = configuration.Value;
            _rssService = rssService;
        }

        [HttpGet]
        public ActionResult GetFeeds() {
            var result = _configuration.Select(kvp => new { 
                name = kvp.Key,
                configuration = kvp.Value
            });

            return new JsonResult(result);
        }

        [HttpGet("{name}")]
        public ActionResult<string> GetFeedInformation(string name) {
            if(!_configuration.TryGetValue(name, out var feedConfiguration))
                return NotFound();

            return new JsonResult(feedConfiguration);
        }
    }
}
