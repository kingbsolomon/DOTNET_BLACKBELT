using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BeltExam.Models;
using Microsoft.AspNetCore.Identity;
using BeltExam.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BeltExam.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext _context;
        private User GetUserFromDB()
        {
            return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        }

        public HomeController(HomeContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("newuser")]
        public IActionResult NewUser(User reg)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == reg.Email))
                {
                    ModelState.AddModelError("Email", "This Email Address is Already Taken.");
                    return View("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                reg.Password = Hasher.HashPassword(reg, reg.Password);
                _context.Users.Add(reg);
                _context.SaveChanges();

                HttpContext.Session.SetInt32("UserId", reg.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Register");
        }

        [HttpGet("new/event")]
        public IActionResult NewEvent()
        {
            return View();
        }

        [HttpPost("create/event")]
        public IActionResult CreateEvent(Event e)
        {
            User userInDb = GetUserFromDB();
            if (userInDb != null)
            {
                if (ModelState.IsValid)
                {
                    e.UserId = userInDb.UserId;
                    _context.Events.Add(e);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
                return View("NewEvent");
            }
            return RedirectToAction("Logout");
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User UserInDb = GetUserFromDB();
            if (UserInDb == null)
            {
                return RedirectToAction("Logout");
            }
            ViewBag.User = UserInDb;
            List<Event> AllEvents = _context.Events
                                        .Where(d => d.Start > DateTime.Now)
                                        .OrderBy(d => d.Start)
                                        .Include(p => p.Creator)
                                        .Include(p => p.Attendees)
                                        .ThenInclude(r => r.Participant)
                                        .ToList();
            return View(AllEvents);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                User user = _context.Users.FirstOrDefault(u => u.Email == loginUser.LoginEmail);
                if (user == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                    return View("Index");
                }

                PasswordHasher<LoginUser> hash = new PasswordHasher<LoginUser>();
                var result = hash.VerifyHashedPassword(loginUser, user.Password, loginUser.LoginPassword);

                if (result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                    return View("Index");
                }

                HttpContext.Session.SetInt32("UserId", user.UserId);
                return RedirectToAction("Dashboard");

            }

            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("join/{userId}/{eventId}")]
        public IActionResult Join(int userId, int eventId)
        {
            RSVP attend = new RSVP();
            attend.UserId = userId;
            attend.EventId = eventId;
            _context.RSVPS.Add(attend);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("unjoin/{userId}/{eventId}")]
        public IActionResult UnJoin(int userId, int eventId)
        {
            RSVP attend = _context.RSVPS.FirstOrDefault(e => e.UserId == userId && e.EventId == eventId);
            _context.RSVPS.Remove(attend);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("delete/{eventId}")]
        public IActionResult DeleteEvent(int eventId)
        {
            Event delEvent = _context.Events.FirstOrDefault(e => e.EventId == eventId);
            _context.Events.Remove(delEvent);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("event/{eventId}")]
        public IActionResult Activity(int eventId)
        {
            User UserInDb = GetUserFromDB();
            if (UserInDb == null)
            {
                return RedirectToAction("Logout");
            }
            ViewBag.User = UserInDb;
            Event show = _context.Events
                                .Include(a => a.Attendees)
                                .ThenInclude( p => p.Participant)
                                .Include (c => c.Creator)
                                .FirstOrDefault(e => e.EventId == eventId);
            return View(show);
        }
    }
}
