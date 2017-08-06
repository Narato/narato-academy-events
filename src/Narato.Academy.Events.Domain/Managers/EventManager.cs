﻿using Narato.Academy.Events.Domain.Managers.Interfaces;
using Narato.Academy.Events.Domain.Models;
using System.Threading.Tasks;
using Narato.Academy.Events.Domain.Contracts.DataProviders;
using Narato.ResponseMiddleware.Models.Models;

namespace Narato.Academy.Events.Domain.Managers
{
    public class EventManager : IEventManager
    {
        private readonly IEventDataProvider _eventDataProvider;

        public EventManager(IEventDataProvider eventDataProvider)
        {
            _eventDataProvider = eventDataProvider;
        }

        public async Task<Event> CreateEvent(Event ev)
        {
            return await _eventDataProvider.CreateAsync(ev);
        }

        public async Task<Paged<Event>> GetAllEvents(int page, int pagesize)
        {
            var events = await _eventDataProvider.GetAllAsync(page, pagesize);
            var total = await _eventDataProvider.CountAllAsync();

            return new Paged<Event>(events, page, pagesize, total);
        }
    }
}
