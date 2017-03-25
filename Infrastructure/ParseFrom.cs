using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AnimeciBackend.Data.Objects.MAL.List;
using Newtonsoft.Json;

namespace AnimeciBackend
{
    public class JAnime
    {
        public string Anime { get; set; }
        public string link { get; set; }
        public string animeID { get; set; }
    }

    public class ParseFrom
    {
        HttpClient _client;
        string _ua;

        public ParseFrom()
        {
            _ua = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            var proxy = Environment.GetEnvironmentVariable("HTTP_PROXY");
            if (!string.IsNullOrEmpty(proxy))
                _client = new HttpClient(new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    UseProxy = true,
                    Proxy = new MyProxy(proxy)
                });
            else
                _client = new HttpClient(new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                });

            _client.DefaultRequestHeaders.Add("User-Agent", _ua);
        }
        
        // public async Task<IHtmlDocument> Url(string url)
        // {
        //     var req = new HttpRequestMessage(HttpMethod.Get, url);
        //     req.Headers.Add("Cookie", _cookie);
        //     try
        //     {
        //         var res = await _client.SendAsync(req);
        //         res.EnsureSuccessStatusCode();
        //         var content = await res?.Content.ReadAsStreamAsync();
        //         var document = await new HtmlParser().ParseAsync(content);

        //         return document;
        //     }
        //     catch (Exception hre) when (hre.HResult == -2147012744)
        //     {
        //         Console.WriteLine("Request to '{0}' failed, retrying...", url);

        //         return await Url(url);
        //     }
        // }

        public async Task<byte[]> Image(string url)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            // req.Headers.Add("Cookie", _cookie);
            var response = await _client.SendAsync(req);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsByteArrayAsync();
            return content;
        }

        public async Task<string> Source(string url)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            // req.Headers.Add("Cookie", _cookie);
            var response = await _client.SendAsync(req);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        public async Task<Data.Objects.Kitsu.Anime.AnimeObject> Kitsu(int id)
        {
            var ksrc = await Source("https://kitsu.io/api/edge/anime/" + id);
            var k = JsonConvert.DeserializeObject<Data.Objects.Kitsu.Anime.AnimeObject>(ksrc);
            return k;
        }

        public async Task<Data.Objects.Kitsu.Mappings.MappingResults> KitsuMapping(object id)
        {
            var map_src = await Source($"https://kitsu.io/api/edge/anime/{id}/mappings");
            var maps = JsonConvert.DeserializeObject<Data.Objects.Kitsu.Mappings.MappingResults>(map_src);
            return maps;
        }

        public async Task<Myanimelist> MalListOf(string usr)
        {
            using (var req = await _client.GetStreamAsync($"https://myanimelist.net/malappinfo.php?u={usr}&status=all&type=anime"))
            {
                var xmls = new XmlSerializer(typeof(Myanimelist));
                return xmls.Deserialize(req) as Myanimelist;
            }
        }

        public async Task<IEnumerable<JAnime>> GetJSONList()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "http://www.turkanime.tv/liste.json");
            // req.Headers.Add("Cookie", _cookie);
            //var json = await _client.GetStringAsync("http://www.turkanime.tv/liste.json");
            using (var resp = await _client.SendAsync(req))
            {
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IEnumerable<JAnime>>(json);
                return list;
            }
        }
    }
}
