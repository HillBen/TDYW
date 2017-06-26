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
    public class RecipientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private string _userId;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;

        public RecipientsController(ApplicationDbContext context, IEmailSender emailSender, ILoggerFactory loggerFactory)
        {
            _context = context;
            _userId = User.Identity.Name;
            _emailSender = emailSender;
            _logger = loggerFactory.CreateLogger<RecipientsController>();
        }

        // GET: Recipients
        public async Task<IActionResult> Index(int? invitationId)
        {
            if(invitationId == null)
            {
                return NotFound();
            }
            var invitation = await _context.Invitations.Include(i=>i.Recipients).SingleOrDefaultAsync(w => w.Id == invitationId.Value);
            if(invitation == null)
            {
                return NotFound();
            }
            if(invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            if(invitation.Pool.IsPreGame() && invitation.Recipients.Any(a=>a.DateSent == null))
            {
                foreach(Recipient i in invitation.Recipients.Where(w=>w.DateSent == null))
                {
                    try
                    {
                        _emailSender.SendEmailAsync(i.Email, i.Invitation.Subject, i.Invitation.Content).Wait();
                        i.DateSent = DateTime.Now;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(1, ex, "Invite Email Error: recipientId=" + i.Id + " to " + i.Email);
                    } 
                }
            }
            return View(invitation.Recipients.ToList());
        }

        // GET: Recipients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipients.SingleOrDefaultAsync(m => m.Id == id);
            if (recipient == null)
            {
                return NotFound();
            }
            if(recipient.Invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            return View(recipient);
        }

        // GET: Recipients/Create
        public IActionResult Create(int? invitationId)
        {
            if (invitationId == null)
            {
                return NotFound();
            }
            ViewData["InvitationId"] = invitationId;
            return View();
        }

        // POST: Recipients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,InvitationId")] Recipient recipient)
        {
            if (ModelState.IsValid)
            {
                var invitation = await _context.Invitations.SingleOrDefaultAsync(w => w.Id == recipient.InvitationId);
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
                    _context.Add(recipient);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    ModelState.AddModelError("PreGameException", "This pool has started. The invitation period is over.");
                }
                return RedirectToAction("Index", new { invitationId = invitation.Id });
            }
            return View(recipient);
        }

        // GET: Recipients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var recipient = await _context.Recipients.SingleOrDefaultAsync(m => m.Id == id);
            if (recipient == null)
            {
                return NotFound();
            }
            if(recipient.Invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            return View(recipient);
        }

        // POST: Recipients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,InvitationId")] Recipient recipientNew)
        {
            if (id != recipientNew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var recipientOld = await _context.Recipients.SingleOrDefaultAsync(m => m.Id == id);
                if (recipientOld == null)
                {
                    return NotFound();
                }
                if (recipientOld.Invitation.Pool.UserId != _userId)
                {
                    return Unauthorized();
                }
                if (recipientOld.Invitation.Pool.IsPreGame())
                {
                    _context.Entry(recipientOld).CurrentValues.SetValues(recipientNew);
                    //_context.Update(recipientNew);
                    _context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("PreGameException", "This pool has started. The invitation period is over.");
                }
                return RedirectToAction("Index", new { @invitationId = recipientOld.InvitationId });
            }
            return View(recipientNew);
        }

        // GET: Recipients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipient = await _context.Recipients
                .Include(i => i.Invitation)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (recipient == null)
            {
                return NotFound();
            }
            if(recipient.Invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            return View(recipient);
        }

        // POST: Recipients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipient = await _context.Recipients.SingleOrDefaultAsync(m => m.Id == id);
            if(recipient.Invitation.Pool.UserId != _userId)
            {
                return Unauthorized();
            }
            int invitationId = recipient.InvitationId;
            _context.Recipients.Remove(recipient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { @invitationId = invitationId });
        }

        private bool RecipientExists(int id)
        {
            return _context.Recipients.Any(e => e.Id == id);
        }
    }
}
