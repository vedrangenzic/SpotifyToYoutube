using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading.Tasks;
using SpotifyToYoutube.Models;

namespace SpotifyToYoutube.Services
{
    public class SpotifyService : ISpotifyService
    {

        HttpClient client = new HttpClient();
        FinalGeneratedClass finalResponse = new FinalGeneratedClass();

        public List<Models.Track> GetTracks(string token, string id)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var p = new Models.Playlist
            {
                Tracks = new List<Models.Track>()
            };
            p.HttpResponseMessage = client.GetAsync($"https://api.spotify.com/v1/playlists/{id}/tracks").Result;
            p.ResponsetContent = p.HttpResponseMessage.Content;
            p.ResponseString = p.ResponsetContent.ReadAsStringAsync().Result;
            finalResponse = FinalGeneratedClass.FromJson(p.ResponseString);
            for (int i = 0; i < finalResponse.Total; i++)
            {
                Models.Track t = new Models.Track
                {
                    Name = finalResponse.Items[i].Track.Name,
                    Artist = finalResponse.Items[i].Track.Artists[0].Name

                };
                p.Tracks.Add(t);
            };

            return p.Tracks;
        }
    }
}
