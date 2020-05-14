using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Core.Models;
using ToDoApp.DAL;
using ToDoApp.DAL.Data;

namespace ToDoApp.Controllers
{
    public class TaskController : Controller
    {
        private IRepository _repository;

        public TaskController(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Goals"] = await _repository.GetActualGoals();
            return View(await _repository.GetNonArchivedTasks());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoTask task)
        {
            try
            {
                await _repository.AddTask(task);
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
                await _repository.UpdateTask(task);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _repository.GetTask(id);
            if (task != null)
                return View(task);
            return NotFound();
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var task = await _repository.GetTask(id);
            if (task != null)
                return View(task);
            return NotFound();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ret = new ReturnResult();
            try
            {
                var task = await _repository.GetTask(id);
                if (task != null)
                {
                    await _repository.DeleteTask(task);
                    ret.Success = true;
                } else
                {
                    ret.Error = StaticData.taskFindError;
                }
                
            }
            catch (Exception e)
            {
                ret.Error = e.Message;
            }
            return Json(ret);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleDone(int id)
        {
            var ret = new ReturnResult();
            try
            {
                var task = await _repository.GetTask(id);
                if (task != null)
                {
                    task.IsDone = !task.IsDone;
                    await _repository.UpdateTask(task);
                    ret.Success = true;
                } else {
                    ret.Error = StaticData.taskFindError;
                }
            }
            catch (Exception e)
            {
                ret.Error = e.Message;
            }
            return Json(ret);
        }

        public IActionResult DeleteDoneTasks()
        {
            _repository.DeleteDoneTasks();
            return RedirectToAction(nameof(Index));
        }
    }
}