using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SpotifyToYoutube.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyToYoutube.Controllers
{
    [Route("youtube")]
    public class YoutubeController : Controller
    {
        private readonly ISpotifyService spotifyService;

        public YoutubeController(ISpotifyService spotifyService)
        {
            this.spotifyService = spotifyService;
        }
        [Route("")]
        public async Task<ViewResult> Index(string token, string id, string playlistName)
        {
            var tracks = spotifyService.GetTracks(token, id);
            await CreateYoutubePlaylist(playlistName, tracks);

            return View();
        }
        private async Task<Models.Track> GetYoutubeVideo(string name, string artist)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "YOUR_API_KEY",
                ApplicationName = this.GetType().ToString()
            });

            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = artist + " " + name; // Replace with your search term.
            searchListRequest.MaxResults = 10;
            searchListRequest.Type = "video";

            // Call the search.list method to retrieve results matching the specified query term.
            var searchListResponse = await searchListRequest.ExecuteAsync();

            var video = new Models.Track();

            // Add each result to the appropriate list, and then display the lists of
            // matching videos, channels, and playlists.
            video.Name = String.Format(searchListResponse.Items.First().Snippet.Title);
            video.Id = String.Format(searchListResponse.Items.First().Id.VideoId);

            return video;
        }
        private async Task<string> CreateYoutubePlaylist(string playlistName, List<Models.Track> tracks)
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for full read/write access to the
                    // authenticated user's account.
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Create a new, private playlist in the authorized user's channel.
            var newPlaylist = new Google.Apis.YouTube.v3.Data.Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = playlistName;
            newPlaylist.Snippet.Description = "A playlist created with the YouTube API v3";
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "public";
            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();

            foreach (var track in tracks)
            {
                var video = await GetYoutubeVideo(track.Name, track.Artist);
                var newPlaylistItem = new PlaylistItem();
                newPlaylistItem.Snippet = new PlaylistItemSnippet();
                newPlaylistItem.Snippet.PlaylistId = newPlaylist.Id;
                newPlaylistItem.Snippet.ResourceId = new ResourceId();
                newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
                newPlaylistItem.Snippet.ResourceId.VideoId = video.Id;
                newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();
            }

            return newPlaylist.Id;
        }

    }
}
