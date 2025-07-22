using AutoMapper;
using Kullanici.API.DTOs;
using Kullanici.API.Models;

namespace Kullanici.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
          
            CreateMap<User, UserForDetailDto>();
        }
    }
}
