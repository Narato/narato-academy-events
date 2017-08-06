using Narato.Academy.Events.Domain.Contracts.DataProviders;
using Narato.Academy.Events.Domain.Models;
using System.Threading.Tasks;
using Narato.Academy.Events.DataProvider.Contexts;
using AutoMapper;

namespace Narato.Academy.Events.DataProvider.DataProviders
{
    public class EventDataProvider : IEventDataProvider
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public EventDataProvider(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Event> CreateAsync(Event ev)
        {
            var mappedEvent = _mapper.Map<Models.Event>(ev);

            _context.Events.Add(mappedEvent);
            await _context.SaveChangesAsync();
            return _mapper.Map<Event>(mappedEvent);
        }
    }
}
