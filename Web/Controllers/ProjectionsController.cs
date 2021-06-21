using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.DomainModels;
using Repository;
using Domain.DTO;
using Service.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    public class ProjectionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectionService _projectionService;

        public ProjectionsController(ApplicationDbContext context, 
                                     IProjectionService projectionService)
        {
            _context = context;
            _projectionService = projectionService;
        }

        // GET: Projections
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projections.ToListAsync());
        }

        // GET: Projections/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projection = await _context.Projections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projection == null)
            {
                return NotFound();
            }

            return View(projection);
        }

        // GET: Projections/Create
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Movies = await _context.Movies.ToListAsync();
            return View();
        }

        // POST: Projections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DateTime,MovieId,Hall,Price")] MovieProjectionViewModel projection)
        {
            if (ModelState.IsValid)
            {
                _projectionService.CreateNewProjection(projection);
    
                return RedirectToAction(nameof(Index));
            }
            return View(projection);
        }

        // GET: Projections/Edit/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projection = await _context.Projections.FindAsync(id);
            if (projection == null)
            {
                return NotFound();
            }
            return View(projection);
        }

        // POST: Projections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DateTime,Id")] Projection projection)
        {
            if (id != projection.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectionExists(projection.Id))
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
            return View(projection);
        }

        // GET: Projections/Delete/5
        [Authorize(Roles = "ADMINISTRATOR")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projection = await _context.Projections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projection == null)
            {
                return NotFound();
            }

            return View(projection);
        }

        // POST: Projections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var projection = await _context.Projections.FindAsync(id);
            _context.Projections.Remove(projection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectionExists(Guid id)
        {
            return _context.Projections.Any(e => e.Id == id);
        }
    }
}
