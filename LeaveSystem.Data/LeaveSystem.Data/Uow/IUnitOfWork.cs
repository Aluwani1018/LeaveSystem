using LeaveSystem.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Data.Uow
{
    public interface IUnitOfWork
    {
        int Save();
        ILeaveRepository Leaves { get; }
        ILeaveStatusRepository LeaveStatus { get; }
        IPublicHolidayRepository PublicHolidays { get; }
        IEmployeeRepository Employees { get; }
        IRoleRepository Roles { get; }
        IEmployeeRolesRepository EmployeeRoles { get; }
    }
}
