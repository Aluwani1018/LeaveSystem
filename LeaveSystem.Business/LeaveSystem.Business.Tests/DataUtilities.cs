using LeaveSystem.Data.Model;
using LeaveSystem.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Business.Tests
{
    public static class DataUtilities
    {
        public static string id
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        /// <summary>
        /// Generates a mock Employee object with predefined details.
        /// </summary>
        /// <returns>An <see cref="Employee"/> object with specific preset properties such as ID, UserName, and Email.</returns>
        /// <example>
        /// var mockEmployee = GetMockEmployee();
        /// Console.WriteLine(mockEmployee.UserName); // Output: "admin"
        /// </example>
        public static Employee GetMockEmployee()
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

        /// <summary>
        /// Provides a mocked list of employee data for testing purposes.
        /// </summary>
        /// <returns>A <see cref="List{Employee}"/> containing pre-defined employee objects.</returns>
        /// <example>
        /// var mockedEmployees = GetListOfMockedEmployees();
        /// foreach (var employee in mockedEmployees)
        /// {
        ///     Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.Email}");
        /// }
        /// </example>
        public static List<Employee> GetListOfMockedEmployees()
        {
            return new List<Employee>()
            {
                     new Employee
                     {
                         Id=id,
                       UserName = "admin",
                       FirstName = "Inbuilt",
                       LastName = "Administrator",
                       Email = "admin@company1.com",
                       PhoneNumber = "+1 (123) 000-0000",
                       ManagerId=id,
                       EmailConfirmed = true,
                       IsEnabled = true,
                       CreatedDate = DateTime.Now.Date
                    },
                  new Employee
                  {
                      Id = id,
                      ManagerId=id,
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

        /// <summary>
        /// Retrieves a list of mocked leave records for testing purposes.
        /// </summary>
        /// <returns>An enumerable collection of mocked <c>Leave</c> objects.</returns>
        /// <example>
        /// var mockedLeaves = GetListOfMockedLeaves();
        /// foreach (var leave in mockedLeaves)
        /// {
        ///     Console.WriteLine($"Leave ID: {leave.Id}, Status: {leave.LeaveStatus.Name}");
        /// }
        /// </example>
        public static IEnumerable<Leave> GetListOfMockedLeaves()
        {
            return new List<Leave>()
            {
                new Leave()
                {
                    EmployeeId = id,
                    FromDate = DateTime.Now.Date,
                    ToDate = DateTime.Now.AddDays(5),
                    Id = 1,
                    NumberOfDays=4,
                    Reason="No reason",
                    StatusId =(int) LeaveStatusEnum.Pending,
                    LeaveStatus = new LeaveStatus
                    {
                        Id = (int)LeaveStatusEnum.Pending,
                        Name = LeaveStatusEnum.Pending.ToString()
                    },
                },
                  new Leave()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FromDate = DateTime.Now.Date,
                    ToDate = DateTime.Now.AddDays(3),
                    Id = 2,
                    NumberOfDays=3,
                    Reason="No reason",
                    StatusId =(int) LeaveStatusEnum.Pending,
                    LeaveStatus = new LeaveStatus
                    {
                        Id = (int)LeaveStatusEnum.Pending,
                        Name = LeaveStatusEnum.Pending.ToString()
                    }
                },
                    new Leave()
                {
                    EmployeeId = id,
                    FromDate = DateTime.Now.Date,
                    ToDate = DateTime.Now.AddDays(5),
                    Id = 3,
                    NumberOfDays=4,
                    LeaveStatus = new LeaveStatus
                    {
                        Id = (int)LeaveStatusEnum.Approved,
                        Name = LeaveStatusEnum.Approved.ToString()
                    },
                    Reason="No reason",
                    StatusId =(int) LeaveStatusEnum.Approved
                },
                      new Leave()
                {
                    EmployeeId = Guid.NewGuid().ToString(),
                    FromDate = DateTime.Now.Date,
                    ToDate = DateTime.Now.AddDays(5),
                    Id = 4,
                    NumberOfDays=4,
                    Reason="No reason",
                    StatusId =(int) LeaveStatusEnum.Pending,
                    LeaveStatus = new LeaveStatus
                    {
                        Id = (int)LeaveStatusEnum.Pending,
                        Name = LeaveStatusEnum.Pending.ToString()
                    },
                }
            };
        }

        /// <summary>
        /// Returns a list of mocked public holiday dates and names.
        /// </summary>
        /// <returns>A collection of <see cref="PublicHoliday"/> representing various public holidays in 2018.</returns>
        /// <example>
        /// foreach (var holiday in GetListOfMockedPublicHolidays())
        /// {
        ///     Console.WriteLine($"{holiday.Name}: {holiday.Date.ToShortDateString()}");
        /// }
        /// // Expected output:
        /// // New Year's Day: 01/01/2018
        /// // Human Right's Day: 21/03/2018
        /// // Good Friday: 30/03/2018
        /// // Family Day: 02/04/2018
        /// // Freedom Day: 27/04/2018
        /// // Labour Day: 01/05/2018
        /// // National Womans Day: 09/08/2018
        /// // Heritage Day: 24/09/2018
        /// // Day of Reconciliation: 16/12/2018
        /// // Christmas Day: 25/12/2018
        /// // Day of Good Will: 26/12/2018
        /// </example>
        public static IEnumerable<PublicHoliday> GetListOfMockedPublicHolidays()
        {
            return new List<PublicHoliday>()
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
        }

        /// <summary>
        /// Creates and returns a mocked Leave object with predefined attributes.
        /// </summary>
        /// <returns>A mocked Leave object with default values set for testing purposes.</returns>
        /// <example>
        /// var leave = GetMockedLeaveToAdd();
        /// Console.WriteLine(leave.EmployeeId); // Outputs the default employee ID
        /// Console.WriteLine(leave.FromDate); // Outputs: 12/02/2018 00:00:00
        /// Console.WriteLine(leave.ToDate); // Outputs: 15/02/2018 00:00:00
        /// </example>
        public static Leave GetMockedLeaveToAdd()
        {
            return new Leave()
            {
                EmployeeId = id,
                FromDate = new DateTime(2018, 2, 12),
                ToDate = new DateTime(2018, 2, 15),
                NumberOfDays = 0,
                Reason = "No reason",
                StatusId = (int)LeaveStatusEnum.Pending
                //LeaveStatus = new LeaveStatus
                //{
                //    Id = (int)LeaveStatusEnum.Pending,
                //    Name = LeaveStatusEnum.Pending.ToString()
                //},
            };
        }
    }
}
