# TestConsole

This is a minimal console application that demonstrates how to use the `Edynzo.Sitemap` library to crawl a website and generate a `sitemap.xml` file.

How it works

- The program asks for a website URL on standard input.
- It instantiates `Edynzo.Sitemap.Generator.Generator` with the provided URL and calls `CrawlAndGenerateSitemap(maxDepth)`.
- The crawler visits pages on the same host and the generator writes `sitemap.xml` in the current working directory.

Run the demo

From repository root:

    dotnet run --project TestConsole/TestConsole.csproj

Enter a valid URL when prompted (for example `https://example.com`).

Notes

- Because the HTML parsing is simple, some pages may not yield any links or may produce incorrect results on complex HTML.
- Consider running the sample against a small static site or local test pages to observe expected results.

*** End ***