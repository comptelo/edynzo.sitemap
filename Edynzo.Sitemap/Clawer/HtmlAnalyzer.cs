using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Edynzo.Sitemap.Clawer
{
    /// <summary>
    /// A simple helper class for HTML page analysis and extraction of links (&lt;a&gt; elements).
    /// </summary>
    /// <remarks>
    /// HtmlAnalyzer provides methods to download the HTML content of a page, find &lt;a&gt; elements
    /// and extract the values of their href attributes. The output contains absolute URLs where possible
    /// and duplicate URLs are removed.
    /// Note: This is a very basic string-based parser intended only for simple use cases.
    /// For robust HTML parsing consider using a proper HTML parser (e.g. AngleSharp or HtmlAgilityPack).
    /// </remarks>
    public class HtmlAnalyzer
    {
        /// <summary>
        /// Downloads the HTML content of the specified URL.
        /// </summary>
        /// <param name="url">The URL of the page to download.</param>
        /// <returns>
        /// Returns the HTML content as a string. If reading the content fails, the method returns an empty string
        /// and prints an error message to the console.
        /// </returns>
        /// <remarks>
        /// This method uses synchronous calls HttpClient.GetAsync(...).Result and ReadAsStringAsync().Result,
        /// which block the calling thread. In production code prefer using the asynchronous variants.
        /// </remarks>
        private string GetPage(string url)
        {
            HttpClient wc = new HttpClient();
            var response = wc.GetAsync(url).Result;
            try
            {
                string html = response.Content.ReadAsStringAsync().Result;
                return html;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching page {url}: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Extracts and returns a list of distinct hyperlinks (href values) from the HTML content of the specified page.
        /// </summary>
        /// <param name="BaseUrl">The base URL of the page used to resolve relative links to absolute URLs.</param>
        /// <returns>
        /// A list of distinct absolute URLs found in href attributes of &lt;a&gt; elements.
        /// Returned links include absolute HTTP/HTTPS URLs and relative links converted to absolute form.
        /// </returns>
        /// <remarks>
        /// The method:
        /// - calls GetAElements to obtain raw &lt;a&gt; elements (as strings),
        /// - extracts the href attribute value (between double quotes) from each element,
        /// - keeps only valid HTTP/HTTPS links or relative paths starting with "/" or "./",
        /// - converts relative paths to absolute URLs using the provided base URL (scheme + host + path),
        /// - returns only unique URLs.
        ///
        /// This approach does not handle more complex href forms (e.g. attributes without quotes, javascript: links,
        /// data: schemes, or unusual HTML structures). For more comprehensive handling use an HTML parser.
        /// </remarks>
        public List<string> LinkOnPage(string BaseUrl)
        {
            List<string> links = GetAElements(BaseUrl);
            List<string> result = new List<string>();
            foreach (string link in links)
            {
                int hrefPos = link.ToLower().IndexOf("href=");
                if (hrefPos > -1)
                {
                    int startQuote = link.IndexOf('"', hrefPos);
                    int endQuote = link.IndexOf('"', startQuote + 1);
                    string href = link.Substring(startQuote + 1, endQuote - startQuote - 1);
                    if (href.StartsWith("http") || href.StartsWith("https"))
                    {
                        result.Add(href);
                    }
                    else if (href.StartsWith("/") || href.StartsWith("./"))
                    {
                        if(href.StartsWith("./"))
                        {
                            href = href.Substring(1);
                        }
                        Uri baseUri = new Uri(BaseUrl);
                        string fullUrl = baseUri.Scheme + "://" + baseUri.Host + href;
                        result.Add(fullUrl);
                    }
                }
            }
            return result.Distinct().ToList();
        }


        /// <summary>
        /// Returns a list of raw &lt;a&gt; elements found in the HTML as strings.
        /// </summary>
        /// <param name="BaseURL">The URL of the page whose HTML will be parsed for &lt;a&gt; elements.</param>
        /// <returns>List of strings representing found &lt;a&gt; opening tags including their attributes.</returns>
        /// <remarks>
        /// This implementation performs a very simple scan of the source HTML:
        /// - iterates characters and looks for a '<' followed by 'a' (lowercase is expected),
        /// - then finds the nearest '>' and returns the substring from '<' to '>' (inclusive).
        ///
        /// Limitations:
        /// - Does not handle multi-line attributes, comments, scripts, or nested structures.
        /// - May fail on invalid or complex HTML.
        /// </remarks>
        private List<string> GetAElements(string BaseURL)
        {
            string html = GetPage(BaseURL);
            List<string> links = new List<string>();
            int pos = 0;
            foreach (char c in html)
            {
                if (c == '<')
                {
                    if (html.Substring(pos, 2).ToLower() == "<a")
                    {
                        int endPos = html.IndexOf(">", pos);
                        string aElement = html.Substring(pos, endPos - pos + 1);
                        links.Add(aElement);
                    }
                }
                pos++;
            }
            return links;
        }
    }
}
