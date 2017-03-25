using System;
using System.Linq;
using System.Threading.Tasks;
using AnimeciBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeciBackend.Controllers
{
    [Produces("application/json")]
    public class IdentityController : Controller
    {
        AnimeciDbContext _context;
        UserManager<ApplicationUser> _userManager;

        public IdentityController(AnimeciDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("api/Uyelik/Kim")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            if (User != null)
            {
                var currUser = await _userManager.GetUserAsync(User);
                var list = await _context.UAnimeListe.Where(o => o.UserId == currUser.Id).ToListAsync();
                var claims = from c in User.Claims select new { c.Type, c.Value };
                return Json(new { 
                    email = currUser.Email, 
                    uid = currUser.UserName, 
                    id = currUser.Id,
                    // 1 watching // 2 completed // 3 hold // 4 dropped // 6 planto
                    uw = list.Count(i => i.ListType == 1),
                    uc = list.Count(i => i.ListType == 2),
                    uh = list.Count(i => i.ListType == 3),
                    ud = list.Count(i => i.ListType == 4),
                    up = list.Count(i => i.ListType == 6),
                    total = "daha izlenen anime yok"
                    });
            }
            else
                return Json(false);
        }

        [RouteAttribute("api/Uyelik/Kayit")]
        [HttpPost]
        public async Task<IActionResult> Kayit(string email, string uname, string pw)
        {
            var u = new ApplicationUser();
            u.Email = email;
            u.ConcurrencyStamp = Guid.NewGuid().ToString();
            u.UserName = uname;
            u.NormalizedUserName = uname;
            u.NormalizedEmail = email;
            u.SecurityStamp = Guid.NewGuid().ToString();
            u.LockoutEnabled = true;
            var r = await _userManager.CreateAsync(u, pw);

            return Json(r);
        }
    }
}