using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LeaveSystem.Data;
using LeaveSystem.Data.Model;
using LeaveSystem.Data.Uow;
using LeaveSystem.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveSystem.Business
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly UserManager<Employee> _employeeManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeManager(UserManager<Employee> employeeManager, RoleManager<Role> roleManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _employeeManager = employeeManager;
            _roleManager = roleManager;

        }

        public async Task<bool> HasPasswordAsync(Employee employee)
        {
            return await _employeeManager.HasPasswordAsync(employee);
        }

        public async Task<Employee> GetEmployeeByIdAsync(string userId)
        {
            return await _employeeManager.FindByIdAsync(userId);
        }

        public async Task<Employee> GetEmployeeByUserNameAsync(string userName)
        {
            return await _employeeManager.FindByNameAsync(userName);
        }

        public Employee GetEmployeeByEmail(string email)
        {
            return _employeeManager.FindByEmailAsync(email).Result;
        }

        public List<string> GetEmployeeRoles(Employee user)
        {
            var userRolesId = _unitOfWork.EmployeeRoles
                .GetWhere(x => x.UserId == user.Id)
                .Select(x => x.RoleId).ToList();
            List<string> userRoleNames = _unitOfWork.Roles.GetWhere(x => userRolesId.Contains(x.Id)).Select(x => x.Name).ToList();
            return userRoleNames;
        }

        public Tuple<Employee, string[]> GetEmployeeAndRolesAsync(string userId)
        {
            var user = _unitOfWork.Employees
                .GetAllIncluding(e => e.Id == userId, r => r.Roles)
                .FirstOrDefault();

            if (user == null)
                return null;

            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();

            var roles = _unitOfWork.Roles
                .GetWhere(r => userRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToArray();

            return Tuple.Create(user, roles);
        }

        public async Task<Tuple<bool, string[]>> CreateEmployeeAsync(Employee employee, IEnumerable<string> roles, string password)
        {
            //construct a username
            employee.UserName = employee.FirstName + employee.LastName + employee.EmployeeNumber;
            var result = await _employeeManager.CreateAsync(employee, password);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());


            employee = await _employeeManager.FindByNameAsync(employee.UserName);

            try
            {
                result = await _employeeManager.AddToRolesAsync(employee, roles);
            }
            catch (Exception ex)
            {
                await DeleteUserAsync(employee);
                throw;
            }

            if (!result.Succeeded)
            {
                await DeleteUserAsync(employee);
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
            }

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> ResetPasswordAsync(Employee user, string newPassword)
        {
            string resetToken = await _employeeManager.GeneratePasswordResetTokenAsync(user);

            var result = await _employeeManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> UpdatePasswordAsync(Employee user, string currentPassword, string newPassword)
        {
            var result = await _employeeManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());

            return Tuple.Create(true, new string[] { });
        }

        private async Task<Tuple<bool, string[]>> DeleteUserAsync(Employee user)
        {
            var result = await _employeeManager.DeleteAsync(user);
            return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }

        public async Task<Role> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<Tuple<bool, string[]>> CreateRoleAsync(Role role, IEnumerable<string> claims)
        {
            if (claims == null)
                claims = new string[] { };

            string[] invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
            if (invalidClaims.Any())
                return Tuple.Create(false, new[] { "The following claim types are invalid: " + string.Join(", ", invalidClaims) });


            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
                return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());


            role = await _roleManager.FindByNameAsync(role.Name);

            foreach (string claim in claims.Distinct())
            {
                result = await this._roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));

                if (!result.Succeeded)
                {
                    await DeleteRoleAsync(role);
                    return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                }
            }

            return Tuple.Create(true, new string[] { });
        }

        private async Task<Tuple<bool, string[]>> DeleteRoleAsync(Role role)
        {
            var result = await _roleManager.DeleteAsync(role);
            return Tuple.Create(result.Succeeded, result.Errors.Select(e => e.Description).ToArray());
        }






    }
}
