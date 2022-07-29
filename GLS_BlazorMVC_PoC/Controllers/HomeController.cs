using GLS_BlazorMVC_PoC.Data;
using GLS_BlazorMVC_PoC.Models;
using GLS_BlazorMVC_PoC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace GLS_BlazorMVC_PoC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _context;

        public HomeController(ILogger<HomeController> logger, MyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var checkDbUser = _context.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            if (checkDbUser == null)
            {
                ModelState.AddModelError("UserError", "Email not found!");
                return View();
            }
            if (!Helpers.Hasher.VerifyHashedPassword(checkDbUser.HashedPassword, user.HashedPassword))
            {
                ModelState.AddModelError("PassError", "Wrong password!");
                return View();
            }
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email)
                        };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimPrincipal, new AuthenticationProperties());

            await HttpContext.AuthenticateAsync("Cookies");

            HttpContext.User = claimPrincipal;

            _logger.LogInformation("Successful login attempt by user " + user.Email);
            return View("Index");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            _logger.LogInformation("Successful logout attempt by user " + User.Identity.Name);

            return RedirectToAction("Index");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            var checkDbUser = _context.Users.Where(x => x.Email == user.Email).FirstOrDefault();
            if (checkDbUser != null)
            {
                ModelState.AddModelError("UserError", "Email already registered!");
                return View();
            }
            ModelState.Remove("Username");
            if (ModelState.IsValid)
            {
                user.HashedPassword = Helpers.Hasher.HashPassword(user.HashedPassword);
                _context.Add(user);
                await _context.SaveChangesAsync();

                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email)
                        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimPrincipal, new AuthenticationProperties());

                await HttpContext.AuthenticateAsync("Cookies");

                HttpContext.User = claimPrincipal;

                _logger.LogInformation("Successful register attempt by user " + user.Email);

                return View("Index");
            }
            return View(user);
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}