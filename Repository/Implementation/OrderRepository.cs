using Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Order> entities;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<Order>();
        }

        public List<Order> getAllOrders()
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.Tickets)
                .Include("Tickets.MovieProjection")
                .Include("Tickets.MovieProjection.Movie")
                .ToListAsync().Result;
        }

        public Order getOrderDetails(Guid id)
        {
            return entities
                .Include(z => z.User)
                .Include(z => z.Tickets)
                .Include("Tickets.MovieProjection")
                .Include("Tickets.MovieProjection.Movie")
                .SingleOrDefaultAsync(z => z.Id == id).Result;
        }
    }
}
