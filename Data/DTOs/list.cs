using System;
using AutoMapper;

namespace AnimeciBackend.Data.DTOs
{
    public class UAnimeListeDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AnimeId { get; set; }
        public int ListType { get; set; }
        public string Kaynak { get; set; }
        public DateTimeOffset Tarih { get; set; }
        public int Puan { get; set; }
        public int Ekstra { get; set; }
    }

    public class AFavListProfile : Profile
    {
        public AFavListProfile()
        {
            CreateMap<UAnimeListe, UAnimeListeDTO>();
        }
    }
}
    