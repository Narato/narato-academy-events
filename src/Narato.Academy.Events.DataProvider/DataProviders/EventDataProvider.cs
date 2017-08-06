using Narato.Academy.Events.Domain.Contracts.DataProviders;
using Narato.Academy.Events.Domain.Models;
using System.Threading.Tasks;
using Narato.Academy.Events.DataProvider.Contexts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        public async Task<int> CountAllAsync()
        {
            return await _context.Events.CountAsync();
        }

        public async Task<Event> CreateAsync(Event ev)
        {
            var mappedEvent = _mapper.Map<Models.Event>(ev);

            _context.Events.Add(mappedEvent);
            await _context.SaveChangesAsync();
            return _mapper.Map<Event>(mappedEvent);
        }

        public async Task<IEnumerable<Event>> GetAllAsync(int page, int pagesize)
        {
            var events = await _context.Events
                .Skip((page - 1) * pagesize)
                .Take(pagesize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<Event>>(events);
        }
    }
}
