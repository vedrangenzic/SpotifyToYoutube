using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SpotifyToYoutube.Models;

namespace SpotifyToYoutube.Controllers
{
    class SpotifyAuthentication
    {
        public string clientID = "CLIENT_ID";
        public string clientSecret = "CLIENT_SECRET";
        public string redirectURL = "https://localhost:44340/callback";
    }

    [Route("")]
    public class SpotifyController : Controller
    {
        SpotifyAuthentication spotifyAuth = new SpotifyAuthentication();
        FinalGeneratedClass finalResponse = new FinalGeneratedClass();
        string responseString = "";

        [HttpGet]
        public IActionResult Get()
        {
            var qb = new QueryBuilder
            {
                { "response_type", "code" },
                { "client_id", spotifyAuth.clientID },
                { "scope", "playlist-read-private" },
                { "redirect_uri", spotifyAuth.redirectURL }
            };

            return Redirect("https://accounts.spotify.com/authorize/" + qb.ToQueryString().ToString());

        }
        [Route("/callback")]
        public IActionResult Get(string code)
        {
            List<Playlist> PlaylistList = new List<Playlist>();
            string token = "";

            if (code.Length > 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(spotifyAuth.clientID + ":" + spotifyAuth.clientSecret)));

                    FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("code", code),
                        new KeyValuePair<string, string>("redirect_uri", spotifyAuth.redirectURL),
                        new KeyValuePair<string, string>("grant_type", "authorization_code"),
                    });

                    var response = client.PostAsync("https://accounts.spotify.com/api/token", formContent).Result;

                    var responseContent = response.Content;
                    responseString = responseContent.ReadAsStringAsync().Result;

                    var json = JsonSerializer.Deserialize<AccessToken>(responseString);

                    token = json.access_token;

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var res = client.GetAsync("https://api.spotify.com/v1/me/playlists").Result;

                    var resCont = res.Content;
                    responseString = resCont.ReadAsStringAsync().Result;

                    var playlistsJson = FinalGeneratedClass.FromJson(responseString);
                    // get playlists from json and store them in list
                    for (int i = 0; i < playlistsJson.Total; i++)
                    {
                        var playlist = new Playlist
                        {
                            Id = playlistsJson.Items[i].Id,
                            Name = playlistsJson.Items[i].Name,
                            Tracks = new List<Models.Track>()
                        };

                        PlaylistList.Add(playlist);

                    }

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    // read all tracks from each playlist from list
                    foreach (var p in PlaylistList)
                    {
                        p.HttpResponseMessage = client.GetAsync($"https://api.spotify.com/v1/playlists/{p.Id}/tracks").Result;
                        p.ResponsetContent = p.HttpResponseMessage.Content;
                        p.ResponseString = p.ResponsetContent.ReadAsStringAsync().Result;
                        finalResponse = FinalGeneratedClass.FromJson(p.ResponseString);
                        for (int i = 0; i < finalResponse.Total; i++)
                        {
                            Models.Track t = new Models.Track
                            {
                                Name = finalResponse.Items[i].Track.Name
                            };
                            p.Tracks.Add(t);
                        };
                    }
                }
            }
            var model = new PlaylistViewModel
            {
                Playlists = PlaylistList,
                Token = token
            };

            return View(model);
        }
    }
}


