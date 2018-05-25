using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveSystem.Business;
using LeaveSystem.Business.Interfaces;
using LeaveSystem.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace LeaveSystem.Presentation.Controllers.Base
{
    public class BaseController : Controller
    {
        private readonly ILeaveManager leaveManager;
        private readonly IEmployeeManager _employeeManager;
        public BaseController(ILeaveManager leaveManager, IEmployeeManager employeeManager)
        {
            this.leaveManager = leaveManager;
            _employeeManager = employeeManager;
        }
        public  Employee currentEmployee
        {
            get
            {
                return _employeeManager.GetEmployeeByUserNameAsync(User.Identity.Name).Result;
            }
        }
 
    }
}