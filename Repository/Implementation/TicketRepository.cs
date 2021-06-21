using Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<Ticket> entities;

        public TicketRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entities = context.Set<Ticket>();
        }

        public Ticket Get(Guid? id)
        {
            return entities
                    .Include(z => z.MovieProjection)
                    .Include("MovieProjection.Movie")
                    .SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<Ticket> GetAll()
        {
            return entities
                .Include(z => z.MovieProjection)
                .Include("MovieProjection.Movie")
                .ToListAsync().Result;
        }

        public void Insert(Ticket entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(Ticket entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(Ticket entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
