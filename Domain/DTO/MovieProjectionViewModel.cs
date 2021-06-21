using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MovieProjectionViewModel
    {
        public Guid MovieId { get; set; }
        public string Hall { get; set; }
        public DateTime DateTime { get; set; }
        public double Price { get; set; }
    }
}
