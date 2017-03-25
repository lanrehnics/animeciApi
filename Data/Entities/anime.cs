using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using AnimeciBackend.Data.Objects.Atarashii;
using Newtonsoft.Json;

namespace AnimeciBackend.Data
{
    public class Anime
    {
        public Anime()
        {
            Bolumler = new List<Bolum>();
            // Turleri = new List<string>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Adi { get; set; }
        public string Alternatif { get; set; }
        public string Ozet { get; set; }
        public int Taid { get; set; }
        public string Yili { get; set; }
        public string URL { get; set; }
        public string Poster { get; set; }
        public string BolumSayisi { get; set; }
        public string[] Turleri { get; set; }

        public bool MalInfo { get; set; }
        public int MalID { get; set; }
        public DateTimeOffset MalGuncel { get; set; }
        public DateTimeOffset Guncel { get; set; }
        public DateTimeOffset Eklendi { get; set; }
        public string Atarashii { get; set; }
        public int Sure { get; set; }
        public string Tip { get; set; }

        public List<Bolum> Bolumler { get; set; }

        public virtual UAnimeListe Liste { get; set; }
        public virtual AnimeDetail GetAtarashii() =>
            JsonConvert.DeserializeObject<AnimeDetail>(Atarashii);
    }
}