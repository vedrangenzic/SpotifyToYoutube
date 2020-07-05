using System.Collections.Generic;

namespace SpotifyToYoutube.Services
{
    public interface ISpotifyService
    {
        List<Models.Track> GetTracks(string token, string id);
        
    }
}