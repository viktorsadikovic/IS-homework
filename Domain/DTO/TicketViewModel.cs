using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class TicketViewModel
    {
        public Guid MovieProjectionId { get; set; }
        public int Seat { get; set; }
        public double Price { get; set; }
        public bool Availability { get; set; }
    }
}
