using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GLS_BlazorMVC_PoC.Data;
using GLS_BlazorMVC_PoC.Models;
using Microsoft.AspNetCore.Authorization;

namespace GLS_BlazorMVC_PoC.Controllers
{
    [Authorize()]
    public class PasswordsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;

        public PasswordsController(MyDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Passwords
        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.Passwords.Where(p => p.User.Email == User.Identity.Name);
            return View(await myDbContext.ToListAsync());
        }

        // GET: Passwords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Passwords == null)
            {
                return NotFound();
            }

            var password = await _context.Passwords
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (password == null)
            {
                return NotFound();
            }

            if (password.UserId != _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
            {
                return Unauthorized();
            }

            return View(password);
        }

        // GET: Passwords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Passwords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServiceName,EncryptedPassword,UserId")] Password password)
        {
            password.UserId = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;
            password.EncryptedPassword = Helpers.Encryptor.Encrypt(password.EncryptedPassword);
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                _context.Add(password);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", password.UserId);


            _logger.LogInformation("Successful Password creation attempt by user " + User.Identity.Name);

            return View(password);
        }

        // GET: Passwords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Passwords == null)
            {
                return NotFound();
            }

            var password = await _context.Passwords.FindAsync(id);
            if (password == null)
            {
                return NotFound();
            }

            if (password.UserId != _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
            {
                return Unauthorized();
            }

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", password.UserId);
            return View(password);
        }

        // POST: Passwords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceName,EncryptedPassword,UserId")] Password password)
        {
            if (id != password.Id)
            {
                return NotFound();
            }

            password.UserId = _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;
            password.EncryptedPassword = Helpers.Encryptor.Encrypt(password.EncryptedPassword);
            ModelState.Remove("User");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(password);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Successful Password edition attempt by user " + User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasswordExists(password.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", password.UserId);
            return View(password);
        }

        // GET: Passwords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Passwords == null)
            {
                return NotFound();
            }

            var password = await _context.Passwords
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (password == null)
            {
                return NotFound();
            }


            if (password.UserId != _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
            {
                return Unauthorized();
            }

            return View(password);
        }

        // POST: Passwords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Passwords == null)
            {
                return Problem("Entity set 'MyDbContext.Passwords'  is null.");
            }
            var password = await _context.Passwords.FindAsync(id);

            if (password != null)
            {

                if (password.UserId != _context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
                {
                    return Unauthorized();
                }
                _context.Passwords.Remove(password);
                _logger.LogInformation("Successful Password removal attempt by user " + User.Identity.Name);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasswordExists(int id)
        {
          return (_context.Passwords?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
