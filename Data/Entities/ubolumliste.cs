using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeciBackend.Data
{
    public class UBolumListe
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AnimeId { get; set; }
        public int BolumId { get; set; }
        public long Ekstra { get; set; }
        public DateTimeOffset Tarih { get; set; }
        public int Puan { get; set; }

        public ApplicationUser User {get;set;}
    }
}
