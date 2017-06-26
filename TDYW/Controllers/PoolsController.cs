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
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Security.Claims;

namespace TDYW.Controllers
{
    public class PoolsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly UserManager<ApplicationUser> _userManager;
        public PoolsController(ApplicationDbContext context)
        {
            _context = context;
            //_userManager = userManager;
        }

        // GET: Pools
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //foreach(System.Security.Claims.Claim x in User.Claims)
            //{
            //    Debug.WriteLine("");
            //}

            //var user = await _userManager.GetUserAsync(User);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //string _userId = User.Identity.Name;
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "startdate" ? "startdate_desc" : "startdate";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            IQueryable<Pool> pools;

            if(!string.IsNullOrEmpty(userId))
            {
                pools = _context.Pools.Where(p => p.Private == false || p.UserId == userId || p.Players.Any(a => a.UserId == userId));
            }
            else
            {
                pools = _context.Pools.Where(p => p.Private == false);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                pools = pools.Where(s => s.Name.Contains(searchString) || s.Description.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "startdate_desc":
                    pools = pools.OrderByDescending(s => s.StartDate);
                    break;
                case "startdate":
                    pools = pools.OrderBy(s => s.StartDate);
                    break;
                case "name_desc":
                    pools = pools.OrderByDescending(s => s.Name);
                    break;
                default:
                    pools = pools.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Pool>.CreateAsync(pools.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Pools/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pool = await _context.Pools
                                .Include(i => i.Players)
                                .ThenInclude(t => t.ApplicationUser)
                                .SingleOrDefaultAsync(m => m.Id == id);
            if (pool == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var player = pool.Players.FirstOrDefault(p => p.UserId == userId);   //todo: make sure a user can only have one player per pool
            //var user = await _userManager.GetUserAsync(User);
            if (pool.Private)
            {
                if(string.IsNullOrEmpty(userId) || (pool.UserId != userId && player == null))
                {
                    return Unauthorized();
                } 
            }
            ViewData["userId"] = userId;
            ViewData["playerId"] = player != null ? player.Id : 0;
            
            return View(pool);
        }

        // GET: Pools/Create
        [Authorize]
        public IActionResult Create()
        {
            //var pool = new Pool();
            return View(new Pool());
        }

        // POST: Pools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,StartDate,EndDate,OpenEnrollment,PicksPerPlayer")] Pool pool) //,Private,OversPerPlayer,AllowPluralityVote,RequireTwoThirdsVote,FixedAgeBonus,FixedAgeBonusMinuend,WeightedAgeBonus,WeightedAgeBonusFactor,WeightedRankBonus,WeightedRankBonusFactor,FixedRankBonus,FixedRankBonusFactor
        {
            if (ModelState.IsValid)
            {
                //var user = await _userManager.GetUserAsync(User);
                if(_context.Pools.Any(p=>p.Name == pool.Name))
                {
                    ModelState.AddModelError("Name", "There is already a pool with the specified name.");
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.Id == userId);
                if(user != null)
                {
                    user.Pools.Add(pool);
                    await _context.SaveChangesAsync();
                } else
                {
                    return Unauthorized();
                }
                return RedirectToAction("Index");
            }
            return View(pool);
        }

        // GET: Pools/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pool = await _context.Pools.SingleOrDefaultAsync(m => m.Id == id);
            if (pool == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user = await _userManager.GetUserAsync(User);

            if (pool.UserId != userId)
            {
                return Unauthorized();
            }
            return View(pool);
        }

        // POST: Pools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,OpenEnrollment,PicksPerPlayer")] Pool poolNew) //,Private,OversPerPlayer,AllowPluralityVote,RequireTwoThirdsVote,FixedAgeBonus,FixedAgeBonusMinuend,WeightedAgeBonus,WeightedAgeBonusFactor,WeightedRankBonus,WeightedRankBonusFactor,FixedRankBonus,FixedRankBonusFactor
        {
            if (id != poolNew.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.Pools.Any(p => p.Name == poolNew.Name && p.Id != poolNew.Id))
                {
                    ModelState.AddModelError("Name", "There is already a pool with the specified name.");
                }
                var poolOld = await _context.Pools.SingleOrDefaultAsync(m => m.Id == id);
                if(poolOld == null || poolOld.Id != poolNew.Id)
                {
                    return NotFound();
                }

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var user = await _userManager.GetUserAsync(User);

                if (poolOld.UserId != userId)
                {
                    return Unauthorized();
                }
                poolNew.UserId = userId;
                _context.Entry(poolOld).CurrentValues.SetValues(poolNew);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(poolNew);
        }

        // GET: Pools/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pool = await _context.Pools.SingleOrDefaultAsync(m => m.Id == id);
            if (pool == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user = await _userManager.GetUserAsync(User);
            //string _userId = User.Identity.Name;

            if (pool.UserId != userId)
            {
                return Unauthorized();
            }
            return View(pool);
        }

        // POST: Pools/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pool = await _context.Pools.SingleOrDefaultAsync(m => m.Id == id);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var user = await _userManager.GetUserAsync(User);

            if (pool.UserId != userId)
            {
                return Unauthorized();
            }
            _context.Pools.Remove(pool);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }

   


}
