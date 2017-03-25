using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeciBackend.Data;
using AnimeciBackend.Data.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using System;
using Nest;

namespace AnimeciBackend.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class ApimController : Controller
    {
        AnimeciDbContext _context;
        ParseFrom _parseFrom;
        IMapper _mapper;
        UserManager<ApplicationUser> _userManager;

        public ApimController(AnimeciDbContext context, ParseFrom pf, IMapper mapper, UserManager<ApplicationUser> userManager, IHostingEnvironment env)
        {
            _context = context;
            _parseFrom = pf;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Route("api/Anime/{id}")]
        public async Task<IActionResult> GetAnime(int id)
        {
            var anime = await _context.Anime.FindAsync(id);
            if (anime == null)
                return NotFound();

            var ua = await _context.UAnimeListe.FirstOrDefaultAsync(i => i.AnimeId == anime.ID && i.UserId == _userManager.GetUserId(User));
            if (ua != null) anime.Liste = ua;

            var dto = _mapper.Map<AnimeDTO>(anime);

            return Json(dto);
        }

        [Route("api/Bolum/{id}")]
        public async Task<IActionResult> GetBolum(int id)
        {
            var bolum = await _context.Bolum.Include(b => b.Videolar).FirstOrDefaultAsync(b => b.ID == id);
            if (bolum == null)
                return NotFound();

            var ub = await _context.UBolumListe.FirstOrDefaultAsync(i => i.BolumId == bolum.ID && i.UserId == _userManager.GetUserId(User));
            if (ub != null) bolum.Liste = ub;

            var dto = _mapper.Map<BolumDTO>(bolum);

            return Json(dto);
        }

        [Route("api/Bolumler/{id}")]
        public async Task<IActionResult> GetBolumler(int id)
        {
            var bolumleriyle = await _context.Bolum.Where(a => a.AnimeID == id).OrderBy(o => o.Bid).ToListAsync();
            if (bolumleriyle == null || bolumleriyle.Count == 0)
                return NotFound();

            var ub = _context.UBolumListe.Where(i => i.AnimeId == id && i.UserId == _userManager.GetUserId(User));
            if (ub != null)
                foreach (var b in bolumleriyle)
                    b.Liste = await ub.FirstOrDefaultAsync(i => i.BolumId == b.ID);

            var dto = _mapper.Map<IEnumerable<BolumDTO>>(bolumleriyle);

            return Json(dto);
        }

        [Route("api/Bolumleri/{id}")]
        public async Task<IActionResult> GetBolumleri(int id)
        {
            var bolumleriyle = await _context.Bolum.Where(a => a.AnimeID == id).Include(v => v.Videolar).OrderBy(o => o.Bid).ToListAsync();
            if (bolumleriyle == null || bolumleriyle.Count == 0)
                return NotFound();

            var ub = _context.UBolumListe.Where(i => i.AnimeId == id && i.UserId == _userManager.GetUserId(User));
            if (ub != null)
                foreach (var b in bolumleriyle)
                    b.Liste = await ub.FirstOrDefaultAsync(i => i.BolumId == b.ID);

            var dto = _mapper.Map<IEnumerable<BolumDTO>>(bolumleriyle);

            return Json(dto);
        }

        [Route("api/Son/Bolum")]
        public async Task<IActionResult> SonBolumler()
        {
            //TODO: sayfalama lazim
            var BIDeDesc = await _context.Bolum.OrderByDescending(b => b.Eklendi).Include(a => a.Anime).Include(a => a.Videolar).Take(30).ToListAsync();
            BIDeDesc.RemoveAll(p => !p.Videolar.Any());

            foreach (var b in BIDeDesc)
            {
                var ua = await _context.UAnimeListe.FirstOrDefaultAsync(i => i.AnimeId == b.AnimeID && i.UserId == _userManager.GetUserId(User));
                var ub = await _context.UBolumListe.FirstOrDefaultAsync(i => i.BolumId == b.ID && i.UserId == _userManager.GetUserId(User));
                if (ua != null) b.Anime.Liste = ua;
                if (ub != null) b.Liste = ub;
            }

            var dto = _mapper.Map<IEnumerable<SonBolumDTO>>(BIDeDesc);

            return Json(dto);
        }

        [Route("api/Son/Anime")]
        public async Task<IActionResult> SonAnimeler()
        {
            //TODO: sayfalama lazim
            var TaideDesc = await _context.Anime.OrderByDescending(b => b.Taid).Take(20).ToListAsync();
            foreach (var a in TaideDesc)
            {
                var ua = await _context.UAnimeListe.FirstOrDefaultAsync(i => i.AnimeId == a.ID && i.UserId == _userManager.GetUserId(User));
                if (ua != null) a.Liste = ua;
            }
            var dto = _mapper.Map<IEnumerable<BolumDTO>>(TaideDesc);

            return Json(dto);
        }

        [Route("api/Negatifler")]
        public IActionResult Negatifler()
        {
            // md > 0, mf + => Dont touch its done!
            // md > 0, mf - => its correct but ok to update mal info

            // md < 0, mf + => ?
            // md < 0, mf - => searched and needs approval

            // md = 0, mf + => Dont touch, no mal info for this but manual info
            // md = 0, mf - => Dont touch, fck this no info at all

            var animeler = _context.Anime.Where(i => i.MalID < 0 && i.MalID != 0 && i.MalInfo == false).Select(i => new { i.ID, i.MalID, i.Adi, i.Taid });
            return Json(animeler);
        }

        [AllowAnonymous]
        [Route("api/Say")]
        public async Task<IActionResult> Say()
        {
            var Acount = await _context.Anime.CountAsync();
            var Bcount = await _context.Bolum.CountAsync();
            var Vcount = await _context.Video.CountAsync();
            var UBcount = await _context.UBolumListe.CountAsync();
            var UAcount = await _context.UAnimeListe.CountAsync();
            var que_err = await _context.QueJobs.CountAsync(o => o.error_count > 0);
            var queue = await _context.QueJobs.CountAsync();
            var negative = await _context.Anime.CountAsync(i => i.MalID < 0 && i.MalID != 0 && i.MalInfo == false);

            return Json(new { A = Acount, B = Bcount, V = Vcount, UA = UAcount, UB = UBcount, N = negative, QE = queue, QR = que_err });
        }

        [Route("api/Ara/{q}")]
        public async Task<IActionResult> AraPg(string q, string tur = null, string tip = null)
        {
            string sql;
            if (q.StartsWith("id:"))
            {
                int im = int.Parse(q.Split(':')[1]);
                sql = $@"select * from anime where ""ID"" = {im} or ""MalID"" = {im} ";
            }
            else if (q == "%")
                sql = @"select * from anime where ""ID"" > 0 ";
            else
                sql = $@"select * from anime Where coalesce(""Adi"",'') || ' ' || coalesce(""Alternatif"",'')  @@ to_tsquery('{q}:*') ";

            if (!string.IsNullOrEmpty(tur))
                sql += $@"AND ""Turleri"" @> ARRAY['{tur}'] ";
            if (!string.IsNullOrEmpty(tip))
                sql += $@"AND ""Tip"" = '{tip}' ";

            var animeler = await _context.Anime.FromSql(sql).ToListAsync();
            foreach (var a in animeler)
            {
                var ua = await _context.UAnimeListe.FirstOrDefaultAsync(i => i.AnimeId == a.ID && i.UserId == _userManager.GetUserId(User));
                if (ua != null) a.Liste = ua;
            }
            var dto = _mapper.Map<IEnumerable<AnimeDTO>>(animeler);
            return Json(dto);
        }

        [HttpGet]
        [RouteAttribute("api/FavList/{kind}")]
        public async Task<IActionResult> FavList(int kind)
        {
            var favlist = await _context.UAnimeListe.Include(o => o.Anime)
            .Where(i => i.UserId == _userManager.GetUserId(User) && i.ListType == kind).ToListAsync();
            var al = favlist.Select(i => i.Anime).OrderBy(i => i.Adi);

            var dto = _mapper.Map<IEnumerable<AnimeDTO>>(al);

            return Json(dto);
        }

        [HttpPost]
        [RouteAttribute("api/FavList/{id}/{kind}/{point}")]
        public async Task<IActionResult> FavListMe(int id, int kind, int point)
        {
            var exists = await _context.Anime.AnyAsync(o => o.ID == id);
            if (exists == false)
                return NotFound();

            var ua = await _context.UAnimeListe.FirstOrDefaultAsync(i => i.AnimeId == id);
            if (ua != null)
            {
                ua.ListType = kind;
                ua.Puan = point;
            }
            else
            {
                ua = new UAnimeListe();
                ua.AnimeId = id;
                ua.Kaynak = "Animeci";
                ua.ListType = kind;
                ua.Puan = point;
                ua.Tarih = DateTimeOffset.Now;
                ua.UserId = _userManager.GetUserId(User);
                _context.UAnimeListe.Add(ua);
                if (kind == 2)
                {
                    var bl = await _context.Bolum.Where(i => i.AnimeID == id).ToListAsync();
                    foreach (var b in bl)
                    {
                        var ub = await _context.UBolumListe.FirstOrDefaultAsync(i => i.BolumId == b.ID);
                        if (ub == null)
                        {
                            ub = new UBolumListe();
                            ub.AnimeId = b.AnimeID;
                            ub.BolumId = b.ID;
                            ub.Puan = point;
                            ub.Tarih = DateTimeOffset.Now;
                            ub.UserId = _userManager.GetUserId(User);
                            _context.UBolumListe.Add(ub);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            return Json(ua);
        }

        [HttpPost]
        [RouteAttribute("api/FavBolum/{id}")]
        public async Task<IActionResult> BolumIzlendi(int id)
        {
            var ub = new UBolumListe();
            var bolum = await _context.Bolum.FindAsync(id);
            if (bolum != null)
            {
                ub.AnimeId = bolum.AnimeID;
                ub.BolumId = bolum.ID;
                ub.Puan = 0;
                ub.Tarih = DateTimeOffset.Now;
                ub.UserId = _userManager.GetUserId(User);
                _context.UBolumListe.Add(ub);
                var ua = await _context.UAnimeListe.FirstOrDefaultAsync(o => o.AnimeId == bolum.AnimeID);
                ua.Ekstra++;
                if (bolum.Adi.Contains("Bölüm Final"))
                    ua.ListType = 2;
                await _context.SaveChangesAsync();

                return Json(ub);
            }

            return NotFound();
        }

        [HttpPost]
        [RouteAttribute("api/FavDelBolum/{id}")]
        public async Task<IActionResult> BolumIzlenmeSil(int id)
        {
            var ub = await _context.UBolumListe.FindAsync(id);
            if (ub != null)
            {
                _context.UBolumListe.Remove(ub);
                var ua = await _context.UAnimeListe.FirstOrDefaultAsync(o => o.AnimeId == ub.AnimeId);
                ua.Ekstra--;
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        [HttpPost]
        [RouteAttribute("api/FavDel/{id}")]
        public async Task<IActionResult> FavAnimeSil(int id)
        {
            var ub = await _context.UAnimeListe.FindAsync(id);
            if (ub != null)
            {
                _context.UAnimeListe.Remove(ub);
                var bl = _context.UBolumListe.Where(o => o.AnimeId == ub.AnimeId);
                _context.UBolumListe.RemoveRange(bl);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound();
        }

        [HttpGet]
        [Route("api/AraES/{query}")]
        public async Task<IActionResult> Ara(string query)
        {
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex("animeci");
            var client = new ElasticClient(settings);

            var sonuclar = await client.SearchAsync<AnimeDTO>(q => q.Type("animeler").Query(m =>
                            m.MultiMatch(c =>
                                c.Fields(f => f
                                        .Field(p => p.Adi)
                                        .Field(p => p.Alternatif)
                                )
                                .Query(query)
                            ))
                            .Skip(0)
                            .Take(15));

            return Json(sonuclar.Documents);
        }

        [HttpGet]
        [AllowAnonymous]
        [RouteAttribute("api/Export/ADTO/{id}")]
        public async Task<IActionResult> ADTO(int id)
        {
            var anime = await _context.Anime.FirstOrDefaultAsync(i => i.Taid == id);
            if (anime != null)
            {
                var dto = _mapper.Map<AnimeDTO>(anime);
                return Json(dto);
            }

            return NotFound();
        }

        [Route("api/TestWorker")]
        [HttpPost]
        public IActionResult Test([FromBody] JAnime ja)
        {
            var qj = new QueJobs();
            qj.job_class = "FetchAnime";
            qj.args = JsonConvert.SerializeObject(ja);
            _context.QueJobs.Add(qj);
            _context.SaveChanges();

            return Json(qj);
        }

        public IActionResult Error()
        {
            return Json(new { err = "Bir hata olustu :(" });
        }
    }
}
