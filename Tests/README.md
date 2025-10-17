# Tests

This project contains unit tests for the Edynzo.Sitemap solution. To run tests:

    dotnet test Tests/Tests.csproj

If tests are not present or are a skeleton, add new tests under `Tests/` using your preferred test framework (the project uses the default test SDK for .NET projects).

Suggested tests to add:

- Link extraction tests for `HtmlAnalyzer.LinkOnPage` using sample HTML.
- Sitemap generation tests for `Generator.GenerateSitemap` verifying `sitemap.xml` content.
- Integration test that runs the crawler on a set of local HTML files.

*** End ***