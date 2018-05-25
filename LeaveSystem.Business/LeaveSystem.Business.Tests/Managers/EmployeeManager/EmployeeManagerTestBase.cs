using LeaveSystem.Data.Model;
using LeaveSystem.Data.Uow;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Business.Tests.Managers.EmployeeManager
{
    public class EmployeeManagerTestBase
    {
        public Mock<IUnitOfWork> uowMock;
        public Mock<UserManager<Employee>> employeeManagerMock;
        public UserManager<Employee> _employeeManager;
        public RoleManager<Role> _roleManager;
        public Mock<IUserStore<Employee>> _userStoreMock;
        public Mock<IUserEmailStore<Employee>> _emailStoreMock;
        public Mock<IUserRoleStore<Employee>> _userRoleStoreMock;
     

        [SetUp]
        public void SetUp()
        {
            
            uowMock = new Mock<IUnitOfWork>();

            _userStoreMock = new Mock<IUserStore<Employee>>(MockBehavior.Loose);
            
            _roleManager = GetMockRoleManager();
            _emailStoreMock = new Mock<IUserEmailStore<Employee>>();
            _userRoleStoreMock = new Mock<IUserRoleStore<Employee>>();
            employeeManagerMock = new Mock<UserManager<Employee>>();

            var optionsMock = new Mock<IOptions<IdentityOptions>>().Object;
            var passwordHasherMock = new Mock<IPasswordHasher<Employee>>().Object;
            var UserValidatorMock = new Mock<List<IUserValidator<Employee>>>().Object;
            _employeeManager = new UserManager<Employee>(_userStoreMock.Object
                , optionsMock
                , passwordHasherMock
                , UserValidatorMock
                , new Mock<List<IPasswordValidator<Employee>>>().Object
                , new Mock<ILookupNormalizer>().Object
                , new Mock<IdentityErrorDescriber>().Object
                , new Mock<IServiceProvider>().Object
                , new Mock<ILogger<UserManager<Employee>>>().Object);
            //,new Mock<IEnumerable<IUserValidator<Employee>>>().Object
            //,new Mock<IEnumerable<IPasswordValidator<Employee>>>().Object
            //,new Mock<ILookupNormalizer>().Object
            //,new Mock<IdentityErrorDescriber>().Object
            //,new Mock<IServiceProvider>().Object
            //,new Mock<ILogger<UserManager<Employee>>>().Object);
            //var services = new ServiceCollection();
            //services.AddEntityFramework()
            //    .AddInMemoryDatabase()
            //    .AddDbContext<MyDbContext>(options => options.UseInMemoryDatabase());
            employeeManagerMock = new Mock<UserManager<Employee>>();
        }

        private UserManager<Employee> GetMockUserManager()
        {
            
            return new UserManager<Employee>(
                _userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
        private RoleManager<Role> GetMockRoleManager()
        {
            var userStoreMock = new Mock<IRoleStore<Role>>();
            return new RoleManager<Role>(
                userStoreMock.Object, null, null, null,null);
        }
    }
}
