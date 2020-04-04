using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Core.Models
{
    public class ToDoGoal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "Due date")]
        public DateTime? DueDate { get; set; }
        public string Notes { get; set; }
        [Display(Name = "Need to remind")]
        public bool NeedToRemind { get; set; }
        [Display(Name = "Done")]
        public bool IsDone { get; set; } = false;
        [Display(Name = "To archive")]
        public bool IsArchived { get; set; } = false;
    }
}
