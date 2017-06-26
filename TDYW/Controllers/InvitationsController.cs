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
using Microsoft.AspNetCore.DataProtection;
using System.Security.Claims;
using TDYW.Services;

namespace TDYW.Controllers
{
    public class InvitationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDataProtectionProvider _dataProtectionProvider;
        public InvitationsController(ApplicationDbContext context, IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtectionProvider = dataProtectionProvider;
        }

        // GET: Invitations
        [Authorize]
        public async Task<IActionResult> Index(int? poolId)
        {
            if (poolId == null)
            {
                return NotFound();
            }
            var pool = await _context.Pools.Include(i=>i.Invitations).SingleOrDefaultAsync(m => m.Id == poolId);
            if(pool == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (pool.UserId != userId)
            {
                return Unauthorized();
            }
            return View(pool.Invitations.ToList());
        }

        // GET: Invitations/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invitations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invitation == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (invitation.Pool.UserId != userId)
            {
                return Unauthorized();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        [Authorize]
        public IActionResult Create(int? poolId)
        {
            if(poolId == null)
            {
                return NotFound();
            }
            ViewData["PoolId"] = poolId;
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Subject,Content,OpenInvite,PoolId")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                var pool = await _context.Pools.SingleOrDefaultAsync(m => m.Id == invitation.PoolId);
                if(pool == null)
                {
                    return NotFound();
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (pool.UserId != userId)
                {
                    return Unauthorized();
                }
                if (pool.IsPreGame())
                {
                    _context.Add(invitation);
                    await _context.SaveChangesAsync();
                    await _context.Entry(invitation).GetDatabaseValuesAsync();
                    return View("Details", invitation);
                }
                else
                {
                    ModelState.AddModelError("PreGameException","This pool has started. The invitation period is over.");
                }
                return RedirectToAction("Index", new { poolId = invitation.PoolId });
            }
            return View(invitation);
        }

        // GET: Invitations/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invitations.SingleOrDefaultAsync(m => m.Id == id);
            if (invitation == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (invitation.Pool.UserId != userId)
            {
                return Unauthorized();
            }
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Content,OpenInvite")] Invitation invitationNew)
        {
            if (id != invitationNew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var invitationOld = await _context.Invitations.SingleOrDefaultAsync(m => m.Id == id);
                if (invitationOld == null)
                {
                    return NotFound();
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (invitationOld.Pool.UserId != userId)
                {
                    return Unauthorized();
                }
                if (invitationOld.Pool.IsPreGame())
                {
                    _context.Entry(invitationOld).CurrentValues.SetValues(invitationNew);
                    //_context.Update(invitationNew);
                    _context.SaveChanges();
                } else
                {
                    ModelState.AddModelError("PreGameException", "This pool has started. The invitation period is over.");
                }
                return RedirectToAction("Index", new { @poolId = invitationOld.PoolId });
            }
            return View(invitationNew);
        }

        // GET: Invitations/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invitations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invitation == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (invitation.Pool.UserId != userId)
            {
                return Unauthorized();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var invitation = await _context.Invitations.SingleOrDefaultAsync(m => m.Id == id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (invitation.Pool.UserId != userId)
            {
                return Unauthorized();
            }
            _context.Invitations.Remove(invitation);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        [Authorize]
        public async Task<IActionResult> Rsvp(int? id, string token)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invitations
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invitation == null)
            {
                return BadRequest();
            }
            if (invitation.Pool.Private)
            {
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest();
                }
                var protector = _dataProtectionProvider.CreateProtector("InvitationToken");
                int secretId;
                string output = protector.Unprotect(token);
                if (int.TryParse(output, out secretId))
                {
                    if (invitation.OpenInvite == false)
                    {
                        var recipient = invitation.Recipients.SingleOrDefault(i => i.Id == secretId);
                        if (recipient == null || recipient.DateJoined != null)
                        {
                            return BadRequest();
                        }
                        recipient.DateJoined = DateTime.Now;
                    }
                    else if (secretId != invitation.Id)// && !invitation.Recipients.Any(i => i.Id == secretId))
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    //ModelState.AddModelError("InvalidToken", "The invitation token was not valid.");
                    return BadRequest();
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var player = invitation.Pool.Players.SingleOrDefault(p => p.PoolId == invitation.PoolId && p.UserId == userId);
                    if (player == null)
                    {
                        player = new Player { UserId = userId };
                        invitation.Pool.Players.Add(player);
                        _context.SaveChanges();
                        //await _context.SaveChangesAsync();
                        //await _context.Entry(player).GetDatabaseValuesAsync();
                    }
                    return RedirectToAction("Details", "Pools", new { id = invitation.PoolId });
                }
            }
            return Unauthorized();

        }

    }
}
