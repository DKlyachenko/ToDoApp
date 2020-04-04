using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "Done")]
        public bool IsDone { get; set; } = false;
        [Display(Name = "To archive")]
        public bool IsArchived { get; set; } = false;
    }
}
