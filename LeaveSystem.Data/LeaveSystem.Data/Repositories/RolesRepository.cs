using LeaveSystem.Data.Interfaces;
using LeaveSystem.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Data.Repositories
{
    public class RolesRepository: Repository<Role>, IRoleRepository
    {
        private ApplicationDbContext dbContext = null;
        public RolesRepository(IdentityDbContext<Employee, Role,string> dbContext) : base(dbContext)
        {
            this.dbContext = dbContext as ApplicationDbContext;
        }
    }
}
