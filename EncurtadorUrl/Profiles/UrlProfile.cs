using AutoMapper;
using EncurtadorUrl.Dtos;
using EncurtadorUrl.Models;

namespace EncurtadorUrl.Profiles
{
    public class UrlProfile : Profile
    {
        public UrlProfile()
        {
            CreateMap<UrlModel, UrlDto>();
            CreateMap<UrlDto, UrlModel>();
            CreateMap<UrlCreateDto, UrlModel>();
            CreateMap<UrlModel, UrlCreateDto> ();
            CreateMap<UrlModel, UrlUpdateDto>();
            CreateMap<UrlUpdateDto, UrlModel>();
            CreateMap<UrlModel, UrlReadDto>();
            CreateMap<UrlReadDto, UrlModel>();
        }
    }
}
