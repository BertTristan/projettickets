using projetTicket.Data;
using projetTicket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetTicket.Models;
using System.Diagnostics;

namespace projetTicket.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly projetTicketContext _context;

        private string SessionUserName = "SessionUserName";
        private string SessionUserId = "SessionUserId";
        private string SessionRole = "SessionRole";

        public HomeController(ILogger<HomeController> logger, projetTicketContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {

            //HttpContext.Session.SetString(SessionUserName, "a@a.com");
            // HttpContext.Session.SetInt32(SessionUserId, 42);

            return View();
        }

        public IActionResult Privacy()
        {
            // HttpContext.Session.GetString(SessionUserName);

            return View();
        }

        public IActionResult Signin()
        {
            return View(new Utilisateur());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin([Bind("Email,Password")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                utilisateur.Password = BCrypt.Net.BCrypt.HashPassword(utilisateur.Password);
                utilisateur.Role = "Client";
                _context.Add(utilisateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilisateur);
        }

        public IActionResult Login()
        {
            return View(new Utilisateur());
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {

                if (utilisateur.Email == null || utilisateur.Password == null)
                {
                    return View(utilisateur);
                }

                var utilisateurBdd = await _context.Utilisateur
                    .FirstOrDefaultAsync(u => u.Email == utilisateur.Email);

                if (utilisateurBdd == null)
                {
                    return View(utilisateur);
                }

                if (!BCrypt.Net.BCrypt.Verify(utilisateur.Password, utilisateurBdd.Password))
                {
                    return View(utilisateur);
                }

                HttpContext.Session.SetString(SessionUserName, utilisateurBdd.Email);
                HttpContext.Session.SetString(SessionRole, utilisateurBdd.Role);

                return RedirectToAction(nameof(Index));
            }
            return View(utilisateur);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}