using Narato.Academy.Events.Domain.Models;
using Narato.ResponseMiddleware.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Narato.Academy.Events.Domain.Managers.Interfaces
{
    public interface IEventManager
    {
        Task<Event> CreateEvent(Event ev);
        Task<Paged<Event>> GetAllEvents(int page, int pagesize);
    }
}
