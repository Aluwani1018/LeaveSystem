using LeaveSystem.Business.Interfaces;
using LeaveSystem.Data.Model;
using LeaveSystem.Data.Uow;
using LeaveSystem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace LeaveSystem.Business
{
    public class LeaveManager : ILeaveManager
    {
        private readonly IUnitOfWork uow;
        private IPublicHolidaysManager publicHolidaysManager;
        public LeaveManager(IUnitOfWork unitOfWork, IPublicHolidaysManager publicHolidaysManager)
        {
            uow = unitOfWork;
            this.publicHolidaysManager = publicHolidaysManager;
        }
        public IEnumerable<Leave> GetManagerEmployeesLeaves(string ManagerId)
        {
            List<Leave> employeeLeaves = new List<Leave>();
            //get managers employees id's
            var employees = uow.Employees.GetWhere(x => x.ManagerId == ManagerId)
                .Select(x => x.Id).ToList();
            employeeLeaves = uow.Leaves.GetAllIncluding(x => employees.Contains(x.EmployeeId), c => c.LeaveStatus).ToList();
            return employeeLeaves;
        }

        public IEnumerable<Leave> GetLeaveByEmployeeId(string EmployeeId)
        {
            return uow.Leaves
                .GetAllIncluding(x => x.EmployeeId == EmployeeId, c => c.LeaveStatus)
                .ToList();
        }
        public int AddLeave(Leave leave)
        {
            if (IsLeaveADupliucate(leave))
                return -1;//it would be nice to return the actual reason why

            //check for holidays within the range of days taken
            var publicHoliday = publicHolidaysManager.GetPublicHolidaysWithinRange(leave.FromDate, leave.ToDate);
            //calculate the numnber of days taken except for holidays
            var numberOfDays = ((leave.ToDate - leave.FromDate).Days - publicHoliday.Count());
            leave.NumberOfDays = numberOfDays;
            leave.StatusId = (int)LeaveStatusEnum.Pending;
            uow.Leaves.Add(leave);
            return uow.Save();
        }

        public int UpdateLeaveStatus(int leaveId, LeaveStatusEnum leaveStatusEnum)
        {
            var leaveToUpdate = GetLeaveById(leaveId);
            leaveToUpdate.StatusId = (int)leaveStatusEnum;
            uow.Leaves.Update(leaveToUpdate);
            return uow.Save();
        }
        public int UpdateLeave(Leave leave)
        {
            //check for holidays within the range of days taken
            var publicHoliday = publicHolidaysManager.GetPublicHolidaysWithinRange(leave.FromDate, leave.ToDate);
            //calculate the numnber of days taken except for holidays
            var numberOfDays = ((leave.ToDate - leave.FromDate).Days - publicHoliday.Count());
            leave.NumberOfDays = numberOfDays;
            uow.Leaves.Update(leave);
            return uow.Save();
        }
        public Leave GetLeaveById(int leaveId)
        {
            return uow.Leaves.GetById(leaveId);

        }
        private bool IsLeaveADupliucate(Leave leave)
        {
            return uow.Leaves.GetWhere(x => x.FromDate == leave.FromDate.Date && x.ToDate == leave.ToDate.Date).Any();
        }

    }
}
