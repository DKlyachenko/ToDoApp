using System;
using System.Collections.Generic;
using System.Text;
using ToDoApp.Core.Models;

namespace ToDoApp.DAL
{
    interface IRepository
    {
        IList<ToDoTask> GetAllTasks();
        IList<ToDoTask> GetActualTasks();
        IList<ToDoGoal> GetAllGoals();
        IList<ToDoGoal> GetActualGoals();

    }
}
