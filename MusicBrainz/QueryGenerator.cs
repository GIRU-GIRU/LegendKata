using MetaBrainz.MusicBrainz;

namespace LegendKata.MusicBrainz
{
    public static class QueryGenerator
    {
        public static Query GetQuery()
        {
            return new Query("LegendKata", "1.0", "mailto:girugiru100x@gmail.com");
        }
    }
}
