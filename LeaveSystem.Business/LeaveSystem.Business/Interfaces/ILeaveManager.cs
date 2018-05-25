using LeaveSystem.Data.Model;
using LeaveSystem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Business.Interfaces
{
    public interface ILeaveManager
    {
        IEnumerable<Leave> GetManagerEmployeesLeaves(string ManagerId);
        IEnumerable<Leave> GetLeaveByEmployeeId(string EmployeeId);
        int AddLeave(Leave leave);
        int UpdateLeaveStatus(int leaveId, LeaveStatusEnum leaveStatusEnum);
        Leave GetLeaveById(int leaveId);
        int UpdateLeave(Leave leave);
    }
}
