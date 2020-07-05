using System.Collections.Generic;
using System.Net.Http;

namespace SpotifyToYoutube.Models
{
    public class Playlist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }
        public List<Track> Tracks { get; set; }
        public HttpContent ResponsetContent { get; set; }
        public string ResponseString { get; set; }
    }
}
