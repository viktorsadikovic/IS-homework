using Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<Ticket> Tickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
