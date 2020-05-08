using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Core.Models;
using ToDoApp.DAL.Data;
using ToDoApp.DAL;

namespace ToDoApp.Controllers
{
    public class GoalController : Controller
    {
        private IRepository _repository;

        public GoalController (IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllGoals());
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
                await _repository.AddGoal(goal);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            ToDoGoal goal = await _repository.GetGoal(id);
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
                await _repository.UpdateGoal(goal);
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
                ToDoGoal goal = await _repository.GetGoal(id);
                if (goal != null)
                {
                    await _repository.DeleteGoal(goal);
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
        public async Task<IActionResult> ToggleDone(int id)
        {
            try
            {
                var goal = await _repository.GetGoal(id);
                if (goal != null)
                {
                    goal.IsDone = !goal.IsDone;
                    await _repository.UpdateGoal(goal);
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