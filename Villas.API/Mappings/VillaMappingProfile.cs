using AutoMapper;
using Villas.API.DTOs;
using Villas.API.Models.Domain;

namespace Villas.API.Mappings
{
    public class VillaMappingProfile : Profile
    {
        public VillaMappingProfile()
        {
            CreateMap<Villa, VillaResponse>().ReverseMap();
            CreateMap<CreateVillaRequest, Villa>();
            CreateMap<UpdateVillaRequest, Villa>();
        }
    }
}
