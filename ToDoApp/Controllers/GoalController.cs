using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Core.Models;
using ToDoApp.DAL;
using ToDoApp.Core;
using ToDoApp.Core.Services;

namespace ToDoApp.Controllers
{
    public class GoalController : Controller
    {
        private GoalsService _goalsService;

        public GoalController(GoalsService goalsService)
        {
            _goalsService = goalsService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _goalsService.GetAllGoals());
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
                await _goalsService.AddGoal(goal);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            ToDoGoal goal = await _goalsService.GetGoal(id);
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
                await _goalsService.UpdateGoal(goal);
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
            var ret = await _goalsService.Delete(id);
            return Json(ret);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleDone(int id)
        {
            var ret = await _goalsService.ToggleDone(id);
            return Json(ret);
        }
    }
}