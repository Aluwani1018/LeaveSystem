using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Data.Interfaces
{
    public interface IEmployeeRolesRepository : IRepository<IdentityUserRole<string>>
    {
    }
}
