using System;
using System.Collections.Generic;
using System.Text;
using LeaveSystem.Data.Interfaces;
using LeaveSystem.Data.Model;
using LeaveSystem.Data.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LeaveSystem.Data.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationDbContext dbContext = null;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext as ApplicationDbContext;

        }
        public ILeaveRepository Leaves
        {
            get
            {
                return new LeaveRepository(dbContext);
            }
        }

        public ILeaveStatusRepository LeaveStatus
        {
            get
            {
                return new LeaveStatusRepository(dbContext);
            }
        }

        public IPublicHolidayRepository PublicHolidays
        {
            get
            {
                return new PublicHolidayRepository(dbContext);
            }

        }
        public IEmployeeRepository Employees
        {
            get
            {
                return new EmployeeRepository(dbContext);
            }
        }
        public IRoleRepository Roles
        {
            get
            {
                return new RolesRepository(dbContext);
            }
        }
        public IEmployeeRolesRepository EmployeeRoles
        {
            get
            {
                return new EmployeeRolesRepository(dbContext);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(dbContext != null)
                {
                    dbContext.Dispose();
                }
            }
        }
        public int Save()
        {
            return dbContext.SaveChanges();
        }
    }
}
