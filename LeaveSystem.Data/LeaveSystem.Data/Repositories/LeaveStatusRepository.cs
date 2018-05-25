using LeaveSystem.Data.Interfaces;
using LeaveSystem.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Data.Repositories
{
    public class LeaveStatusRepository : Repository<LeaveStatus>, ILeaveStatusRepository
    {
        private ApplicationDbContext dbContext = null;
        public LeaveStatusRepository(IdentityDbContext<Employee, Role, string> dbContext) : base(dbContext)
        {
            this.dbContext = dbContext as ApplicationDbContext;
        }
    }
}
