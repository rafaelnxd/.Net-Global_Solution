using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace HeatWatch.API.Helpers
{
    public static class HateoasHelper
    {
        public static IEnumerable<object> Links(this IUrlHelper urlHelper, string routeName, object routeValues)
        {
            if (urlHelper == null) yield break;
            var href = urlHelper.Link(routeName, routeValues);
            if (href != null)
            {
                yield return new
                {
                    rel = "self",
                    href = href
                };
            }
        }
    }
}
