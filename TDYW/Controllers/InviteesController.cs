using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TDYW.Data;
using TDYW.Models;
using Microsoft.AspNetCore.Authorization;
using TDYW.Services;
using Microsoft.Extensions.Logging;

namespace TDYW.Controllers
{
    [Authorize]
    public class InviteesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string _userId;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public InviteesController(ApplicationDbContext context, IEmailSender emailSender, ILoggerFactory loggerFactory)
        {
            _context = context;
            _userId = User.Identity.Name;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<InviteesController>();
        }

        // GET: Invitees
        public async Task<IActionResult> Index(int? invitationId)
        {
            if(invitationId == null)
            {
                return NotFound();
            }
            var invitation = await _context.Invitations.Include(i=>i.Invitees).SingleOrDefaultAsync(w => w.Id == invitationId.Value);
            if(invitation == null)
            {
                return NotFound();
            }
            if(invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            if(invitation.Pool.IsPreGame() && invitation.Invitees.Any(a=>a.DateSent == null))
            {
                foreach(Invitee i in invitation.Invitees.Where(w=>w.DateSent == null))
                {
                    try
                    {
                        _emailSender.SendEmailAsync(i.Email, i.Invitation.Subject, i.Invitation.Content).Wait();
                        i.DateSent = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(1, ex, "Invite Email Error: inviteeId=" + i.Id + " to " + i.Email);
                    } 
                }
            }
            return View(invitation.Invitees.ToList());
        }

        // GET: Invitees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitee = await _context.Invitees.SingleOrDefaultAsync(m => m.Id == id);
            if (invitee == null)
            {
                return NotFound();
            }
            if(invitee.Invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            return View(invitee);
        }

        // GET: Invitees/Create
        public IActionResult Create(int? invitationId)
        {
            if (invitationId == null)
            {
                return NotFound();
            }
            ViewData["InvitationId"] = invitationId;
            return View();
        }

        // POST: Invitees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,InvitationId")] Invitee invitee)
        {
            if (ModelState.IsValid)
            {
                var invitation = await _context.Invitations.SingleOrDefaultAsync(w => w.Id == invitee.InvitationId);
                if (invitation == null)
                {
                    return NotFound();
                }
                if (invitation.Pool.UserId != _userId)
                {
                    return Unauthorized();
                }
                if(invitation.Pool.IsPreGame())
                {
                    _context.Add(invitee);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("PreGameException", "This pool has started. The invitation period is over.");
                }
                return RedirectToAction("Index", new { invitationId = invitation.Id });
            }
            return View(invitee);
        }

        // GET: Invitees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var invitee = await _context.Invitees.SingleOrDefaultAsync(m => m.Id == id);
            if (invitee == null)
            {
                return NotFound();
            }
            if(invitee.Invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            return View(invitee);
        }

        // POST: Invitees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,InvitationId")] Invitee inviteeNew)
        {
            if (id != inviteeNew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var inviteeOld = await _context.Invitees.SingleOrDefaultAsync(m => m.Id == id);
                if (inviteeOld == null)
                {
                    return NotFound();
                }
                if (inviteeOld.Invitation.Pool.UserId != _userId)
                {
                    return Unauthorized();
                }
                if (inviteeOld.Invitation.Pool.IsPreGame())
                {
                    _context.Entry(inviteeOld).CurrentValues.SetValues(inviteeNew);
                    //_context.Update(inviteeNew);
                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("PreGameException", "This pool has started. The invitation period is over.");
                }
                return RedirectToAction("Index", new { @invitationId = inviteeOld.InvitationId });
            }
            return View(inviteeNew);
        }

        // GET: Invitees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitee = await _context.Invitees
                .Include(i => i.Invitation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invitee == null)
            {
                return NotFound();
            }
            if(invitee.Invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            return View(invitee);
        }

        // POST: Invitees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invitee = await _context.Invitees.SingleOrDefaultAsync(m => m.Id == id);
            if(invitee.Invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            int invitationId = invitee.InvitationId;
            _context.Invitees.Remove(invitee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { @invitationId = invitationId });
        }

        private bool InviteeExists(int id)
        {
            return _context.Invitees.Any(e => e.Id == id);
        }
    }
}
