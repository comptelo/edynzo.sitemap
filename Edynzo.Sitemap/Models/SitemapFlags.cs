using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Edynzo.Sitemap.Models
{
    public class SitemapFlags
    {
        public enum ChangeFrequency
        {
            Always,
            Hourly,
            Daily,
            Weekly,
            Monthly,
            Yearly,
            Never
        }

        Dictionary<string, float> Priority = new Dictionary<string, float>()
        {
            { "Always", 1.0f },
            { "Hourly", 0.9f },
            { "Daily", 0.8f },
            { "Weekly", 0.7f },
            { "Monthly", 0.6f },
            { "Yearly", 0.5f },
            { "Never", 0.4f }
        };
    }
}
