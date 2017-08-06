using Narato.Academy.Events.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Narato.Academy.Events.Domain.Contracts.DataProviders
{
    public interface IEventDataProvider
    {
        Task<Event> CreateAsync(Event ev);
        Task<IEnumerable<Event>> GetAllAsync(int page, int pagesize);
        Task<int> CountAllAsync();
    }
}
