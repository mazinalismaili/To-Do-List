using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            /*Priority priority = new Priority();
            priority.Id = Guid.NewGuid().ToString();
            priority.Name = "Low";

            _context.Add(priority);
            _context.SaveChanges();

            Priority priority2 = new Priority();
            priority2.Id = Guid.NewGuid().ToString();
            priority2.Name = "High";

            _context.Add(priority2);
            _context.SaveChanges();

            Status status = new Status();
            status.Id = Guid.NewGuid().ToString();
            status.Name = "New";

            _context.Add(status);
            _context.SaveChanges();

            Status status1 = new Status();
            status1.Id = Guid.NewGuid().ToString();
            status1.Name = "Completed";

            _context.Add(status1);
            _context.SaveChanges();*/

            return View();
        }

        public IActionResult Create()
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name");
            ViewData["PriorityId"] = new SelectList(_context.Set<Priority>(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name, completed, created, updated, StatusId, PriorityId, UserId")] ToDoTask toDoTask)
        {
            if(User.FindFirst(ClaimTypes.NameIdentifier) == null) return View();
            ModelState.Remove("Id");
            ModelState.Remove("UserId");
            ModelState.Remove("created");
            ModelState.Remove("updated");
            ModelState.Remove("User");
            ModelState.Remove("Status");
            ModelState.Remove("Priority");

            if (ModelState.IsValid)
            {
                toDoTask.Id = Guid.NewGuid().ToString();
                toDoTask.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                toDoTask.created = DateTime.Now;
                toDoTask.updated = DateTime.Now;

                await _context.AddAsync(toDoTask);
                await _context.SaveChangesAsync();

            }

            return View();
        }

        public IActionResult Tasks()
        {
            var currentUser = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var tasks = _context.ToDoTask.Where(i=> i.UserId == currentUser).ToList();
            return View(tasks);
        }

        public async Task<IActionResult> Edit(string Id)
        {
            if(Id.IsNullOrEmpty()) return RedirectToAction("Tasks");

            var task = await _context.ToDoTask.FindAsync(Id);
            if (task == null) return RedirectToAction("Tasks");

            ViewData["StatusId"] = new SelectList(_context.Set<Status>(), "Id", "Name");
            ViewData["PriorityId"] = new SelectList(_context.Set<Priority>(), "Id", "Name");

            return View(task);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            var task = await _context.ToDoTask.FindAsync(Id);
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            if (Id.IsNullOrEmpty()) return RedirectToAction("Tasks");

            var task = await _context.ToDoTask.FindAsync(Id);
            if(task !=null)
            {
                _context.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Tasks");
        }

        public IActionResult Details(string Id)
        {
            if (Id.IsNullOrEmpty()) return RedirectToAction("Tasks");

            var task = _context.ToDoTask.Find(Id);
            var tsk = _context.ToDoTask
                .Include(x => x.User)
                .Include(i => i.Status)
                .Include(i => i.Priority)
                .FirstOrDefault(i => i.Id == Id);
            return View(tsk);
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
