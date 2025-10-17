using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edynzo.Sitemap.Intarfeces
{
    internal interface IHtmlAnalyzer
    {

        /// <summary>
        /// Get HTML page from url
        /// </summary>
        /// <param name="url">url address of page</param>
        /// <returns></returns>
        string GetPage(string url);

        /// <summary>
        /// Extracts and returns a list of distinct hyperlinks from the HTML content of the specified web page.
        /// </summary>
        /// <remarks>This method retrieves all anchor elements from the specified web page, extracts their
        /// "href" attributes, and resolves relative URLs using the provided <paramref name="BaseUrl"/>. Only valid HTTP
        /// and HTTPS links are included in the result.</remarks>
        /// <param name="BaseUrl">The base URL of the web page to analyze. This URL is used to resolve relative links.</param>
        /// <returns>A list of distinct hyperlinks found on the page. The links are returned as absolute URLs, including both
        /// fully qualified URLs (e.g., starting with "http" or "https") and resolved relative URLs.</returns>
        List<string> LinkOnPage(string BaseUrl);
    }
}
