using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AnimeciBackend.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<UAnimeListe> AnimeListesi { get; set; }
        public ICollection<UBolumListe> BolumListesi { get; set; }
    }
}
