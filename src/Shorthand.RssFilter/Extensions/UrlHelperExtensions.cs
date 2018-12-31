using Microsoft.AspNetCore.Mvc;

namespace Shorthand.RssFilter.Extensions {
    public static class UrlHelperExtensions {
        public static string AbsoluteAction(
            this IUrlHelper url,
            string actionName,
            string controllerName,
            object routeValues = null) {
            return url.Action(actionName, controllerName, routeValues, url.ActionContext.HttpContext.Request.Scheme);
        }
    }
}