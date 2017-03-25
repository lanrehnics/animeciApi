namespace AnimeciBackend.Data.Objects.Atarashii
{
    public class EpisodeDetail
    {
        public int number { get; set; }
        public string title { get; set; }
        public OtherTitles other_titles { get; set; }
        public string air_date { get; set; }
    }
}