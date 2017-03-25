
using System.Collections.Generic;

namespace AnimeciBackend.Data.Objects.Atarashii
{
    public class OtherTitles
    {
        public List<string> english { get; set; }
        public List<string> synonyms { get; set; }
        public List<string> japanese { get; set; }
    }

    public class MangaAdaptation
    {
        public int manga_id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }

    public class MalObject
    {
        public int anime_id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Recommendation
    {
        public int id { get; set; }
        public string title { get; set; }
        public string image_url { get; set; }
    }

    public class AnimeDetail
    {
        public int id { get; set; }
        public string title { get; set; }
        public string preview { get; set; }
        public OtherTitles other_titles { get; set; }
        public int rank { get; set; }
        public int popularity_rank { get; set; }
        public string image_url { get; set; }
        public string type { get; set; }
        public int episodes { get; set; }
        public string status { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public int duration { get; set; }
        public string classification { get; set; }
        public double members_score { get; set; }
        public int members_count { get; set; }
        public int favorited_count { get; set; }
        public List<object> external_links { get; set; }
        public string synopsis { get; set; }
        public string background { get; set; }
        public List<string> producers { get; set; }
        public List<string> genres { get; set; }
        public List<MangaAdaptation> manga_adaptations { get; set; }
        public List<object> prequels { get; set; }
        public List<MalObject> sequels { get; set; }
        public List<MalObject> side_stories { get; set; }
        public MalObject parent_story { get; set; }
        public List<MalObject> character_anime { get; set; }
        public List<MalObject> spin_offs { get; set; }
        public List<object> summaries { get; set; }
        public List<object> alternative_versions { get; set; }
        public List<object> other { get; set; }
        public List<string> opening_theme { get; set; }
        public List<string> ending_theme { get; set; }
        public List<Recommendation> recommendations { get; set; }
    }
}