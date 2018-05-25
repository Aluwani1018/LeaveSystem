using LeaveSystem.Data.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LeaveSystem.Business;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LeaveSystem.Business.Tests.Managers.EmployeeManager
{
    [TestFixture]
    public class EmployeeManagerTests : EmployeeManagerTestBase
    {
        private string id
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        private Employee GetMockEmployee()
        {
            return new Employee
            {
                Id = id,
                UserName = "admin",
                FirstName = "Inbuilt",
                LastName = "Administrator",
                Email = "admin@company1.com",
                PhoneNumber = "+1 (123) 000-0000",
                EmailConfirmed = true,
                IsEnabled = true,
                CreatedDate = DateTime.Now.Date
            };
        }

        private List<Employee> GetListOfMockedEmployees()
        {
            return new List<Employee>()
            {
                     new Employee
                     {
                       UserName = "admin",
                       FirstName = "Inbuilt",
                       LastName = "Administrator",
                       Email = "admin@company1.com",
                       PhoneNumber = "+1 (123) 000-0000",
                       EmailConfirmed = true,
                       IsEnabled = true,
                       CreatedDate = DateTime.Now.Date
                    },
                  new Employee
                  {
                   UserName = "employee",
                   FirstName = "Inbuilt",
                   LastName = "Administrator",
                   Email = "employee@company1.com",
                   PhoneNumber = "+1 (123) 000-0000",
                   EmailConfirmed = true,
                   IsEnabled = true,
                   CreatedDate = DateTime.Now.Date
                  }
            };
        }

        //[Test()]
        //public void ShouldGetEmployeeByGivenEmail()
        //{

        //    List<Employee> employeesList = new List<Employee>()
        //    {
        //             new Employee
        //             {
        //               UserName = "admin",
        //               FirstName = "Inbuilt",
        //               LastName = "Administrator",
        //               Email = "admin@company1.com",
        //               PhoneNumber = "+1 (123) 000-0000",
        //               EmailConfirmed = true,
        //               IsEnabled = true,
        //               CreatedDate = DateTime.Now.Date
        //            },
        //          new Employee
        //          {
        //           UserName = "employee",
        //           FirstName = "Inbuilt",
        //           LastName = "Administrator",
        //           Email = "employee@company1.com",
        //           PhoneNumber = "+1 (123) 000-0000",
        //           EmailConfirmed = true,
        //           IsEnabled = true,
        //           CreatedDate = DateTime.Now.Date
        //          }
        //    };
        //    var expectedEmployee = new Employee
        //    {
        //        UserName = "admin",
        //        FirstName = "Inbuilt",
        //        LastName = "Administrator",
        //        Email = "admin@company1.com",
        //        PhoneNumber = "+1 (123) 000-0000",
        //        EmailConfirmed = true,
        //        IsEnabled = true,
        //        CreatedDate = DateTime.Now.Date
        //    };

        //    //_emailStoreMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>())).Returns(Task.FromResult(employee));
        //    uowMock.Setup(x => x.Employees.GetWhere(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employeesList.AsQueryable());

        //    var manager = new LeaveSystem.Business.EmployeeManager(_employeeManager, _roleManager, uowMock.Object);
        //    var results = manager.GetEmployeeByEmail("admin@company1.com");
        //    Assert.That(expectedEmployee.Email, Is.EqualTo(results.Email));
        //}
        //[Test]
        //public void ShouldReturnNullEmployeeWhenGivenEmailDoesNotExist()
        //{
        //    uowMock.Setup(x => x.Employees.GetWhere(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(new List<Employee>().AsQueryable());
        //    var manager = new LeaveSystem.Business.EmployeeManager(_employeeManager, _roleManager, uowMock.Object);
        //    var results = manager.GetEmployeeByEmail("admin@company1.com");
        //    Assert.IsNull(results);
        //}
        [Test]
        public async Task ShouldFindFindEmployeeByGivenIdAsync()
        {
            var employee = new Employee
            {
                Id = id,
                UserName = "admin",
                FirstName = "Inbuilt",
                LastName = "Administrator",
                Email = "admin@company1.com",
                PhoneNumber = "+1 (123) 000-0000",
                EmailConfirmed = true,
                IsEnabled = true,
                CreatedDate = DateTime.Now.Date
            };

            _userStoreMock.Setup(x => x.FindByIdAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>())).Returns(Task.FromResult<Employee>(employee));
            var manager = new LeaveSystem.Business.EmployeeManager(_employeeManager, _roleManager, uowMock.Object);
            var results = await manager.GetEmployeeByIdAsync(id);
            Assert.AreEqual(employee, results);

        }

        [Test]
        public async Task ShouldGetEmployeeByGivenUserNameAsync()
        {
            var expectedEmployee = GetMockEmployee();
            _userStoreMock.Setup(x => x.FindByNameAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
                .Returns(Task.FromResult(expectedEmployee));
            var manager = new LeaveSystem.Business.EmployeeManager(_employeeManager, _roleManager, uowMock.Object);
            var results = await manager.GetEmployeeByUserNameAsync("admin@company1.com");
            Assert.AreEqual(expectedEmployee, results);

        }
        [Test]
        public void ShouldGetRolesForGivenEmployee()
        {
            var employeeRole = new IdentityUserRole<string>()
            {
                RoleId = id,
                UserId = id
            };
            var role = new Role()
            {
                Id = id,
                Name = "administrator"
            };

            List<string> roles = new List<string> { "administrator" };

            uowMock.Setup(x => x.EmployeeRoles.GetWhere(It.IsAny<Expression<Func<IdentityUserRole<string>, bool>>>()))
                .Returns(new List<IdentityUserRole<string>>() { employeeRole }.AsQueryable());

            uowMock.Setup(x => x.Roles.GetWhere(It.IsAny<Expression<Func<Role, bool>>>()))
                .Returns(new List<Role>() { role }.AsQueryable());

            var manager = new LeaveSystem.Business.EmployeeManager(_employeeManager, _roleManager, uowMock.Object);
            var results = manager.GetEmployeeRoles(GetMockEmployee());
            Assert.AreEqual(roles, results);
        }

        [Test]
        public void ShouldGetEmployeeAndRolesForGivenEmployeeId()
        {
            var userRole = new IdentityUserRole<string>
            {
                RoleId = id,
                UserId = id
            };
            var role = new Role
            {
                Id = id,
                Name = "administrator"
            };
            var expectedRoles = new string[] { "administrator" };
            var employee = GetMockEmployee();
            employee.Roles = new List<IdentityUserRole<string>>() { userRole };
            uowMock.Setup(x => x.Employees.GetAllIncluding(It.IsAny<Expression<Func<Employee, bool>>>(), It.IsAny<Expression<Func<Employee, object>>>()))
                .Returns(new List<Employee> { employee }.AsQueryable());

            uowMock.Setup(x => x.Roles.GetWhere(It.IsAny<Expression<Func<Role, bool>>>())).Returns(new List<Role> { role }.AsQueryable());
            var manager = new LeaveSystem.Business.EmployeeManager(_employeeManager, _roleManager, uowMock.Object);
            var results = manager.GetEmployeeAndRolesAsync(id);
            Assert.AreEqual(expectedRoles, results.Item2);
            Assert.AreEqual(employee, results.Item1);

        }

        [Test]
        public void ShouldReturnNullWhenGivenEmployeeIdDoesntExist()
        {
            uowMock.Setup(x => x.Employees.GetAllIncluding(It.IsAny<Expression<Func<Employee, bool>>>(), It.IsAny<Expression<Func<Employee, object>>>()))
                .Returns(new List<Employee>().AsQueryable());
            var manager = new LeaveSystem.Business.EmployeeManager(_employeeManager, _roleManager, uowMock.Object);
            var results = manager.GetEmployeeAndRolesAsync(id);
            Assert.AreEqual(null, results);

        }
        [Test]
        public async Task ShouldCreateEmployeeAndAssignRolesAsync()
        {
            //IdentityResult result = IdentityResult.Success;
            //var identityResults = new IdentityResult();
            //identityResults.Succeeded = true;
            //IdentityResult.Success = true;
            _userStoreMock.Setup(x => x.CreateAsync(It.IsAny<Employee>(), It.IsAny<System.Threading.CancellationToken>()))
                .Returns(Task.FromResult(IdentityResult.Success));

            _userStoreMock.Setup(x => x.FindByNameAsync(It.IsAny<string>(), It.IsAny<System.Threading.CancellationToken>()))
                 .Returns(Task.FromResult(GetMockEmployee()));
            employeeManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<Employee>(), It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            var manager = new LeaveSystem.Business.EmployeeManager(_employeeManager, _roleManager, uowMock.Object);
           // var results =await manager.CreateUserAsync(GetMockEmployee(), new string[] { "administrator" }, "P@ssword.1");

        }
        [Test]
        public async Task ShouldResertEmployeesPasswordAsync()
        {
           employeeManagerMock.Setup(x =>  x.ResetPasswordAsync(It.IsAny<Employee>(), It.IsAny<string>(),It.IsAny<string>()))
                .Returns(Task.FromResult(IdentityResult.Success));
            employeeManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(It.IsAny<Employee>()))
            .Returns(Task.FromResult(Guid.NewGuid().ToString()));
            var manager = new LeaveSystem.Business.EmployeeManager(_employeeManager, _roleManager, uowMock.Object);
           // var results =await manager.ResetPasswordAsync(GetMockEmployee(), "Password.1");
            //,It.IsAny<IEnumerable<string>>()))
        }
    }
}
