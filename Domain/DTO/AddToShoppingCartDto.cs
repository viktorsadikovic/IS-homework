﻿using Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class AddToShoppingCartDto
    {
        public Ticket SelectedTicket { get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
