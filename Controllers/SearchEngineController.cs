using Microsoft.AspNetCore.Mvc;
using LegendKata.Models;
using MetaBrainz.MusicBrainz;
using MetaBrainz.MusicBrainz.Interfaces.Searches;
using MetaBrainz.MusicBrainz.Interfaces.Entities;

namespace LegendKata.Controllers
{
    public class SearchEngineController : Controller
    {
        public SearchEngineController(ILogger<SearchEngineController> logger)
        {
            _logger = logger;
        }
        private ILogger _logger;
        private SearchEngineModel _searchEngineModel = new();
        public async Task<IActionResult> SearchEngine(SearchEngineModel data)
        {
            if (data != null)
            {
                _searchEngineModel.SearchCriteria = data.SearchCriteria.Trim();
                _searchEngineModel.StrictSearch = data.StrictSearch;
            }

            if (!string.IsNullOrWhiteSpace(_searchEngineModel.SearchCriteria))
            {
                await PopulateModel();
            }
            

            return View(_searchEngineModel);
        }

        private async Task PopulateModel()
        {
            try
            {
                var q = new Query("RedStapler", "19.99", "mailto:milton.waddams@initech.com");

                //  new Query("Legend Kata", "1.0", "girugiru100x@gmail.com");
                var artistsQuery = await q.FindArtistsAsync(_searchEngineModel.SearchCriteria);

                List<Artist> artists = new();

                foreach (var artist in artistsQuery.Results)
                {
                    if (CheckToPerformStrictSearch(artist.Item.Name))
                    {
                        artists.Add(new Artist()
                        {
                            Name = artist?.Item?.Name,
                            Songs = await GetSongsForArtist(artist, q)

                        });
                    }
                }

                _searchEngineModel.Artists = artists;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred retrieving the artists for the criteria {_searchEngineModel?.SearchCriteria}", ex);

            }
        }

        private bool CheckToPerformStrictSearch(string artistName)
        {
            if (_searchEngineModel.StrictSearch == false) return true;

            return _searchEngineModel.SearchCriteria.ToLower() == artistName.ToLower();
        }

        private async Task<List<Song>> GetSongsForArtist(ISearchResult<IArtist> artist, Query q)
        {
            List<Song> songs = new();
            try
            {
                var songsQuery = await q.BrowseArtistReleasesAsync(artist.Item.Id);

                foreach (var song in songsQuery.Results)
                {
                    songs.Add(new Song()
                    {
                        Name = song?.Title
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred retrieving songs for {artist.Item.Name}", ex);
            }

            return songs;
        }

    }
}
