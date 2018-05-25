using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using LeaveSystem.Business.Interfaces;
using LeaveSystem.Business;
using LeaveSystem.Data.Model;
using LeaveSystem.Presentation.Controllers;
using System.Security.Principal;

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using LeaveSystem.Presentation.Models.ManageLeaveViewModels;
using LeaveSystem.Infrastructure;

namespace LeaveSystem.Presentation.Test
{
    [TestFixture]
    public class LeaveControllerTest
    {
        private Mock<HttpContext> _contextMock;
        private Mock<ILeaveManager> leaveManagerMock;
        private Mock<IEmployeeManager> employeeManagerMock;
        private GenericIdentity fakeIdentity;
        private GenericPrincipal principal;

        [SetUp]
        public void SetUp()
        {
            _contextMock = new Mock<HttpContext>();
            leaveManagerMock = new Mock<ILeaveManager>();
            employeeManagerMock = new Mock<IEmployeeManager>();
            fakeIdentity = new GenericIdentity("User");
            principal = new GenericPrincipal(fakeIdentity, null);
            _contextMock.Setup(t => t.User).Returns(principal);
        }
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Mapper.Initialize(cfg => cfg.AddProfile<AutoMapperProfile>());

        }

        [Test()]
        public void ShouldReturnLeaveViewWithLeavesOfTheCurrentUser()
        {

            leaveManagerMock.Setup(x => x.GetLeaveByEmployeeId(It.IsAny<string>()))
                .Returns(value: new List<Leave>());
            employeeManagerMock.Setup(x => x.GetEmployeeByUserNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new Employee() { Id = Guid.NewGuid().ToString() }));
            var controller = new LeaveController(leaveManagerMock.Object, employeeManagerMock.Object);
            controller.ControllerContext.HttpContext = _contextMock.Object;
            var results = controller.Index();
            Assert.IsInstanceOf<IActionResult>(results);

        }

        [Test()]
        public void ShouldReturnAddLeaveView()
        {
            var controller = new LeaveController(leaveManagerMock.Object, employeeManagerMock.Object);
            controller.ControllerContext.HttpContext = _contextMock.Object;
            var results = controller.AddLeave();
            Assert.IsInstanceOf<IActionResult>(results);
        }

        [Test()]
        public void ShouldCreateLeaveForCurrentlyLoggedInEmployee()
        {
            var leave = new CreateLeaveViewModel
            {
                FromDate = DateTime.Now.Date,
                ToDate = DateTime.Now.AddDays(2).Date,
                Reason = "No Reason"
            };
            employeeManagerMock.Setup(x => x.GetEmployeeByUserNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new Employee() { Id = Guid.NewGuid().ToString() }));

            leaveManagerMock.Setup(x => x.AddLeave(It.IsAny<Leave>()))
                .Returns(1);
            var controller = new LeaveController(leaveManagerMock.Object, employeeManagerMock.Object);
            controller.ControllerContext.HttpContext = _contextMock.Object;
            var results = controller.AddLeave(leave);
            var redirectToActionResult = results as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [Test()]
        public void ShouldEditTheLeaveTest()
        {
            var leave = new UpdateLeaveViewModel
            {
                FromDate = DateTime.Now.Date,
                ToDate = DateTime.Now.AddDays(2).Date,
                Reason = "No Reason",
                StatusId = (int)LeaveStatusEnum.Pending
            };
            employeeManagerMock.Setup(x => x.GetEmployeeByUserNameAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new Employee() { Id = Guid.NewGuid().ToString() }));

            leaveManagerMock.Setup(x => x.UpdateLeave(It.IsAny<Leave>()))
                .Returns(1);

            var controller = new LeaveController(leaveManagerMock.Object, employeeManagerMock.Object);
            controller.ControllerContext.HttpContext = _contextMock.Object;
            var results = controller.EditLeave(leave);
            var redirectToActionResult = results as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [Test()]
        public void ShouldReturnEditLeavePageWithSpecificLeaveToUpdateTest()
        {
            var leave = new Leave
            {
                FromDate = DateTime.Now.Date,
                ToDate = DateTime.Now.AddDays(2).Date,
                Reason = "No Reason",
                StatusId = (int)LeaveStatusEnum.Pending
            };
            leaveManagerMock.Setup(x => x.GetLeaveById(It.IsAny<int>()))
                .Returns(leave);

            var controller = new LeaveController(leaveManagerMock.Object, employeeManagerMock.Object);
            controller.ControllerContext.HttpContext = _contextMock.Object;
            var results = controller.EditLeave(2);
            Assert.IsInstanceOf<IActionResult>(results);
        }

        [Test()]
        public void ShouldReturnLeaveDetailsViewWithSpecificLeaveTest()
        {
            var leave = new Leave
            {
                FromDate = DateTime.Now.Date,
                ToDate = DateTime.Now.AddDays(2).Date,
                Reason = "No Reason",
                StatusId = (int)LeaveStatusEnum.Pending
            };
            leaveManagerMock.Setup(x => x.GetLeaveById(It.IsAny<int>()))
                .Returns(leave);

            var controller = new LeaveController(leaveManagerMock.Object, employeeManagerMock.Object);
            controller.ControllerContext.HttpContext = _contextMock.Object;
            var results = controller.LeaveDetails(2);
            Assert.IsInstanceOf<IActionResult>(results);

        }
    }
}
