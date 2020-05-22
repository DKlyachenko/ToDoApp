using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoApp.Core.Models
{
    public class ReturnResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }

        public ReturnResult()
        {
            Success = false;
            Error = string.Empty;
        }
    }
}
