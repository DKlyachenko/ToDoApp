using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoApp.Core.Models;

namespace ToDoApp.DAL
{
    public class DbRepository : IRepository
    {
        //private ApplicationContext _context;
        //public DbRepository(ApplicationContext context)
        //{
        //    _context = context;
        //}

        public IList<ToDoGoal> GetActualGoals()
        {
            throw new NotImplementedException();
        }

        public IList<ToDoTask> GetActualTasks()
        {
            throw new NotImplementedException();
        }

        public IList<ToDoGoal> GetAllGoals()
        {
            throw new NotImplementedException();
        }

        public IList<ToDoTask> GetAllTasks()
        {
            throw new NotImplementedException();
        }
    }
}
