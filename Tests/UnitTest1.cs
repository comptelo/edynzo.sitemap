using Edynzo.Sitemap.Clawer;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ClawlerSetup()
        {
            Clawer clawer = new Clawer("https://kanavys.oznuk.cz", 10);
            Assert.That(clawer.VisitedUrls.Count == 0, "should be 0");
            Assert.That(clawer.MaxDepth == 10);
            Assert.That(clawer.BaseUrl == "https://kanavys.oznuk.cz");
            Assert.That(clawer.CurrentDepth == 0);
        }

        [Test]
        public void ClawlerStart()
        {
            Clawer clawer = new Clawer("https://kanavys.oznuk.cz", 2);
            clawer.StartClawing();
            Assert.That(clawer.VisitedUrls.Count > 0, "should be more than 0");
            Assert.That(clawer.CurrentDepth == 0);
            Assert.That(clawer.VisitedUrls.Contains("https://kanavys.oznuk.cz"));
        }

        [Test]
        public void ClawlerMaxDepth()
        {
            Clawer clawer = new Clawer("https://kanavys.oznuk.cz", 1);
            clawer.StartClawing();
            Assert.That(clawer.VisitedUrls.Count > 0, "should be more than 0");
            Assert.That(clawer.CurrentDepth == 0);
            Assert.That(clawer.VisitedUrls.Contains("https://kanavys.oznuk.cz"));
            // Since max depth is 1, it should not contain links other than the base URL
            Assert.That(clawer.VisitedUrls.Count == 1, "should be exactly 1 due to max depth");
        }

        [Test]
        public void ClawlerNoDuplicates()
        {
            Clawer clawer = new Clawer("https://kanavys.oznuk.cz", 2);
            clawer.StartClawing();
            Assert.That(clawer.VisitedUrls.Count > 0, "should be more than 0");
            Assert.That(clawer.CurrentDepth == 0);
            Assert.That(clawer.VisitedUrls.Contains("https://kanavys.oznuk.cz"));
            var distinctUrls = clawer.VisitedUrls.Distinct().ToList();
            Assert.That(clawer.VisitedUrls.Count == distinctUrls.Count, "should be no duplicates");

        }

        [Test]
        public void ClawlerInvalidUrl()
        {
            Clawer clawer = new Clawer("https://thisurldoesnotexist.tld", 2);
            Assert.Throws<Exception>(() => clawer.StartClawing());
        }

        [Test]
        public void HTMLAnalyzerLinks()
        {
            HtmlAnalyzer analyzer = new HtmlAnalyzer();
            var links = analyzer.LinkOnPage("https://kanavys.oznuk.cz");
            Assert.That(links.Count > 0, "should find some links");
        }
    }
}
