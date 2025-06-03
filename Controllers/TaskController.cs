using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Data;
using Microsoft.EntityFrameworkCore;

namespace ToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;
        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            // Set sort parameters for links
            ViewData["TitleSortParam"] = sortOrder == "title_asc" ? "title_desc" : "title_asc";
            ViewData["DueDateSortParam"] = sortOrder == "duedate_asc" ? "duedate_desc" : "duedate_asc";
            ViewData["IsDoneSortParam"] = sortOrder == "isdone_asc" ? "isdone_desc" : "isdone_asc";
            ViewData["CategorySortParam"] = sortOrder == "category_asc" ? "category_desc" : "category_asc";

            var tasks = from t in _context.Tasks
                        select t;

            switch (sortOrder)
            {
                case "title_desc":
                    tasks = tasks.OrderByDescending(t => t.Title);
                    break;
                case "title_asc":
                    tasks = tasks.OrderBy(t => t.Title);
                    break;
                case "duedate_desc":
                    tasks = tasks.OrderByDescending(t => t.DueDate);
                    break;
                case "duedate_asc":
                    tasks = tasks.OrderBy(t => t.DueDate);
                    break;
                case "isdone_desc":
                    tasks = tasks.OrderByDescending(t => t.IsDone);
                    break;
                case "isdone_asc":
                    tasks = tasks.OrderBy(t => t.IsDone);
                    break;
                case "category_desc":
                    tasks = tasks.OrderByDescending(t => t.Category);
                    break;
                case "category_asc":
                    tasks = tasks.OrderBy(t => t.Category);
                    break;
                default:
                    tasks = tasks.OrderBy(t => t.Title); // default sort
                    break;
            }

            return View(await tasks.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDone(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                task.IsDone = !task.IsDone;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
            }   
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    throw(new DbUpdateConcurrencyException("Task not found or has been modified by another user."));
                }
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }
    }
}
