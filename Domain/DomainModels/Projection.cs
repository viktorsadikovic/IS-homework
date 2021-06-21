using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DomainModels
{
    public class Projection : BaseEntity
    {
        public Movie Movie { get; set; }
        public string Hall { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
