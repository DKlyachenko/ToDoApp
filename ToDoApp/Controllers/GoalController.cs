using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Core.Models;
using ToDoApp.Data;

namespace ToDoApp.Controllers
{
    public class GoalController : Controller
    {
        private ApplicationContext _db;
        //private IRepository _repository

        public GoalController (ApplicationContext context)
        {
            _db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _db.ToDoGoals.ToListAsync());
        }
        
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoGoal goal)
        {
            try
            {
                _db.ToDoGoals.Add(goal);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            ToDoGoal goal = await _db.ToDoGoals.FirstOrDefaultAsync(p => p.Id == id);
            if (goal != null)
                return View(goal);
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ToDoGoal goal)
        {
            try
            {
                _db.ToDoGoals.Update(goal);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                ToDoGoal goal = await _db.ToDoGoals.FirstOrDefaultAsync(p => p.Id == id);
                if (goal != null)
                {
                    _db.ToDoGoals.Remove(goal);
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
        public IActionResult ToggleDone(int id)
        {
            try
            {
                ToDoGoal goal = _db.ToDoGoals.FirstOrDefault(p => p.Id == id);
                if (goal != null)
                {
                    goal.IsDone = !goal.IsDone;
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
    }
}