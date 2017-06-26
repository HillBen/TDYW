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

        public InvitationsController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: Invitations
        [Authorize]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pool = await _context.Pools.Include(i=>i.Invitations).SingleOrDefaultAsync(m => m.Id == id);
            if(pool == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (pool.UserId != userId)
            {
                return Unauthorized();
            }
            if (pool.Invitations.Any())
            {
                return View(pool);
            }
            else
            {
                return RedirectToAction("Create", new { @id = id });
            }
            
        }

        // GET: Invitations/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invitations.Include(i=>i.Pool)
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
            //var protector = _dataProtectionProvider.CreateProtector("InvitationToken");
            //invitation.RsvpUrl = protector.Protect(id.Value.ToString());
            return View(invitation);
        }

        // GET: Invitations/Create
        [Authorize]
        public async Task<IActionResult> Create(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var pool = await _context.Pools.SingleOrDefaultAsync(p => p.Id == id.Value);
            if(pool == null)
            {
                return NotFound();
            }
            ViewData["PoolId"] = id;
            if (!pool.OpenEnrollment)
            {
                //create a default random secret
                Random random = new Random();
                const string choiceCharacters =
                            "ABCDEFGHJKLMNPQRTUVWXY" +
                            "abcdefghijkmnpqrtuvwxy" +
                            "346789";
                ViewData["Secret"] = new string(Enumerable.Repeat(choiceCharacters, 8).Select(s => s[random.Next(s.Length)]).ToArray());
            }
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Subject,Content,Secret,PoolId")] Invitation invitation)
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
                    return RedirectToAction("Details", new { @id = invitation.Id });
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

            var invitation = await _context.Invitations.Include(i=>i.Pool).SingleOrDefaultAsync(m => m.Id == id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Content,Secret,PoolId")] Invitation invitationNew)
        {
            if (id != invitationNew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var invitationOld = await _context.Invitations.Include(i => i.Pool).SingleOrDefaultAsync(m => m.Id == id);
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
                return RedirectToAction("Index", new { @id = invitationOld.PoolId });
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
        public async Task<IActionResult> Rsvp(int? id, string secret)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invitation = await _context.Invitations.Include(i=>i.Pool)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (invitation == null)
            {
                return BadRequest();
            }
            if (!invitation.Pool.OpenEnrollment)
            {
                if (string.IsNullOrEmpty(secret) || secret != invitation.Secret)
                {
                    return BadRequest();
                }

            }
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.ApplicationUsers.Include(i=>i.Players).SingleOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return Unauthorized();
                }
                if(!user.Players.Any(p=>p.PoolId == invitation.PoolId))
                {
                    user.Players.Add(new Player { PoolId = invitation.PoolId });
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Details", "Pools", new { id = invitation.PoolId });
                //if (!string.IsNullOrEmpty(userId))
                //{
                //    var player = invitation.Pool.Players.SingleOrDefault(p => p.PoolId == invitation.PoolId && p.UserId == userId);
                //    if (player == null)
                //    {
                //        player = new Player { UserId = userId, PoolId = invitation.PoolId };
                //        await _context.Players.AddAsync(player);
                //        await _context.SaveChangesAsync();
                //        //await _context.SaveChangesAsync();
                //        await _context.Entry(player).GetDatabaseValuesAsync();
                //    }
                //    return RedirectToAction("Details", "Pools", new { id = invitation.PoolId });
                //}
            }
            return Unauthorized();

        }

    }
}
