using System;
using AutoMapper;

namespace AnimeciBackend.Data.DTOs
{
    public class VideoDTO
    {
        public int ID { get; set; }
        public string Grup { get; set; }
        public string EmegiGecenler { get; set; }
        public string Kimin { get; set; }
        public string Src { get; set; }
        public DateTimeOffset Guncel { get; set; }
        public DateTimeOffset Eklendi { get; set; }
        public bool QC { get; set; }
    
        public int BolumID { get; set; }
    }

    public class VideoProfile : Profile
    {
        public VideoProfile()
        {
            CreateMap<Video, VideoDTO>();
        }
    }
}