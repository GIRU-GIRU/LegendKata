using Newtonsoft.Json;

namespace LegendKata.Models
{
    public class SearchEngineModel
    {
        public string SearchCriteria { get; set; }

        public bool StrictSearch { get; set; }
        public List<Artist> Artists { get; set; }
    }

    public class Artist
    {
        public string Name { get; set; }
        public List<Song> Songs { get; set; }
    }
    public class Song
    {
        public string Name { get; set; }
    }


}
