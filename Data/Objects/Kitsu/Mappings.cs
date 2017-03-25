using System.Collections.Generic;

namespace AnimeciBackend.Data.Objects.Kitsu.Mappings
{
    public class Attributes
    {
        public string externalSite { get; set; }
        public string externalId { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
        public string related { get; set; }
    }

    public class Media
    {
        public Links links { get; set; }
    }

    public class Relationships
    {
        public Media media { get; set; }
    }

    public class Datum
    {
        public string id { get; set; }
        public string type { get; set; }
        public Links links { get; set; }
        public Attributes attributes { get; set; }
        public Relationships relationships { get; set; }
    }

    public class MappingResults
    {
        public List<Datum> data { get; set; }
        public Links links { get; set; }
    }
}