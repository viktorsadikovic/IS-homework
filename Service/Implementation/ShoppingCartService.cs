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
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepositorty;
        private readonly ITicketRepository _ticketRepository;
        private readonly IRepository<Order> _orderRepositorty;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _mailRepository;


        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository,
                                   IRepository<Order> orderRepositorty,
                                   IUserRepository userRepository,
                                   ITicketRepository ticketRepository,
                                   IRepository<EmailMessage> mailRepository)
        {
            _shoppingCartRepositorty = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepositorty = orderRepositorty;
            _ticketRepository = ticketRepository;
            _mailRepository = mailRepository;
        }

        public bool deleteTicketFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.TicketsInShoppingCart.Where(z => z.Id.Equals(id)).FirstOrDefault();

                userShoppingCart.TicketsInShoppingCart.Remove(itemToDelete);

                this._shoppingCartRepositorty.Update(userShoppingCart);

                itemToDelete.Availability = true;

                this._ticketRepository.Update(itemToDelete);

                return true;
            }

            return false;
        }

        public bool deleteOrderedTicketFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.TicketsInShoppingCart.Where(z => z.Id.Equals(id)).FirstOrDefault();

                userShoppingCart.TicketsInShoppingCart.Remove(itemToDelete);

                this._shoppingCartRepositorty.Update(userShoppingCart);

                this._ticketRepository.Update(itemToDelete);

                return true;
            }

            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.UserCart;

            var AllTickets = userShoppingCart.TicketsInShoppingCart.ToList();

            var totalPrice = 0.0;


            foreach (var item in AllTickets)
            {
                totalPrice += item.Price;
            }


            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Tickets = AllTickets,
                TotalPrice = totalPrice
            };


            return scDto;
        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage message = new EmailMessage();
                message.MailTo = loggedInUser.Email;
                message.Subject = "Successfully created order";
                message.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser
                };

                this._orderRepositorty.Insert(order);


                foreach (var item in userShoppingCart.TicketsInShoppingCart)
                {
                    item.Order = order;
                    _ticketRepository.Update(item);

                }

                var result = userShoppingCart.TicketsInShoppingCart.ToArray();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Your order is completed. The order contains: ");
                var totalPrice = 0.0;

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];
                    totalPrice += item.Price;
                    sb.AppendLine(i.ToString() + ". Ticket for " + item.MovieProjection.Movie.Title + ", Seat: " + item.Seat +", Hall: " + item.MovieProjection.Hall
                        + ", DateTime: " + item.MovieProjection.DateTime +  " with price of: " + item.Price);
                }

                sb.AppendLine("Total Price: " + totalPrice.ToString());
                message.Content = sb.ToString();


                message.Content = sb.ToString();

                this._mailRepository.Insert(message);

                this._userRepository.Update(loggedInUser);

                return true;
            }
            return false;
        }
    }
}
