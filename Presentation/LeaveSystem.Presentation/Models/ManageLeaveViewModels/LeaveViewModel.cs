using LeaveSystem.Presentation.Models.AccountViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveSystem.Presentation.Models.ManageLeaveViewModels
{
    public class LeaveViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Reason")]
        public string Reason { get; set; }
        [Display(Name = "From date")]
        public DateTime FromDate { get; set; }
        [Display(Name = "To date")]
        public DateTime ToDate { get; set; }
        public int StatusId { get; set; }
        public string EmployeeId { get; set; }
        [Display(Name = "Number of days")]
        public int NumberOfDays { get; set; }
       
        public LeaveStatusViewModel LeaveStatus { get; set; }
        public EmployeeViewModel employee { get; set; }
    }
}
