# Edynzo.Sitemap

Edynzo.Sitemap is a small open-source .NET library and demo console application for crawling a website and generating an XML sitemap (`sitemap.xml`).

This repository contains:

- `Edynzo.Sitemap/` - The main library containing a simple crawler, HTML analyzer and sitemap generator.
- `TestConsole/` - A minimal console application demonstrating how to run the crawler and generator.
- `Tests/` - Unit test project skeleton.

## Quick start

Requirements: .NET SDK (6.0+). The compiled output in the workspace targets a recent .NET TFM — use the SDK you have installed.

Build the solution:

	dotnet build Edynzo.Sitemap.sln

Run the sample console (from the repository root):

	dotnet run --project TestConsole/TestConsole.csproj

When the console starts, enter a URL (for example `https://example.com`) and the program will attempt to crawl links on the host and produce a `sitemap.xml` file in the current working directory.

## Project layout

- `Edynzo.Sitemap/Clawer` - `Clawer.cs` and `HtmlAnalyzer.cs`: a simple depth-limited crawler and a very small HTML link extractor.
- `Edynzo.Sitemap/Generator` - `Generator.cs`: builds a sitemap XML using links collected by the crawler or using article metadata.
- `Edynzo.Sitemap/Models` - model classes (e.g., `ArticlesForClawer`).
- `TestConsole` - example program that reads a URL and generates a sitemap.

## Notes and limitations

- The HTML parsing is implemented with string operations and is not robust for real-world HTML; consider using an HTML parser (AngleSharp / HtmlAgilityPack) for production use.
- `HtmlAnalyzer` performs synchronous HTTP calls using `HttpClient.Result` which can block threads; convert to async/await for better scalability.
- The library is intended as a simple example/demo and not a production-ready crawler (no robots.txt handling, rate limiting, concurrency, or error recovery).

## Next steps (suggestions)

- Improve link extraction by using a proper HTML parser.
- Make `HtmlAnalyzer` and `Clawer` asynchronous and add cancellation support.
- Add unit tests for crawler behavior and generator output.

License: see `LICENSE.md` in the repository root.
