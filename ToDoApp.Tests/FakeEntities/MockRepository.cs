using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Core;
using ToDoApp.Core.Models;
using ToDoApp.DAL;

namespace ToDoApp.Tests.FakeEntities
{
    class MockRepository : IRepository
    {
        private List<ToDoGoal> goals;
        private List<ToDoTask> tasks;

        public MockRepository()
        {
            goals = new List<ToDoGoal>
            {
                new ToDoGoal{Id=1, Title="1", IsDone=false},
                new ToDoGoal{Id=2, Title="2", IsDone=false},
                new ToDoGoal{Id=3, Title="3", IsDone=true}
            };
            tasks = new List<ToDoTask>
            {
                new ToDoTask{Id=1, Title="1", IsDone=false},
                new ToDoTask{Id=2, Title="2", IsDone=false},
                new ToDoTask{Id=3, Title="3", IsDone=true},
                new ToDoTask{Id = 4, Title="4", IsDone=true, IsArchived=true}
            };
        }

        public async Task AddGoal(ToDoGoal goal)
        {
            await Task.Run(() => goals.Add(goal));
        }

        public async Task AddTask(ToDoTask task)
        {
            await Task.Run(() => tasks.Add(task));
        }

        public void DeleteDoneTasks()
        {
            tasks.RemoveAll(x => x.IsDone);
        }

        public async Task DeleteGoal(ToDoGoal goal)
        {
            await Task.Run(() => goals.Remove(goal));
        }

        public async Task DeleteTask(ToDoTask task)
        {
            await Task.Run(() => tasks.Remove(task));
        }

        public async Task<IList<ToDoGoal>> GetActualGoals()
        {
            return await Task.Run(() => goals.Where(x => !x.IsDone && !x.IsArchived).ToList());
        }

        public async Task<IList<ToDoTask>> GetActualTasks()
        {
            return await Task.Run(() => tasks.Where(x => !x.IsDone && !x.IsArchived).ToList());
        }

        public async Task<IList<ToDoGoal>> GetAllGoals()
        {
            return await Task.Run(() => goals);
        }

        public async Task<IList<ToDoTask>> GetAllTasks()
        {
            return await Task.Run(() => tasks);
        }

        public async Task<ToDoGoal> GetGoal(int id)
        {
            return await Task.Run(() => goals.FirstOrDefault(x => x.Id == id));
        }

        public async Task<IList<ToDoTask>> GetNonArchivedTasks()
        {
            return await Task.Run(() => tasks.Where(x => !x.IsArchived).ToList());
        }

        public async Task<ToDoTask> GetTask(int id)
        {
            return await Task.Run(() => tasks.FirstOrDefault(x => x.Id == id));
        }

        public async Task UpdateGoal(ToDoGoal goal)
        {
            var goalFound = goals.First(x => x.Id == goal.Id);
            goalFound.Title = goal.Title;
            goalFound.IsDone = goal.IsDone;
            goalFound.IsArchived = goal.IsArchived;
            goalFound.Notes = goal.Notes;
            await Task.Run(() => {});
        }

        public async Task UpdateTask(ToDoTask task)
        {
            var taskFound = tasks.First(x => x.Id == task.Id);
            taskFound.Title = task.Title;
            taskFound.IsDone = task.IsDone;
            taskFound.IsArchived = task.IsArchived;
            await Task.Run(() => { });
        }
    }
}
