using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationContext _db;

        public TaskController(ApplicationContext context)
        {
            _db = context;
        }
        // GET: Task
        public async Task<IActionResult> Index()
        {
            ViewData["Goals"] = _db.ToDoGoals.Where(x=> !x.IsArchived && !x.IsDone).ToList();
            return View(await _db.ToDoTasks.Where(x => !x.IsArchived).ToListAsync());
        }

        
        // POST: Task/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoTask task)
        {
            try
            {
                _db.ToDoTasks.Add(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ToDoTask task)
        {
            try
            {
                _db.ToDoTasks.Update(task);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ToDoTask task = await _db.ToDoTasks.FirstOrDefaultAsync(p => p.Id == id);
            if (task != null)
                return View(task);
            return NotFound();
        }

        // GET: Task/Delete/5
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            ToDoTask task = await _db.ToDoTasks.FirstOrDefaultAsync(p => p.Id == id);
            if (task != null)
                return View(task);
            return NotFound();
        }

        // POST: Task/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                ToDoTask task = await _db.ToDoTasks.FirstOrDefaultAsync(p => p.Id == id);
                if (task != null)
                {
                    _db.ToDoTasks.Remove(task);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult ToggleDone(int id)
        {
            try
            {
                ToDoTask task = _db.ToDoTasks.FirstOrDefault(p => p.Id == id);
                if (task != null)
                {
                    task.IsDone = !task.IsDone;
                    //_db.ToDoTasks.Update(task);
                    _db.SaveChanges();
                    return null;
                }
                return NotFound();
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult DeleteDoneTasks()
        {
            var tasksToDelete = _db.ToDoTasks.Where(x => !x.IsArchived && x.IsDone);
            _db.RemoveRange(tasksToDelete);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}