using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BitproKeyGen.Data;
using BitproKeyGen.Models;

namespace BitproKeyGen.Controllers
{
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
            return View(await _context.KeyRequest.ToListAsync());
        }

        // GET: KeyRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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
            return View();
        }

        // POST: KeyRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductKey,LicenseKey,Duration")] KeyRequest keyRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(keyRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(keyRequest);
        }

        // GET: KeyRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductKey,LicenseKey,Duration")] KeyRequest keyRequest)
        {
            if (id != keyRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        private bool KeyRequestExists(int id)
        {
            return _context.KeyRequest.Any(e => e.Id == id);
        }
    }
}
