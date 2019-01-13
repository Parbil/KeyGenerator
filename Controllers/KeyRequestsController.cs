using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BitproKeyGen.Data;
using BitproKeyGen.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BitproKeyGen.Controllers
{
    [Authorize]
    public class KeyRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KeyRequestsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: KeyRequests
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId==null)
            {
                return NotFound();
            }
            

            return View(await _context.KeyRequest.Where(i => i.RequestedBy == userId).OrderByDescending(i=>i.Id).ToListAsync());

            //return View(await _context.KeyRequest.ToListAsync());
        }

        // GET: KeyRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }

            var keyRequest = await _context.KeyRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keyRequest == null)
            {
                return NotFound();
            }

            return View(keyRequest);
        }

        // GET: KeyRequests/Create
        public IActionResult Create()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: KeyRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductKey,LicenseKey,Duration,ShopName,ShopContactNo")] KeyRequest keyRequest)
        {
            
           if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                keyRequest.RequestedBy = userId;
                keyRequest.RequestedDate = DateTime.Now;
                keyRequest.Status = "Pending";
                _context.Add(keyRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(keyRequest);
        }

        // GET: KeyRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var keyRequest = await _context.KeyRequest.FindAsync(id);
            if (keyRequest == null)
            {
                return NotFound();
            }
            return View(keyRequest);
        }

        // POST: KeyRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductKey,LicenseKey,Duration,ShopName,ShopContactNo")] KeyRequest keyRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            if (id != keyRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    keyRequest.RequestedBy = userId;
                    keyRequest.RequestedDate = DateTime.Now;
                    keyRequest.Status = "Pending";
                    _context.Update(keyRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyRequestExists(keyRequest.Id))
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
            return View(keyRequest);
        }

        // GET: KeyRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var keyRequest = await _context.KeyRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keyRequest == null)
            {
                return NotFound();
            }

            return View(keyRequest);
        }

        // POST: KeyRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var keyRequest = await _context.KeyRequest.FindAsync(id);
            _context.KeyRequest.Remove(keyRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //adminindex
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AdminIndex()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Admin"))
            {
                return View(await _context.KeyRequest.OrderByDescending(i => i.Id).ToListAsync());
            }
            else return NotFound();

            //return View(await _context.KeyRequest.ToListAsync());
        }

        //pending
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Pending()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }


            return View(await _context.KeyRequest.Where(i => i.Status == "Pending").OrderByDescending(i => i.Id).ToListAsync());

            //return View(await _context.KeyRequest.ToListAsync());
        }

        //approve
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Approved()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }


            return View(await _context.KeyRequest.Where(i=>i.Status=="Approved").OrderByDescending(i => i.Id).ToListAsync());

            //return View(await _context.KeyRequest.ToListAsync());
        }

        //reject
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Rejected()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }


            return View(await _context.KeyRequest.Where(i=>i.Status=="Rejected").OrderByDescending(i => i.Id).ToListAsync());

            //return View(await _context.KeyRequest.ToListAsync());
        }

        //GET AdminReject
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AdminReject(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var keyRequest = await _context.KeyRequest.FindAsync(id);
            if (keyRequest == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {


                    keyRequest.Status = "Rejected";
                    _context.Update(keyRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyRequestExists(keyRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("AdminIndex","KeyRequests");
            }
            return View(keyRequest);
        }

        //adminApprove
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AdminApprove(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var keyRequest = await _context.KeyRequest.FindAsync(id);
          
            if (keyRequest == null)
            {
                return NotFound();
            }
           KeyRequest key = new KeyRequest();
            var data = _context.KeyRequest.Where(i => i.Id == id).SingleOrDefault();
            key.ProductKey = data.ProductKey;
            key.Duration = data.Duration;
          



            key.LicenseKey = Generate(data.ProductKey, data.Duration);
            return View(key);



        }




        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AdminApprove(KeyRequest key)
        {
            ModelState.Clear();
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }


            var keyRequest = await _context.KeyRequest.FindAsync(key.Id);
            if (keyRequest == null)
            {
                return NotFound();
            }



            key.LicenseKey = Generate(key.ProductKey, key.Duration);

            if (ModelState.IsValid)
            {
                try
                {


                    keyRequest.Status = "Approved";
                    keyRequest.ApprovedDate = DateTime.Now;
                    keyRequest.LicenseKey = key.LicenseKey;
                    keyRequest.Duration = key.Duration;
                    
                    _context.Update(keyRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KeyRequestExists(keyRequest.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(key);
        }




        // GET: KeyRequests/AdminDetails
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AdminDetails(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }

            var keyRequest = await _context.KeyRequest
                .FirstOrDefaultAsync(m => m.Id == id);
            if (keyRequest == null)
            {
                return NotFound();
            }

            return View(keyRequest);
        }






        private string Generate(string pkey, int days)
        {
            //using (LicenseServices key = new LicenseServices())
            //{

            //    return key.GenerateKey(pkey, days);

            //}
            return pkey+days;

        }






        private bool KeyRequestExists(int id)
        {
            return _context.KeyRequest.Any(e => e.Id == id);
        }


    }
}
