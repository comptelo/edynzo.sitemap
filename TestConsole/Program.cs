using Edynzo.Sitemap.Clawer;
using Edynzo.Sitemap.Generator;


internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting Clawer and Sitemap Generator...");
        Console.WriteLine("=========================================");
        Console.WriteLine("Write your website URL and max depth in the code to test.");
        string urlString = Console.ReadLine();
        Generator generator = new Generator(urlString);
        generator.CrawlAndGenerateSitemap(10);
    }
}