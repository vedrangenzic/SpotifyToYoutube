//using System;
//using System.Globalization;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;

//namespace SpotifyToYoutube
//{
//    public partial class GeneratedClass
//    {
//        [JsonProperty("href")]
//        public Uri Href { get; set; }

//        [JsonProperty("items")]
//        public Item[] Items { get; set; }

//        [JsonProperty("limit")]
//        public long Limit { get; set; }

//        [JsonProperty("next")]
//        public object Next { get; set; }

//        [JsonProperty("offset")]
//        public long Offset { get; set; }

//        [JsonProperty("previous")]
//        public object Previous { get; set; }

//        [JsonProperty("total")]
//        public long Total { get; set; }
//    }

//    public partial class Item
//    {
//        [JsonProperty("collaborative")]
//        public bool Collaborative { get; set; }

//        [JsonProperty("description")]
//        public string Description { get; set; }

//        [JsonProperty("external_urls")]
//        public ExternalUrls ExternalUrls { get; set; }

//        [JsonProperty("href")]
//        public Uri Href { get; set; }

//        [JsonProperty("id")]
//        public string Id { get; set; }

//        [JsonProperty("images")]
//        public Image[] Images { get; set; }

//        [JsonProperty("name")]
//        public string Name { get; set; }

//        [JsonProperty("owner")]
//        public Owner Owner { get; set; }

//        [JsonProperty("primary_color")]
//        public object PrimaryColor { get; set; }

//        [JsonProperty("public")]
//        public bool Public { get; set; }

//        [JsonProperty("snapshot_id")]
//        public string SnapshotId { get; set; }

//        [JsonProperty("tracks")]
//        public Tracks Tracks { get; set; }

//        [JsonProperty("type")]
//        public string Type { get; set; }

//        [JsonProperty("uri")]
//        public string Uri { get; set; }
//    }

//    public partial class ExternalUrls
//    {
//        [JsonProperty("spotify")]
//        public Uri Spotify { get; set; }
//    }

//    public partial class Image
//    {
//        [JsonProperty("height")]
//        public long Height { get; set; }

//        [JsonProperty("url")]
//        public Uri Url { get; set; }

//        [JsonProperty("width")]
//        public long Width { get; set; }
//    }

//    public partial class Owner
//    {
//        [JsonProperty("display_name")]
//        public string DisplayName { get; set; }

//        [JsonProperty("external_urls")]
//        public ExternalUrls ExternalUrls { get; set; }

//        [JsonProperty("href")]
//        public Uri Href { get; set; }

//        [JsonProperty("id")]
//        public string Id { get; set; }

//        [JsonProperty("type")]
//        public string Type { get; set; }

//        [JsonProperty("uri")]
//        public string Uri { get; set; }
//    }

//    public partial class Tracks
//    {
//        [JsonProperty("href")]
//        public Uri Href { get; set; }

//        [JsonProperty("total")]
//        public long Total { get; set; }
//    }

//    public partial class GeneratedClass
//    {
//        public static GeneratedClass FromJson(string json) => JsonConvert.DeserializeObject<GeneratedClass>(json, Converter._Settings);
//    }

//    public static class Serialize
//    {
//        public static string ToJson(this GeneratedClass self) => JsonConvert.SerializeObject(self, Converter._Settings);
//    }

//    internal static class Converter
//    {
//        public static readonly JsonSerializerSettings _Settings = new JsonSerializerSettings
//        {
//            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
//            DateParseHandling = DateParseHandling.None,
//            Converters =
//            {
//                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
//            },
//        };
//    }
//}
