namespace AnimeciBackend.Data.Objects.Kitsu.Anime
{
    public class Data
    {
        public string id { get; set; }
        public string type { get; set; }
        public Links links { get; set; }
        public Attributes attributes { get; set; }
        public Relationships relationships { get; set; }
    }

    public class AnimeObject
    {
        public Data data { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
        public string related { get; set; }
    }

    public class Titles
    {
        public string en { get; set; }
        public string en_jp { get; set; }
        public string ja_jp { get; set; }
    }

    public class PosterImage
    {
        public string tiny { get; set; }
        public string small { get; set; }
        public string medium { get; set; }
        public string large { get; set; }
        public string original { get; set; }
    }

    public class CoverImage
    {
        public string small { get; set; }
        public string large { get; set; }
        public string original { get; set; }
    }

    public class Attributes
    {
        public string slug { get; set; }
        public string synopsis { get; set; }
        public int coverImageTopOffset { get; set; }
        public Titles titles { get; set; }
        public string canonicalTitle { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public PosterImage posterImage { get; set; }
        public CoverImage coverImage { get; set; }
        public int? episodeCount { get; set; }
        public int? episodeLength { get; set; }
        public string showType { get; set; }
        public string youtubeVideoId { get; set; }
        public string ageRating { get; set; }
        public string ageRatingGuide { get; set; }
        public bool nsfw { get; set; }
    }

    public class Genres
    {
        public Links links { get; set; }
    }


    public class Castings
    {
        public Links links { get; set; }
    }

    public class Installments
    {
        public Links links { get; set; }
    }

    public class Mappings
    {
        public Links links { get; set; }
    }


    public class Reviews
    {
        public Links links { get; set; }
    }


    public class Episodes
    {
        public Links links { get; set; }
    }

    public class StreamingLinks
    {
        public Links links { get; set; }
    }

    public class Relationships
    {
        public Genres genres { get; set; }
        public Castings castings { get; set; }
        public Installments installments { get; set; }
        public Mappings mappings { get; set; }
        public Reviews reviews { get; set; }
        public Episodes episodes { get; set; }
        public StreamingLinks streamingLinks { get; set; }
    }
}