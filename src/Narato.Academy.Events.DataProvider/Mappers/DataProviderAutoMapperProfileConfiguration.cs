using AutoMapper;

namespace Narato.Academy.Events.DataProvider.Mappers
{
    public class DataProviderAutoMapperProfileConfiguration : Profile
    {
        public DataProviderAutoMapperProfileConfiguration()
        {
            CreateMap<Models.Event, Domain.Models.Event>()
                .ReverseMap();
        }
    }
}
