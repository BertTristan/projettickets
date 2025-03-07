using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using projetTicket.Data;
using projetTicket.Models;

namespace projetTicket.Controllers
{
    public class TicketsController : Controller
    {
        private readonly projetTicketContext _context;

        public TicketsController(projetTicketContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var userEmail = HttpContext.Session.GetString("SessionUserName");
            var userRole = HttpContext.Session.GetString("SessionRole");

            if (userRole == "Admin")
            {
                return View(await _context.Ticket.ToListAsync());
            }
            
            var tickets = await _context.Ticket.Where(t => t.AssignedTo == userEmail).ToListAsync();
            return View(tickets);
        }

        public async Task<IActionResult> Resolved(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var ticket = await _context.Ticket.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }
            ticket.IsResolved = true;

            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Statuses = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>().ToList();
            var userEmails = _context.Utilisateur.Select(u => u.Email).ToList();
            ViewBag.UserEmails = new SelectList(userEmails);
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Status,CreationDate,IsResolved,AssignedTo,ResolutionDate")] Ticket ticket)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewBag.Statuses = Enum.GetValues(typeof(TicketStatus)).Cast<TicketStatus>().ToList();
            var userEmails = _context.Utilisateur.Select(u => u.Email).ToList();
            ViewBag.UserEmails = new SelectList(userEmails);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Status,CreationDate,IsResolved,AssignedTo,ResolutionDate")] Ticket ticket)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Ticket
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("SessionRole") == null || HttpContext.Session.GetString("SessionRole") != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
            var ticket = await _context.Ticket.FindAsync(id);
            if (ticket != null)
            {
                _context.Ticket.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Ticket.Any(e => e.Id == id);
        }
    }
}
