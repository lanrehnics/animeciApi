using AnimeciBackend.Data;
using AnimeciBackend.Data.Objects.Kitsu.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace AnimeciBackend.Controllers
{
    [Authorize]
    public class ImportController : Controller
    {
        AnimeciDbContext _context;
        ParseFrom _parseFrom;
        ILogger<ImportController> _logger;
        UserManager<ApplicationUser> _userManager;

        public ImportController(AnimeciDbContext context, ParseFrom parseFrom, ILogger<ImportController> logger, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _parseFrom = parseFrom;
            _userManager = userManager;
        }

        [Route("api/Job/TA")]
        public async Task<IActionResult> JTA()
        {
            var list = await _parseFrom.GetJSONList() as List<JAnime>;
            _logger.LogWarning(1, "Adding `FetchAnime` jobs @{listc}", list.Count);
            foreach (var j in list)
            {
                var qj = new QueJobs();
                qj.job_class = "FetchAnime";
                qj.args = JsonConvert.SerializeObject(j);
                _context.QueJobs.Add(qj);
            }
            var r = await _context.SaveChangesAsync();
            _logger.LogWarning(1, "Added `FetchAnime` jobs {r}/{listc}", r, list.Count);
            return Json(list.Count);
        }

        [Route("api/Job/CTA")]
        public async Task<IActionResult> CTA()
        {
            var r = await _context.QueJobs.FromSql("delete from que_jobs").CountAsync();
            return Json(r);
        }

        [Route("api/Export/Kontrol/{id}")]
        public async Task<IActionResult> animeForKontrol(int id)
        {
            var anime = await _context.Anime.FindAsync(id);
            if (anime == null)
                return NotFound();

            return Json(anime);
        }

        [Route("api/Import/FromKitsu/{id}")]
        [HttpPost]
        public async Task<string> fromKitsu(int id, [FromBody]Datum k)
        {
            var anime = await _context.Anime.FindAsync(id);
            if (anime != null)
            {
                // var k = await _parseFrom.Kitsu(kid);
                var maps = await _parseFrom.KitsuMapping(k.id);
                var mal = maps.data.FirstOrDefault(o => o.attributes.externalSite == "myanimelist/anime");
                anime.MalID = Convert.ToInt32(mal.attributes.externalId);
                anime.MalInfo = anime.MalID == 0 ? true : false;

                anime.Poster = k.attributes.posterImage.original;
                anime.Sure = k.attributes.episodeLength.HasValue ? k.attributes.episodeLength.Value : 0;
                anime.Tip = k.attributes.showType;
                if (!string.IsNullOrEmpty(k.attributes.titles.en))
                    anime.Alternatif += k.attributes.titles.en + ", ";
                if (!string.IsNullOrEmpty(k.attributes.titles.en_jp))
                    anime.Alternatif += k.attributes.titles.en_jp + ", ";
                if (!string.IsNullOrEmpty(k.attributes.titles.ja_jp))
                    anime.Alternatif += k.attributes.titles.ja_jp + ", ";
                if (anime.Alternatif.EndsWith(", "))
                    anime.Alternatif = anime.Alternatif.Substring(0, anime.Alternatif.Length - 2);
                anime.Atarashii = JsonConvert.SerializeObject(k);

                var qj = new QueJobs();
                qj.job_class = "FetchAnime";
                qj.priority = 80;
                qj.args = JsonConvert.SerializeObject(new { Anime = anime.Adi, link = anime.URL, animeID = anime.Taid.ToString() });
                _context.QueJobs.Add(qj);
                var r = await _context.SaveChangesAsync();

                return "Kitsu and JSON updated(" + r + ") via " + anime.MalID;
            }

            return "Failed to update";
        }

        [Route("api/Import/FromMal/{id}")]
        [HttpPost]
        public async Task<string> updateFromMal(int id, int mi)
        {
            var anime = await _context.Anime.FindAsync(id);
            if (anime != null)
            {
                if (mi > 0)
                {
                    anime.MalID = mi;
                    anime.MalInfo = false;
                    var r = await _context.SaveChangesAsync();

                    return "(" + r + ") MalID updated via " + anime.MalID;
                }
                else if (mi == 0)
                {
                    anime.MalID = mi;
                    anime.MalInfo = false;
                    var r = await _context.SaveChangesAsync();

                    return "(" + r + ") MalID updated with 0, NO MAL INFO";
                }

                var qj = new QueJobs();
                qj.job_class = "FetchAnime";
                qj.priority = 80;
                qj.args = JsonConvert.SerializeObject(new { Anime = anime.Adi, link = anime.URL, animeID = anime.Taid.ToString() });
                _context.QueJobs.Add(qj);
                await _context.SaveChangesAsync();
            }
            return "Failed to update";
        }

        // [Route("api/Import/Afisler")]
        // public async Task<IActionResult> ResimIsleri()
        // {
        //     var animeler = await _context.Anime.Select(i => new { Anime = i.Adi, link = i.Poster, animeID = i.ID.ToString() }).ToListAsync();
        //     foreach (var a in animeler)
        //     {
        //         var qj = new QueJobs();
        //         qj.job_class = "SavePoster";
        //         qj.priority = 80;
        //         qj.args = JsonConvert.SerializeObject(a);
        //         _context.QueJobs.Add(qj);
        //     }
        //     var r = await _context.SaveChangesAsync();

        //     return Json(r);
        // }

        [RouteAttribute("api/Import/MalList/{user}")]
        public async Task<IActionResult> MalList(string user)
        {
            var MalList = await _parseFrom.MalListOf(user);
            var currUser = await _userManager.GetUserAsync(HttpContext.User);
            // var currUser = await _userManager.FindByEmailAsync("alican.dilbaz@gmail.com");
            _logger.LogInformation("MAL List for " + MalList.Myinfo.User_name + " fetched, C:" + MalList.Myinfo.User_completed + " W:" + MalList.Myinfo.User_watching + " H:" + MalList.Myinfo.User_onhold + " P:" + MalList.Myinfo.User_plantowatch);
            var l = new List<Data.Objects.MAL.List.XmlAnime>();
            var dt = DateTimeOffset.Now;
            foreach (var xmla in MalList.Anime)
            {
                var anime = await _context.Anime.Select(o => new { mid = o.MalID, id = o.ID }).FirstOrDefaultAsync(i => i.mid == int.Parse(xmla.Series_animedb_id));
                if (anime != null)
                {
                    bool exists = true;
                    var ua = await _context.UAnimeListe.FirstOrDefaultAsync(i => i.AnimeId == anime.id);
                    if (ua == null)
                    {
                        ua = new UAnimeListe();
                        exists = false;
                    }

                    ua.AnimeId = anime.id;
                    ua.Kaynak = "MAL-" + user;
                    ua.Tarih = dt;
                    ua.ListType = int.Parse(xmla.My_status);
                    // 1 watching // 2 completed // 3 hold // 4 dropped // 6 planto
                    ua.Puan = int.Parse(xmla.My_score);
                    ua.User = currUser;

                    if (exists == false)
                        _context.UAnimeListe.Add(ua);

                    var eps = int.Parse(xmla.My_watched_episodes);
                    var bolumler = await _context.Bolum.Where(s => s.Bid < 0).ToListAsync();
                    if (ua.ListType == 1)
                        bolumler = await _context.Bolum.OrderBy(o => o.Bid).Where(o => o.AnimeID == anime.id).Take(eps).ToListAsync();
                    else if (ua.ListType == 2)
                        bolumler = await _context.Bolum.Where(o => o.AnimeID == anime.id).ToListAsync();

                    foreach (var b in bolumler)
                    {
                        bool bexists = true;
                        var ub = await _context.UBolumListe.FirstOrDefaultAsync(i => i.BolumId == b.ID);
                        if (ub == null)
                        {
                            ub = new UBolumListe();
                            bexists = false;
                        }

                        ub.AnimeId = anime.id;
                        ub.BolumId = b.ID;
                        ub.Puan = ua.Puan;
                        ub.Tarih = dt;
                        ub.User = currUser;

                        if (bexists == false)
                            _context.UBolumListe.Add(ub);
                    }
                    var r = await _context.SaveChangesAsync();
                }
                else
                {
                    l.Add(xmla);
                    _logger.LogWarning(currUser.UserName + "'nin listesinde <" + user + "> " + xmla.Series_title + " [" + xmla.Series_animedb_id + "] veritabaninda bulunamadi!");
                }
            }
            return Json(l);
        }

        [RouteAttribute("api/Export/2ES")]
        public async Task<IActionResult> Index2ES()
        {
            var animeler = await _context.Anime.ToListAsync();
            foreach (var anime in animeler)
            {
                var qj = new QueJobs();
                qj.job_class = "Index2ES";
                qj.priority = 80;
                qj.args = JsonConvert.SerializeObject(new { Anime = anime.Adi, link = anime.URL, animeID = anime.Taid.ToString() });
                _context.QueJobs.Add(qj);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        // public async Task<HM_Anime> GetHummingbirdMEv1(string adi)
        // {
        //     var document = await Source("http://hummingbird.me/api/v1/search/anime?query=" + WebUtility.UrlEncode(adi));
        //     var hm = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<HM_Anime>>(document);
        //     foreach (var item in hm)
        //         if (item.title.Equals(adi, StringComparison.OrdinalIgnoreCase))
        //             return item;
        //         else if (item.title.Contains(adi))
        //             return item;

        //     return hm.FirstOrDefault();
        // }

        // public async Task<IEnumerable<HM_Anime>> GetHummingbirdMEv1(string adi, bool all)
        // {
        //     var document = await Source("http://hummingbird.me/api/v1/search/anime?query=" + WebUtility.UrlEncode(adi));
        //     var hm = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<HM_Anime>>(document);
        //     foreach (var item in hm)
        //         if (item.title.Equals(adi, StringComparison.OrdinalIgnoreCase))
        //             item.choosen = true;
        //         else if (item.title.Contains(adi))
        //             item.choosen = true;

        //     hm.FirstOrDefault().choosen = true;

        //     return hm;
        // }

        // public async Task<HM_Anime2> GetHummingbirdv2(int malid)
        // {
        //     using (HttpClient hc = new HttpClient())
        //     {
        //         hc.DefaultRequestHeaders.Add("X-Client-Id", "dfa43a9b627a4530ea70");
        //         var document = await hc.GetStringAsync("https://hummingbird.me/api/v2/anime/myanimelist:" + malid);
        //         var hm2 = Newtonsoft.Json.JsonConvert.DeserializeObject<HM_Anime2>(document);
        //         return hm2;
        //     }
        // }

        // public async Task<AnimeDetail> GetAtarashii(int malid)
        // {
        //     var document = await Source("http://hbv3-mal-api.herokuapp.com/2.1/anime/" + malid);
        //     var atarashii = Newtonsoft.Json.JsonConvert.DeserializeObject<AnimeDetail>(document);
        //     return atarashii;
        // }

        // public async Task<IEnumerable<EpisodeDetail>> GetAtarashiiEpisodes(int malid)
        // {
        //     var document = await Source("http://hbv3-mal-api.herokuapp.com/2.1/anime/episodes/" + malid);
        //     var atarashii = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<EpisodeDetail>>(document);

        //     return atarashii.DistinctBy(i => i.number);
        // }
    }
}
