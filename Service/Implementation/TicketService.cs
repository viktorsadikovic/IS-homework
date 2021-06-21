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
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IUserRepository _userRepository;


        public TicketService(ITicketRepository ticketRepository,
                             IUserRepository userRepository,
                             IRepository<ShoppingCart> shoppingCartRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public bool AddToShoppingCart(Ticket item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userShoppingCard = user.UserCart;

            if (item != null && userShoppingCard != null)
            {
                userShoppingCard.TicketsInShoppingCart.Add(item);
                _shoppingCartRepository.Update(userShoppingCard);
                item.Availability = false;
                _ticketRepository.Update(item);
                
                return true;
            }
            return false;
        }

        public void CreateNewTicket(Ticket p)
        {
            this._ticketRepository.Insert(p);
        }

        public void DeleteTicket(Guid id)
        {
            var ticket = this.GetDetailsForTicket(id);
            this._ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return this._ticketRepository.GetAll().ToList().FindAll(t => t.Availability);
        }

        public Ticket GetDetailsForTicket(Guid? id)
        {
            return _ticketRepository.Get(id);
        }

        public AddToShoppingCartDto GetShoppingCartInfo(Guid? id)
        {
            var ticket = GetDetailsForTicket(id);
            AddToShoppingCartDto model = new AddToShoppingCartDto
            {
                Quantity = 1,
                TicketId = ticket.Id,
                SelectedTicket = ticket
            };

            return model;
        }

        public void UpdeteExistingTicket(Ticket p)
        {
            _ticketRepository.Update(p);
        }
    }
}
