using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Narato.Academy.Events.APIContracts.DTO;
using Narato.Academy.Events.Domain.Managers.Interfaces;
using Narato.Academy.Events.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Narato.Academy.Events.API.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventManager _eventManager;
        private readonly IMapper _mapper;

        public EventsController(IEventManager eventManager, IMapper mapper)
        {
            _eventManager = eventManager;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetEventById")]
        public Task<IActionResult> GetEventById(Guid id)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventDto ev)
        {
            var businessEvent = _mapper.Map<Event>(ev);
            var createdEvent = await _eventManager.CreateEvent(businessEvent);
            return CreatedAtRoute("GetEventById", new { Id = createdEvent.Id }, _mapper.Map<EventDto>(createdEvent));
        }
    }
}
