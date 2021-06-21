using Domain.DomainModels;
using Domain.DTO;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Implementation
{
    public class ProjectionService : IProjectionService
    {
        private readonly IRepository<Projection> _projectionRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IRepository<Movie> _movieRepository;

        public ProjectionService(IRepository<Projection> projectionRepository,
                                 ITicketRepository ticketRepository,
                                 IRepository<Movie> movieRepository)
        {
            _projectionRepository = projectionRepository;
            _ticketRepository = ticketRepository;
            _movieRepository = movieRepository;
        }

        public void CreateNewProjection(MovieProjectionViewModel p)
        {
            Projection newProjection = new Projection();
            newProjection.Id = Guid.NewGuid();
            newProjection.Hall = p.Hall;
            newProjection.DateTime = p.DateTime;
            Movie movie = _movieRepository.Get(p.MovieId);
            newProjection.Movie = movie;

            _projectionRepository.Insert(newProjection);

            for (int i = 1; i <= 30; i++)
            {
                Ticket ticket = new Ticket();
                ticket.Availability = true;
                ticket.Id = Guid.NewGuid();
                ticket.Price = p.Price;
                ticket.Seat = i;
                ticket.MovieProjection = newProjection;
                _ticketRepository.Insert(ticket);
            }
        }

        public void DeleteProjection(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<Projection> GetAllProjections()
        {
            return _projectionRepository.GetAll().ToList();
        }

        public Projection GetDetailsForProjection(Guid? id)
        {
            return _projectionRepository.Get(id);
        }

        public void UpdeteExistingProjection(Projection p)
        {
            throw new NotImplementedException();
        }
    }
}
