using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Core.Models;

namespace ToDoApp.Core.Services
{
    
    public class TasksService
    {
        IRepository _repository;

        public TasksService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ToDoTask>> GetNonArchivedTasks()
        {
            return await _repository.GetNonArchivedTasks();
        }

        public async Task AddTask(ToDoTask task)
        {
            await _repository.AddTask(task);
        }
        public async Task UpdateTask(ToDoTask task)
        {
            await _repository.UpdateTask(task);
        }

        public async Task<ToDoTask> GetTask(int id)
        {
            return await _repository.GetTask(id);
        }

        public async Task<ReturnResult> Delete(int id)
        {
            var ret = new ReturnResult();
            try
            {
                var task = await _repository.GetTask(id);
                if (task != null)
                {
                    await _repository.DeleteTask(task);
                    ret.Success = true;
                }
                else
                {
                    ret.Error = StaticData.taskFindError;
                }

            }
            catch (Exception e)
            {
                ret.Error = e.Message;
            }
            return ret;
        }

        public async Task<ReturnResult> ToggleDone(int id)
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
                }
                else
                {
                    ret.Error = StaticData.taskFindError;
                }
            }
            catch (Exception e)
            {
                ret.Error = e.Message;
            }
            return ret;
        }

        public void DeleteDoneTasks()
        {
            _repository.DeleteDoneTasks();
        }

        public async Task<IEnumerable<ToDoGoal>> GetActualGoals()
        {
            return await _repository.GetActualGoals();
        }
    }
}
