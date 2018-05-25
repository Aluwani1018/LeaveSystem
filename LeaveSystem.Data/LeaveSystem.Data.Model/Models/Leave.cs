using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Data.Model
{
    public class Leave
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int NumberOfDays { get; set; }
        public int StatusId { get; set; }
        public string EmployeeId { get; set; }
        public virtual LeaveStatus LeaveStatus { get; set; }
    }
}
