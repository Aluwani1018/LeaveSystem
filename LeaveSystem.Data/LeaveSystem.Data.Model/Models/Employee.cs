using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveSystem.Data.Model
{
    public class Employee : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EmployeeNumber { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string ManagerId { get; set; }
        public virtual Employee Manager { get; set; }
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }


    }
}
