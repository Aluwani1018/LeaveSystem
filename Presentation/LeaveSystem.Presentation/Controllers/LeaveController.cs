using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LeaveSystem.Business;
using LeaveSystem.Business.Interfaces;
using LeaveSystem.Data.Model;
using LeaveSystem.Infrastructure;
using LeaveSystem.Presentation.Controllers.Base;
using LeaveSystem.Presentation.Models;
using LeaveSystem.Presentation.Models.ManageLeaveViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LeaveSystem.Presentation.Controllers
{
    [Authorize]
    public class LeaveController : BaseController
    {
        private readonly ILeaveManager leaveManager;
        private readonly IEmployeeManager employeeManager;
        public LeaveController(ILeaveManager leaveManager, IEmployeeManager employeeManager)
            : base(leaveManager, employeeManager)
        {
            this.leaveManager = leaveManager;
            this.employeeManager = employeeManager;
        }
        [Authorize]
        public IActionResult Index()
        {

            try
            {
                var leaves = leaveManager.GetLeaveByEmployeeId(currentEmployee.Id).ToList();
                var results = Mapper.Map<List<Leave>, List<LeaveViewModel>>(leaves);
                return View(results);
            }
            catch (Exception ex)
            {
                ViewData["results"] = "Error:" + ex.Message;
                return View();
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddLeave()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddLeave(CreateLeaveViewModel leaveViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var leave = Mapper.Map<CreateLeaveViewModel, Leave>(leaveViewModel);
                    leave.EmployeeId = currentEmployee.Id;
                    var results = leaveManager.AddLeave(leave);
                    if (results > 0)
                    {
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("error", "Error when creating the leave");


                }
                return View(leaveViewModel);
            }
            catch (Exception ex)
            {
                ViewData["results"] ="Error:"+ ex.Message;
                return View(leaveViewModel);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult EditLeave(UpdateLeaveViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //can only update leave that is pending
                    if (model.StatusId != (int)LeaveStatusEnum.Pending)
                    {
                        ViewData["error"] = "You can only update Leave Request that is on Pending Status";
                        return View(model);
                    }
                    var leave = Mapper.Map<UpdateLeaveViewModel, Leave>(model);
                    var results = leaveManager.UpdateLeave(leave);
                    ViewData["results"] = "Leave updated successfully";
                    if (results > 0)
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("error", "Error when Updating the leave");

                }
                return View(model);
            }
            catch (Exception ex)
            {
                ViewData["results"] = "Error: "+ex.Message;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult EditLeave(int leaveId)
        {
            try
            {
                //get the leave
                var leave = leaveManager.GetLeaveById(leaveId);
                //map leave
                var leaveModel = Mapper.Map<Leave, UpdateLeaveViewModel>(leave);
                return View(leaveModel);
            }
            catch (Exception ex)
            {
                ViewData["results"] ="Error:"+ ex.Message;
                return View();
            }
        }

        [HttpGet]
        public IActionResult LeaveDetails(int leaveId)
        {
            try
            {
                var leave = leaveManager.GetLeaveById(leaveId);
                var leaveModel = Mapper.Map<Leave, LeaveViewModel>(leave);
                return View(leaveModel);
            }
            catch (Exception ex)
            {
                ViewData["results"] ="Error:"+ ex.Message;
                return View();
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void AddErrors(IEnumerable<string> result)
        {
            foreach (var error in result)
            {
                ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}