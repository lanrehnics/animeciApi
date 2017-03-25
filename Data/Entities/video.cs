using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeciBackend.Data
{
    public class Video
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Grup { get; set; }
        public string EmegiGecenler { get; set; }
        public string Kimin { get; set; }
        public string Src { get; set; }
        public string Nerden { get; set; }
        public DateTimeOffset Guncel { get; set; }
        public DateTimeOffset Eklendi { get; set; }

        public bool QC { get; set; }
        public int Vid { get; set; }

        public int BolumID { get; set; }
        public Bolum Bolum { get; set; }
    }
}