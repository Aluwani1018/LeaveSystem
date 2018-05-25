using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LeaveSystem.Data.Model;
using LeaveSystem.Presentation.Models.AccountViewModels;
using LeaveSystem.Presentation.Models.ManageLeaveViewModels;

namespace LeaveSystem.Presentation
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, RegisterViewModel>()
                .ForMember(x => x.Password, v => v.Ignore())
                .ForMember(x => x.ConfirmPassword, v => v.Ignore())
                .ReverseMap()
                 .ForMember(x => x.AccessFailedCount, v => v.Ignore())
                 .ForMember(x => x.Claims, v => v.Ignore())
                 .ForMember(x => x.ConcurrencyStamp, v => v.Ignore())
                 .ForMember(x => x.CreatedDate, v => v.Ignore())
                 .ForMember(x => x.EmailConfirmed, v => v.Ignore())
                 .ForMember(x => x.Employees, v => v.Ignore())
                 .ForMember(x => x.Id, v => v.Ignore())
                 .ForMember(x => x.IsEnabled, v => v.Ignore())
                 .ForMember(x => x.IsLockedOut, v => v.Ignore())
                 .ForMember(x => x.LockoutEnabled, v => v.Ignore())
                 .ForMember(x => x.LockoutEnd, v => v.Ignore())
                 .ForMember(x => x.Manager, v => v.Ignore())
                 .ForMember(x => x.ManagerId, v => v.Ignore())
                 .ForMember(x => x.NormalizedEmail, v => v.Ignore())
                 .ForMember(x => x.NormalizedUserName, v => v.Ignore())
                 .ForMember(x => x.PhoneNumberConfirmed, v => v.Ignore())
                 .ForMember(x => x.Roles, v => v.Ignore())
                 .ForMember(x => x.SecurityStamp, v => v.Ignore())
                 .ForMember(x => x.TwoFactorEnabled, v => v.Ignore())
                 .ForMember(x => x.UpdatedDate, v => v.Ignore());

            CreateMap<Leave, LeaveViewModel>()
                .ForMember(x => x.employee, v => v.Ignore())
                .ReverseMap();
            CreateMap<LeaveStatus, LeaveStatusViewModel>();
            CreateMap<Employee, EmployeeViewModel>()
                .ReverseMap();
                //.ForMember(x => x.Employees, v => v.Ignore());
                
            CreateMap<Leave, CreateLeaveViewModel>()
                .ReverseMap()
                .ForMember(x => x.StatusId, v => v.Ignore())
                .ForMember(x => x.LeaveStatus, d => d.Ignore())
                .ForMember(x => x.Id, d => d.Ignore());
            CreateMap<Leave, UpdateLeaveViewModel>();
        }
    }
}
