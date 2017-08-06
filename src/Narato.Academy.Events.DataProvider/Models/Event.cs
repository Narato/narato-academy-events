using System;
using System.Collections.Generic;
using System.Text;

namespace Narato.Academy.Events.DataProvider.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
