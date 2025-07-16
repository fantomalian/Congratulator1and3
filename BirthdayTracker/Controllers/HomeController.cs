using BirthdayTracker.Data;
using BirthdayTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BirthdayTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var people = await _context.People.ToListAsync();

            // Сегодняшние дни рождения
            var todaysBirthdays = people
                .Where(p => p.BirthDate.Month == today.Month && p.BirthDate.Day == today.Day)
                .ToList();

            // Ближайшие дни рождения (30 дней)
            var upcomingBirthdays = people
                .Where(p => IsUpcomingBirthday(p.BirthDate))
                .OrderBy(p => p.BirthDate)
                .Take(10)
                .ToList();

            ViewBag.TodaysBirthdays = todaysBirthdays;
            ViewBag.UpcomingBirthdays = upcomingBirthdays;

            return View();
        }

        private bool IsUpcomingBirthday(DateTime birthDate)
        {
            var today = DateTime.Today;
            var nextBirthday = new DateTime(today.Year, birthDate.Month, birthDate.Day);

            if (nextBirthday < today)
                nextBirthday = nextBirthday.AddYears(1);

            return (nextBirthday - today).TotalDays <= 30 && (nextBirthday - today).TotalDays!=0;
        }
    }
}