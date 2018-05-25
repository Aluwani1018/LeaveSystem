using LeaveSystem.Business;
using LeaveSystem.Business.Interfaces;
using LeaveSystem.Data;
using LeaveSystem.Data.Model;
using LeaveSystem.Data.Uow;
using LeaveSystem.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace LeaveSystem.Presentation
{
    public interface IDatabaseInitializer
    {
        Task Seed();
    }
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeManager _employeeManager;
        private readonly UserManager<Employee> _employeeManager2;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaveManager leaveManager;

        public DatabaseInitializer(ApplicationDbContext context
            , IEmployeeManager accountManager
            , RoleManager<Role> roleManager
            , IUnitOfWork unitOfWork
            , ILeaveManager leaveManager
            , UserManager<Employee> employeeManager2)
        {
            _employeeManager = accountManager;
            _context = context;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            this.leaveManager = leaveManager;
            _employeeManager2 = employeeManager2;

        }
        public async Task Seed()
        {

            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Users.AnyAsync())
            {
                const string adminRoleName = "administrator";
                const string employeeRoleName = "employee";
                const string managerRoleName = "manager";


                await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(employeeRoleName, "Default employee", new string[] { });
                await EnsureRoleAsync(managerRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());

                //System Admin

                await CreateEmployeeAsync("Password.1", 0000, "Inbuilt", "Administrator", "admin@company1.com", "+1 (123) 000-0000", new string[] { adminRoleName, managerRoleName, employeeRoleName });

                //CEO
                await CreateEmployeeAsync("Password.1", 0001, "Linda", "Jenkins", "lindan@acme.com", "+36 55 979 367", new string[] { managerRoleName });


                //Management Team
                await CreateEmployeeWithManagerAsync("Password.1", 0003, "Colin", "Horton", "colinhorton@amce.com", "+353 20 915 7545", "lindan@acme.com", new string[] { managerRoleName, employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 0002, "Milton", "Coleman", "miltoncoleman@amce.com", "+36 55 937 274", "lindan@acme.com", new string[] { employeeRoleName, managerRoleName });


                //Dev Team
                await CreateEmployeeWithManagerAsync("Password.1", 2005, "Ella", "Jefferson", "ellajefferson@acme.com", "+36 55 979 367", "colinhorton@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 2006, "Earl", "Craig", "earlcraig@acme.com", "+353 20 916 5608", "colinhorton@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 2008, "Marsha", "Murphy", "marshamurphy@acme.com", "+36 55 949 891", "colinhorton@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 2009, "Luis", "Ortega", "luisortega@acme.com", "+353 20 917 1339", "colinhorton@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 2010, "Faye", "Dennis", "fayedennis@acme.com", "", "colinhorton@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 2012, "Amy", "Burns", "amyburns@acme.com", "+353 20 914 1775", "colinhorton@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 2013, "Darrel", "Weber", "darrelweber@acme.com", "+36 55 615 463", "colinhorton@amce.com", new string[] { employeeRoleName });

                //Support Team
                await CreateEmployeeWithManagerAsync("Password.1", 1005, "Charlotte", "Osborne", "charlotteosborne@acme.com", "+36 55 760 177", "miltoncoleman@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 1006, "Marie", "Walters", "mariewalters@acme.com", "+353 20 918 6908", "miltoncoleman@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 1008, "Leonard", "Gill", "leonardgill@acme.com", "+36 55 525 585", "miltoncoleman@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 1009, "Enrique", "Thomas", "enriquethomas@acme.com", "+353 20 916 1335", "miltoncoleman@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 2010, "Omar", "Dunn", "omardunn@acme.com", "", "miltoncoleman@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 1012, "Dewey", "George", "deweygeorge@acme.com", "+36 55 260 127", "miltoncoleman@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 1013, "Rudy", "Lewis", "rudylewis@acme.com", "", "miltoncoleman@amce.com", new string[] { employeeRoleName });
                await CreateEmployeeWithManagerAsync("Password.1", 1015, "Neal", "French", "nealfrench@acme.com", "+353 20 919 4882", "miltoncoleman@amce.com", new string[] { employeeRoleName });

            }
            if (!await _context.LeaveStatus.AnyAsync())
            {
                var leaveStatus = new List<LeaveStatus>()
                {
                    new LeaveStatus()
                    {
                        Name = LeaveStatusEnum.Approved.ToString()
                    },
                    new LeaveStatus()
                    {
                        Name = LeaveStatusEnum.Pending.ToString()
                    },
                    new LeaveStatus()
                    {
                        Name = LeaveStatusEnum.Rejected.ToString()
                    },
                    new LeaveStatus()
                    {
                        Name = LeaveStatusEnum.Cancelled.ToString()
                    }
                };
                CreateLeaveStatus(leaveStatus);
            }
            if (!await _context.PublicHoliday.AnyAsync())
            {
                var publicHoliday = GetPublicHolidays();
                foreach (var item in publicHoliday)
                {
                    _unitOfWork.PublicHolidays.Add(item);
                    _unitOfWork.Save();
                }
            }
        }



        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if ((await _employeeManager.GetRoleByNameAsync(roleName)) == null)
            {
                Role role = new Role(roleName, description);

                var result = await this._employeeManager.CreateRoleAsync(role, claims);

                if (!result.Item1)
                    throw new Exception($"Seeding \"{description}\" role failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");
            }
        }

        private async Task<Employee> CreateEmployeeAsync(string password, int employeeNumber, string firstName, string lastName, string email, string phoneNumber, string[] roles)
        {
            Employee applicationUser = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                EmployeeNumber = employeeNumber,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,
                IsEnabled = true,
                CreatedDate = DateTime.Now.Date
            };

            var result = await _employeeManager.CreateEmployeeAsync(applicationUser, roles, password);

            if (!result.Item1)
                throw new Exception($"Seeding \"{email}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");


            return applicationUser;
        }
        private async Task CreateEmployeeWithManagerAsync(string password, int employeeNumber, string firstName, string lastName, string email, string phoneNumber, string managerEmail, string[] roles)
        {
            //get the manager id
            var manager = _employeeManager.GetEmployeeByEmail(managerEmail);
            if (manager != null)
            {
                Employee applicationUser = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    EmployeeNumber = employeeNumber,
                    ManagerId = manager.Id,
                    PhoneNumber = phoneNumber,
                    EmailConfirmed = true,
                    IsEnabled = true,
                    CreatedDate = DateTime.Now.Date
                };

                var result = await _employeeManager.CreateEmployeeAsync(applicationUser, roles, password);

                if (!result.Item1)
                    throw new Exception($"Seeding \"{email}\" user failed. Errors: {string.Join(Environment.NewLine, result.Item2)}");

            }

        }
        private void CreateLeaveStatus(List<LeaveStatus> leaveStatus)
        {
            foreach (var item in leaveStatus)
            {
                _unitOfWork.LeaveStatus.Add(item);
                _unitOfWork.Save();
            }
        }

        private List<PublicHoliday> GetPublicHolidays()
        {
            List<PublicHoliday> publicHolidays = new List<PublicHoliday>()
            {
                new PublicHoliday()
                {
                    Date = new DateTime(2018,1,1).Date,
                    Name = "New Year's Day"
                },
                 new PublicHoliday()
                {
                    Date = new DateTime(2018,3,21).Date,
                    Name = "Human Right's Day"
                },
                  new PublicHoliday()
                {
                    Date = new DateTime(2018,3,30).Date,
                    Name = "Good Friday"
                },
                   new PublicHoliday()
                {
                    Date = new DateTime(2018,4,2).Date,
                    Name = "Family Day"
                },
                    new PublicHoliday()
                {
                    Date = new DateTime(2018,4,27).Date,
                    Name = "Freedom Day"
                },
                        new PublicHoliday()
                {
                    Date = new DateTime(2018,5,1).Date,
                    Name = "Labour Day"
                },
                            new PublicHoliday()
                {
                    Date = new DateTime(2018,8,9).Date,
                    Name = "National Womans Day"
                },
                  new PublicHoliday()
                {
                    Date = new DateTime(2018,9,24).Date,
                    Name = "Heritage Day"
                },
                      new PublicHoliday()
                {
                    Date = new DateTime(2018,12,16).Date,
                    Name = "Day of Reconciliation"
                },
                    new PublicHoliday()
                {
                    Date = new DateTime(2018,12,25).Date,
                    Name = "Christmas Day"
                },
                        new PublicHoliday()
                {
                    Date = new DateTime(2018,12,26).Date,
                    Name = "Day of Good Will"
                },
            };
            return publicHolidays;


        }
    }
}
