using System;
using AutoMapper;

namespace AnimeciBackend.Data.DTOs
{
    public class AnimeDTO
    {
        public int ID { get; set; }
        public int Taid { get; set; }
        public string Adi { get; set; }
        public string Alternatif { get; set; }
        public string Ozet { get; set; }
        public string Yili { get; set; }
        public string Poster { get; set; }
        public string BolumSayisi { get; set; }
        public string[] Turleri { get; set; }
        public DateTimeOffset Guncel { get; set; }
        public DateTimeOffset Eklendi { get; set; }
        public UAnimeListeDTO Liste { get; set; }
        public int Sure { get; set; }
        public string Tip { get; set; }
    }

    public class AnimeProfile : Profile
    {
        public AnimeProfile()
        {
            CreateMap<Anime, AnimeDTO>();
        }
    }
}