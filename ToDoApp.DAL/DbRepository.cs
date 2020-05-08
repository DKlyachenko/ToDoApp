using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Core.Models;
using ToDoApp.DAL.Data;

namespace ToDoApp.DAL
{
    public class DbRepository : IRepository
    {
        private ApplicationContext _context;
        public DbRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddGoal(ToDoGoal goal)
        {
            _context.ToDoGoals.Add(goal);
            await _context.SaveChangesAsync();
        }

        public async Task AddTask(ToDoTask task)
        {
            _context.ToDoTasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public void DeleteDoneTasks()
        {
            var tasksToDelete = _context.ToDoTasks.Where(x => !x.IsArchived && x.IsDone);
            _context.RemoveRange(tasksToDelete);
            _context.SaveChanges();
        }

        public async Task DeleteGoal(ToDoGoal goal)
        {

            _context.ToDoGoals.Remove(goal);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTask(ToDoTask task)
        {
            _context.ToDoTasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<ToDoGoal>> GetActualGoals()
        {
            return await _context.ToDoGoals.Where(goal => !goal.IsArchived && !goal.IsDone).ToListAsync();
        }

        public async Task<IList<ToDoTask>> GetActualTasks()
        {
            return await _context.ToDoTasks.Where(task => !task.IsArchived && !task.IsDone).ToListAsync();
        }

        public async Task<IList<ToDoGoal>> GetAllGoals()
        {
            return await _context.ToDoGoals.ToListAsync();
        }

        public async Task<IList<ToDoTask>> GetAllTasks()
        {
            return await _context.ToDoTasks.ToListAsync();
        }

        public async Task<ToDoGoal> GetGoal(int id)
        {
            return await _context.ToDoGoals.FirstOrDefaultAsync(goal => goal.Id == id);
        }

        public async Task<IList<ToDoTask>> GetNonArchivedTasks()
        {
            return await _context.ToDoTasks.Where(task => !task.IsArchived).ToListAsync();
        }

        public async Task<ToDoTask> GetTask(int id)
        {
            return await _context.ToDoTasks.FirstOrDefaultAsync(task => task.Id == id);
        }

        public async Task UpdateGoal(ToDoGoal goal)
        {
            _context.ToDoGoals.Update(goal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTask(ToDoTask task)
        {
            _context.ToDoTasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
