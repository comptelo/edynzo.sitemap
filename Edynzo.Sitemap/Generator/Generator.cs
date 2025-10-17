using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Edynzo.Sitemap.Models;

namespace Edynzo.Sitemap.Generator
{
    public class Generator
    {
        private string _baseUrl;
        private string _sitemapPath = "sitemap.xml";
        private List<string> _links = new List<string>();
        private List<ArticlesForClawer> _articlesForClawers = new List<ArticlesForClawer>();

        public Generator(string baseUrl, string sitemapPath, List<ArticlesForClawer> articlesForClawers, List<string> links) : this(baseUrl)
        {
            _sitemapPath = sitemapPath;
            _articlesForClawers = articlesForClawers;
        }

        public Generator(string baseUrl)
        {
            _baseUrl = baseUrl;
        }
        public void GenerateSitemap(bool useArticles)
        {
            if (_articlesForClawers == null || !_articlesForClawers.Any() || !useArticles)
            {
                if (_links == null || !_links.Any())
                {
                    throw new InvalidOperationException("No links available to generate sitemap.");
                }
                StringBuilder sitemapBuilder = new StringBuilder();
                sitemapBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sitemapBuilder.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
                foreach (var link in _links)
                {
                    sitemapBuilder.AppendLine("  <url>");
                    sitemapBuilder.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(link)}</loc>");
                    sitemapBuilder.AppendLine($"    <lastmod>{DateTime.UtcNow.ToString("yyyy-MM-dd")}</lastmod>");
                    sitemapBuilder.AppendLine("    <changefreq>monthly</changefreq>");
                    sitemapBuilder.AppendLine("    <priority>0.8</priority>");
                    sitemapBuilder.AppendLine("  </url>");
                }
                sitemapBuilder.AppendLine("</urlset>");

                try
                {
                    System.IO.File.WriteAllText(_sitemapPath, sitemapBuilder.ToString(), new System.Text.UTF8Encoding(true));
                    Console.WriteLine($"Sitemap generated at {_sitemapPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating sitemap: {ex.Message}");
                }
            }
            else
            {
                StringBuilder sitemapBuilder = new StringBuilder();
                sitemapBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sitemapBuilder.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
                foreach (var article in _articlesForClawers)
                {
                    sitemapBuilder.AppendLine("  <url>");
                    sitemapBuilder.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(article.Url)}</loc>");
                    sitemapBuilder.AppendLine($"    <lastmod>{article.LastModified.ToString("yyyy-MM-dd")}</lastmod>");
                    sitemapBuilder.AppendLine("    <changefreq>monthly</changefreq>");
                    sitemapBuilder.AppendLine("    <priority>0.8</priority>");
                    sitemapBuilder.AppendLine("  </url>");
                }

                sitemapBuilder.AppendLine("</urlset>");

                try
                {
                    System.IO.File.WriteAllText(_sitemapPath, sitemapBuilder.ToString());
                    Console.WriteLine($"Sitemap generated at {_sitemapPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing sitemap to {_sitemapPath}: {ex.Message}");
                    throw;
                }
            }
        }


        public void CrawlAndGenerateSitemap(int maxDepth = 3)
        {
            Clawer.Clawer clawer = new Clawer.Clawer(_baseUrl, maxDepth);
            clawer.StartClawing();
            _links = clawer.VisitedUrls;
            GenerateSitemap(false);
        }
    }
}
