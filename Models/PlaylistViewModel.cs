using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyToYoutube.Models
{
    public class PlaylistViewModel
    {
        public List<Playlist> Playlists { get; set; }
        public string Token { get; set; }
    }
}
