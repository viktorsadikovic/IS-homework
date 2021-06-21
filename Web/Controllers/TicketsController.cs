using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.DomainModels;
using Repository;
using Service.Interface;
using Domain.DTO;
using System.Security.Claims;
using Domain;
using GemBox.Document;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITicketService _ticketService;
        private readonly IProjectionService _projectionService;

        public TicketsController(ApplicationDbContext context, ITicketService ticketService, IProjectionService projectionService)
        {
            _context = context;
            _ticketService = ticketService;
            _projectionService = projectionService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMINISTRATOR,USER")]
        public IActionResult AddTicketToCart(Guid? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var ticket = _ticketService.GetDetailsForTicket(id);

            var result = this._ticketService.AddToShoppingCart(ticket, userId);

            if (result)
            {
                return RedirectToAction("Index", "Tickets");
            }

            return View(ticket);
        }

        // GET: Tickets
        [AllowAnonymous]
        public IActionResult Index()
        {
            var allTickets = _ticketService.GetAllTickets();
            return View(allTickets);
        }

        // GET: Tickets/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Projections = await _context.Projections
                                                .Include(z => z.Movie)
                                                .ToListAsync();
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Price,Availability,Id,MovieProjectionId,Seat")] TicketViewModel ticketDto)
        {
            if (ModelState.IsValid)
            {
                Ticket ticket = new Ticket();
                ticket.Price = ticketDto.Price;
                ticket.Seat = ticketDto.Seat;
                ticket.Availability = ticketDto.Availability;
                ticket.MovieProjection = _projectionService.GetDetailsForProjection(ticketDto.MovieProjectionId);
                ticket.Id = Guid.NewGuid();
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        // GET: Tickets/ExportTickets
        [Authorize(Roles = "ADMINISTRATOR")]
        public IActionResult ExportTickets()
        {
            ViewBag.Genres = Enum.GetValues(typeof(Genre));
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExportTickets([Bind("Genre")] ExportTicketsViewModel data)
        {
            string fileName = "ExportTickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            if (ModelState.IsValid)
            {
                List<Ticket> tickets = _ticketService.GetAllTickets().FindAll(z => z.MovieProjection.Movie.Genre == data.Genre);

                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet worksheet = workbook.Worksheets.Add("Tickets");

                    worksheet.Cell(1, 1).Value = "Ticket Id";
                    worksheet.Cell(1, 2).Value = "Seat";
                    worksheet.Cell(1, 3).Value = "Hall";
                    worksheet.Cell(1, 4).Value = "Movie";
                    worksheet.Cell(1, 5).Value = "Genre";
                    worksheet.Cell(1, 6).Value = "DateTime";
                    worksheet.Cell(1, 7).Value = "Price";

                    for (int i = 1; i <= tickets.Count(); i++)
                    {
                        var item = tickets[i - 1];

                        worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                        worksheet.Cell(i + 1, 2).Value = item.Seat;
                        worksheet.Cell(i + 1, 3).Value = item.MovieProjection.Hall;
                        worksheet.Cell(i + 1, 4).Value = item.MovieProjection.Movie.Title;
                        worksheet.Cell(i + 1, 5).Value = item.MovieProjection.Movie.Genre;
                        worksheet.Cell(i + 1, 6).Value = item.MovieProjection.DateTime;
                        worksheet.Cell(i + 1, 7).Value = item.Price;
                    }

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(content, contentType, fileName);
                    }

                }

            }
            return RedirectToAction("Index","Tickets");
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Price,Availability,Id")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(Guid id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}
