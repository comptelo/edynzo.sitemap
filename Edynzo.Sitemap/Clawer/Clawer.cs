using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Edynzo.Sitemap.Clawer
{
    /// <summary>
    /// Simple recursive web crawler that collects links starting from a base URL.
    /// </summary>
    /// <remarks>
    /// This class performs a depth-limited, single-threaded crawl starting from <see cref="BaseUrl"/>.
    /// It stores visited absolute URLs in <see cref="VisitedUrls"/> to avoid duplicates and uses
    /// <see cref="HtmlAnalyzer"/> to extract links from pages.
    ///
    /// Limitations and notes:
    /// - This implementation is not thread-safe.
    /// - Depth is tracked with <see cref="CurrentDepth"/>; after a complete run it should return to 0.
    /// - Exceptions thrown while fetching or parsing pages are wrapped and re-thrown to provide context.
    /// - Public fields are exposed for easier testing; consider converting them to properties or making them private for production.
    /// </remarks>
    public class Clawer
    {
        /// <summary>
        /// The base URL where the crawl starts. Also used to restrict which links are followed.
        /// Expected to be an absolute URL (e.g. "https://example.com").
        /// </summary>
        public string BaseUrl;

        /// <summary>
        /// List of absolute URLs that have been visited by the crawler.
        /// </summary>
        public List<string> VisitedUrls;

        /// <summary>
        /// Maximum allowed recursion depth (number of levels from <see cref="BaseUrl"/>).
        /// </summary>
        public int MaxDepth;

        /// <summary>
        /// Current recursion depth during crawling. It is incremented when entering a page and decremented after processing it.
        /// After a complete crawl this value should be 0.
        /// </summary>
        public int CurrentDepth;

        /// <summary>
        /// Creates a new instance of <see cref="Clawer"/>.
        /// </summary>
        /// <param name="baseUrl">Starting absolute URL for crawling.</param>
        /// <param name="maxDepth">Maximum recursion depth (default is 3).</param>
        public Clawer(string baseUrl, int maxDepth = 3)
        {
            BaseUrl = baseUrl;
            MaxDepth = maxDepth;
            VisitedUrls = new List<string>();
            CurrentDepth = 0;
        }

        /// <summary>
        /// Starts crawling from <see cref="BaseUrl"/>.
        /// </summary>
        /// <remarks>
        /// This method wraps the internal recursive call in a try/catch to add context to exceptions.
        /// Any exception thrown during crawling is re-thrown as an <see cref="Exception"/> with the original
        /// exception set as <c>innerException</c>.
        /// </remarks>
        public void StartClawing()
        {
            try
            {
                Claw(BaseUrl);
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to crawl page", innerException: ex);
            }
        }


        /// <summary>
        /// Internal recursive method that processes a single URL and then continues to discovered links.
        /// </summary>
        /// <param name="url">Absolute URL of the page to process.</param>
        /// <remarks>
        /// Behavior:
        /// - If <see cref="CurrentDepth"/> has reached <see cref="MaxDepth"/> or the URL is already present in
        ///   <see cref="VisitedUrls"/>, the method returns immediately.
        /// - The URL is added to <see cref="VisitedUrls"/> and <see cref="CurrentDepth"/> is incremented.
        /// - Links are obtained using <see cref="HtmlAnalyzer.LinkOnPage(string)"/> and filtered so that only links
        ///   containing <see cref="BaseUrl"/> and not yet visited are followed.
        /// - For each allowed link the method calls itself recursively.
        ///
        /// Exceptions:
        /// - Exceptions thrown by <see cref="HtmlAnalyzer"/> are logged to the console, <see cref="CurrentDepth"/>
        ///   is decremented, and the exception is wrapped and re-thrown to the caller.
        ///
        /// Implementation notes:
        /// - The filter uses string containment (link.Contains(BaseUrl)); consider using URI host comparison for stricter domain checks.
        /// - The method uses synchronous operations and is therefore blocking.
        /// </remarks>
        private void Claw(string url)
        {
            if (CurrentDepth >= MaxDepth || VisitedUrls.Contains(url))
            {
                return;
            }
            VisitedUrls.Add(url);
            CurrentDepth++;
            // Simulate fetching and parsing the page
            Console.WriteLine($"Clawing URL: {url} at depth {CurrentDepth}");
            // Here you would add logic to fetch the page content and extract links
            // For demonstration, we'll simulate finding two new links

            List<string> links = new List<string>();
            HtmlAnalyzer analyzer = new HtmlAnalyzer();
            try
            {
                links = analyzer.LinkOnPage(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching links from {url}: {ex.Message}");
                CurrentDepth--;
                throw new Exception($"Failed to fetch links from {url}", ex);
            }


            var newLinks = links.Where(link => !VisitedUrls.Contains(link) && link.Contains(BaseUrl)).ToList();

            foreach (var link in newLinks)
            {
                Console.WriteLine($"Found link: {link}");
            }

            foreach (var link in newLinks)
            {
                Claw(link);
            }
            CurrentDepth--;
        }
    }
}
