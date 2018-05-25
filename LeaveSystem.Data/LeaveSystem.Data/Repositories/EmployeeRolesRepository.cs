using LeaveSystem.Data.Interfaces;
using LeaveSystem.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Data.Repositories
{
    public class EmployeeRolesRepository : Repository<IdentityUserRole<string>>, IEmployeeRolesRepository
    {
        private ApplicationDbContext dbContext = null;
        public EmployeeRolesRepository(IdentityDbContext<Employee, Role, string> dbContext) : base(dbContext)
        {
            this.dbContext = dbContext as ApplicationDbContext;
        }
    }
}
