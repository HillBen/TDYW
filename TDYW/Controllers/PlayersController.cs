using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TDYW.Data;
using TDYW.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TDYW.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        //private string _userId;

        public PlayersController(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger<PlayersController>();
            //_userId = User.Identity.Name;
        }


        // GET: Players
        public async Task<IActionResult> Index(int? poolId)
        {
            if (poolId == null)
            {
                return NotFound();
            }
            var pool = await _context.Pools.SingleOrDefaultAsync(s => s.Id == poolId);
            if(pool == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (pool.Private && pool.UserId != userId && !pool.UserIsPlaying(userId))
            {
                return Unauthorized();
            }
            return View(pool.Players.ToList());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var player = await _context.Players
                .SingleOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (player.Pool.Private == false || player.Pool.UserId == userId || player.Pool.UserIsPlaying(userId))
            {
                return View(player);
            }
            else
            {
                return Unauthorized();
            }
            
        }

        

        
        // GET: Players/Create
        [Authorize]
        public async Task<IActionResult> Create(int? poolId)
        {
            if(poolId == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var player = await _context.Players.SingleOrDefaultAsync(p => p.PoolId == poolId && p.UserId == userId);
            if(player == null)
            {
                var pool = await _context.Pools.SingleOrDefaultAsync(s => s.Id == poolId);
                if (!pool.IsPreGame())
                {
                    ModelState.AddModelError("Error", "Players cannot join a pool after it has started.");
                    return BadRequest(ModelState);
                }
                else if (pool.Private && pool.UserId != userId)
                {
                    ModelState.AddModelError("Error", "You cannot join a private pool without an invitation.");
                    return BadRequest(ModelState);
                }
                else
                {
                    player = new Player { UserId = userId, PoolId = poolId.Value };
                    await _context.Players.AddAsync(player);
                    await _context.SaveChangesAsync();
                    await _context.Entry(player).GetDatabaseValuesAsync();
                }
            }
            if(player != null)
            {
                return View("Details", player);
            }
            ModelState.AddModelError("Error", "An error occured while attempting to join a pool (id=" + poolId + ").");
            return BadRequest(ModelState);
        }

   

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.ApplicationUser)
                .Include(p => p.Pool)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (player.UserId != userId)
            {
                return Unauthorized();
            }
            if (!player.Pool.IsPreGame())
            {
                ModelState.AddModelError("Error", "Players cannot leave a pool after it has started.");
                return BadRequest(ModelState);
            }
            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.SingleOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (player.UserId != userId)
            {
                return Unauthorized();
            }
            if (!player.Pool.IsPreGame())
            {
                ModelState.AddModelError("Error", "Players cannot leave a pool after it has started.");
                return BadRequest(ModelState);
            }
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
