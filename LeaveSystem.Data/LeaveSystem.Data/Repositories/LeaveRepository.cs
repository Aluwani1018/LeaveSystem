﻿using LeaveSystem.Data.Interfaces;
using LeaveSystem.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Data.Repositories
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private ApplicationDbContext dbContext = null;
        public LeaveRepository(IdentityDbContext<Employee, Role, string> dbContext) : base(dbContext)
        {
            this.dbContext = dbContext as ApplicationDbContext;
        }
    }
}
