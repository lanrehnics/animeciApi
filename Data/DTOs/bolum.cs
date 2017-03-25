using System;
using System.Collections.Generic;
using AutoMapper;

namespace AnimeciBackend.Data.DTOs
{
    public class BolumDTO
    {
        public BolumDTO()
        {
            Videolar = new List<VideoDTO>();
        }

        public int ID { get; set; }
        public string Adi { get; set; }
        public DateTimeOffset Guncel { get; set; }
        public DateTimeOffset Eklendi { get; set; }
        public List<VideoDTO> Videolar { get; set; }
        public int AnimeID { get; set; }
        public UBolumListe Liste { get; set; }
    }

    public class SonBolumDTO
    {
        public int ID { get; set; }
        public string Adi { get; set; }
        public DateTimeOffset Guncel { get; set; }
        public DateTimeOffset Eklendi { get; set; }
        public AnimeDTO Anime { get; set; }
        public int VideoSayisi { get; set; }
        public UBolumListe Liste { get; set; }

    }

    public class BolumProfile : Profile
    {
        public BolumProfile()
        {
            CreateMap<Bolum, BolumDTO>();
            CreateMap<Bolum, SonBolumDTO>()
                .ForMember(i => i.VideoSayisi, b => b.MapFrom(vl => vl.Videolar.Count));
        }
    }
}