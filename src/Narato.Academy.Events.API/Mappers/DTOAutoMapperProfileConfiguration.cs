using AutoMapper;
using Narato.Academy.Events.APIContracts.DTO;
using Narato.Academy.Events.Domain.Models;

namespace Narato.Academy.Events.API.Mappers
{
    public class DTOAutoMapperProfileConfiguration : Profile
    {
        public DTOAutoMapperProfileConfiguration()
        {
            CreateMap<Event, EventDto>()
                .ReverseMap();
        }
    }
}
