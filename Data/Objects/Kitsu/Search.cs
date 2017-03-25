using System.Collections.Generic;
using AnimeciBackend.Data.Objects.Kitsu.Anime;

namespace AnimeciBackend.Data.Objects.Kitsu.Search
{
    public class Datum
    {
        public string id { get; set; }
        public string type { get; set; }
        public Links links { get; set; }
        public Attributes attributes { get; set; }
        public Relationships relationships { get; set; }
    }

    public class Pager
    {
        public string first { get; set; }
        public string last { get; set; }
    }

    public class SearchResults
    {
        public List<Datum> data { get; set; }
        public Pager links { get; set; }
    }

}