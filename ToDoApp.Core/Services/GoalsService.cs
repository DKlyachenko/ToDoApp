using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Core.Models;

namespace ToDoApp.Core.Services
{
    public class GoalsService
    {
        IRepository _repository;

        public GoalsService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ToDoGoal>> GetAllGoals(){
            return await _repository.GetAllGoals();
        }

        public async Task AddGoal(ToDoGoal goal)
        {
            await _repository.AddGoal(goal);
        }

        public async Task<ToDoGoal> GetGoal(int id)
        {
            return await _repository.GetGoal(id);
        }

        public async Task UpdateGoal(ToDoGoal goal)
        {
            await _repository.UpdateGoal(goal);
        }

        public async Task<ReturnResult> Delete(int id)
        {
            var ret = new ReturnResult();
            try
            {
                ToDoGoal goal = await _repository.GetGoal(id);
                if (goal != null)
                {
                    await _repository.DeleteGoal(goal);
                    ret.Success = true;
                }
                else
                {
                    ret.Error = StaticData.goalFindError;
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
                var goal = await _repository.GetGoal(id);
                if (goal != null)
                {
                    goal.IsDone = !goal.IsDone;
                    await _repository.UpdateGoal(goal);
                    ret.Success = true;
                }
                else
                {
                    ret.Error = StaticData.goalFindError;
                }
            }
            catch (Exception e)
            {
                ret.Error = e.Message;
            }

            return ret;
        }
    }
}
