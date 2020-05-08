using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Core.Models;

namespace ToDoApp.DAL
{
    public interface IRepository
    {
        Task<IList<ToDoTask>> GetAllTasks();
        Task<IList<ToDoTask>> GetActualTasks();
        Task<IList<ToDoTask>> GetNonArchivedTasks();
        Task AddTask(ToDoTask task);
        Task<ToDoTask> GetTask(int id);
        Task UpdateTask(ToDoTask task);
        Task DeleteTask(ToDoTask task);
        void DeleteDoneTasks();
        Task<IList<ToDoGoal>> GetAllGoals();
        Task<IList<ToDoGoal>> GetActualGoals();
        Task AddGoal(ToDoGoal goal);
        Task<ToDoGoal> GetGoal(int id);
        Task UpdateGoal(ToDoGoal goal);
        Task DeleteGoal(ToDoGoal goal);

    }
}
