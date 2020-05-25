using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Core;
using ToDoApp.Core.Models;
using ToDoApp.Core.Services;
using ToDoApp.DAL;
using ToDoApp.DAL.Data;

namespace ToDoApp.Controllers
{
    public class TaskController : Controller
    {
        private TasksService _tasksService;

        public TaskController(TasksService tasksService)
        {
            _tasksService = tasksService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Goals"] = await _tasksService.GetActualGoals();
            return View(await _tasksService.GetNonArchivedTasks());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoTask task)
        {
            try
            {
                await _tasksService.AddTask(task);
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
                await _tasksService.UpdateTask(task);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _tasksService.GetTask(id);
            if (task != null)
                return View(task);
            return NotFound();
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var task = await _tasksService.GetTask(id);
            if (task != null)
                return View(task);
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ret = await _tasksService.Delete(id);
            return Json(ret);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleDone(int id)
        {
            var ret = await _tasksService.ToggleDone(id);
            return Json(ret);
        }

        public IActionResult DeleteDoneTasks()
        {
            _tasksService.DeleteDoneTasks();
            return RedirectToAction(nameof(Index));
        }
    }
}