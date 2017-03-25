using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeciBackend.Data
{
    public class Bolum
    {
        public Bolum()
        {
            Videolar = new List<Video>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Adi { get; set; }
        public long Bid { get; set; }
        public string URL { get; set; }
        public DateTimeOffset Guncel { get; set; }
        public DateTimeOffset Eklendi { get; set; }

        public List<Video> Videolar { get; set; }

        public int AnimeID { get; set; }
        public Anime Anime { get; set; }

        public virtual UBolumListe Liste { get; set; }
    }
}