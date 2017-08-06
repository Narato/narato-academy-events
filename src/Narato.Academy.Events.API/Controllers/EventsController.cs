using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Narato.Academy.Events.APIContracts.DTO;
using Narato.Academy.Events.Domain.Managers.Interfaces;
using Narato.Academy.Events.Domain.Models;
using Narato.ResponseMiddleware.Models.Models;
using System;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pagesize = 10)
        {
            var pagedEvents = await _eventManager.GetAllEvents(page, pagesize);
            var eventDtos = _mapper.Map<IEnumerable<EventDto>>(pagedEvents.Items);
            return Ok(new Paged<EventDto>(eventDtos, page, pagesize, pagedEvents.Total));
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
