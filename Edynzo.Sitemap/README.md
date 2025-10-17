# Edynzo.Sitemap (library)

This folder contains the Edynzo.Sitemap library: a small set of classes to crawl a website and generate an XML sitemap.

Core components

- `Clawer/Clawer.cs` - a depth-limited crawler which records visited URLs starting from the base URL.
- `Clawer/HtmlAnalyzer.cs` - primitive HTML downloader and extractor that returns anchor (`<a>`) hrefs found on a page.
- `Generator/Generator.cs` - builds `sitemap.xml` from a list of links or from article metadata (`Models/ArticlesForClawer`).
- `Models/` - contains simple model types used by the generator.
- `Intarfeces/` - contains interface skeletons (note: interfaces are declared internal in current codebase).

Usage (programmatic)

- Create an instance of `Generator` with a base URL and call `CrawlAndGenerateSitemap(maxDepth)` to crawl the site and write `sitemap.xml`.
- Alternatively, construct `Generator` with a list of `ArticlesForClawer` and call `GenerateSitemap(true)` to produce a sitemap using those items.

Example (C#):

```csharp
var generator = new Edynzo.Sitemap.Generator.Generator("https://example.com");
generator.CrawlAndGenerateSitemap(3);
```

Design notes and limitations

- HTML parsing is implemented with string scanning and is not robust. For production, switch to an HTML parser library such as AngleSharp or HtmlAgilityPack.
- Networking operations are synchronous and use `.Result`. Convert to async (`HttpClient.GetAsync` / `ReadAsStringAsync`) and propagate async methods to avoid thread blocking.
- There is no respect for `robots.txt`, rate limiting, or politeness features. The crawler is intended as an educational/demo implementation.

Contributing

If you plan to improve the library, consider the following low-risk improvements:

- Replace `HtmlAnalyzer` with an implementation using AngleSharp.
- Make crawling asynchronous and add cancellation tokens.
- Add unit tests for link extraction and sitemap generation.

*** End ***