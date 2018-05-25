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
