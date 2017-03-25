using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeciBackend.Data
{
    public class UAnimeListe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AnimeId { get; set; }
        public int ListType { get; set; }
        public string Kaynak { get; set; }
        public int Ekstra { get; set; }
        public DateTimeOffset Tarih { get; set; }
        public int Puan { get; set; }

        public Anime Anime { get; set; }
        public ApplicationUser User {get;set;}
    }
}