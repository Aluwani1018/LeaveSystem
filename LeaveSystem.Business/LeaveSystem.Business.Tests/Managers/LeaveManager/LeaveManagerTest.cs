using LeaveSystem.Data.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LeaveSystem.Business.Tests.Managers
{
    [TestFixture]
    public class LeaveManagerTest : LeaveManagerTestBase
    {
        [Test]
        public void ShouldGetEmployeesLeaveWithGivenManagerId()
        {
            uowMock.Setup(x => x.Employees.GetWhere(It.IsAny<Expression<Func<Employee, bool>>>()))
                .Returns(DataUtilities.GetListOfMockedEmployees().AsQueryable());
            uowMock.Setup(x => x.Leaves.GetAllIncluding(It.IsAny<Expression<Func<Leave, bool>>>(), It.IsAny<Expression<Func<Leave, object>>>()))
                .Returns(DataUtilities.GetListOfMockedLeaves().AsQueryable());
            var manager = new LeaveManager(uowMock.Object, publicHolidaysManager.Object);
            var results = manager.GetManagerEmployeesLeaves(DataUtilities.id);

            var expectedResults = DataUtilities.GetListOfMockedLeaves();
            Assert.AreEqual(expectedResults.Count(), results.Count());
        }

        [Test]
        public void ShouldGetLeaveForGivenEmployeeIdTest()
        {
            uowMock.Setup(x => x.Leaves.GetAllIncluding(It.IsAny<Expression<Func<Leave, bool>>>(), It.IsAny<Expression<Func<Leave, object>>>()))
                .Returns(DataUtilities.GetListOfMockedLeaves().AsQueryable());
            var manager = new LeaveManager(uowMock.Object, publicHolidaysManager.Object);
            var results = manager.GetLeaveByEmployeeId(DataUtilities.id);
            var expectedResults = DataUtilities.GetListOfMockedLeaves();
            Assert.AreEqual(expectedResults.Count(), results.Count());
        }
        [Test]
        public void ShouldAddLeaveThatIsNotADuplicateWithNoPublicHolidaysInBetweenTest()
        {
            uowMock.Setup(x => x.Leaves.GetWhere(It.IsAny<Expression<Func<Leave, bool>>>()))
                .Returns(new List<Leave>().AsQueryable());
            uowMock.Setup(x => x.PublicHolidays.GetWhere(It.IsAny<Expression<Func<PublicHoliday, bool>>>()))
                .Returns(new List<PublicHoliday>().AsQueryable());
            uowMock.Setup(x => x.Leaves.Add(It.IsAny<Leave>()));
            uowMock.Setup(x => x.Save()).Returns(1);
            var manager = new LeaveManager(uowMock.Object, publicHolidaysManager.Object);
            var results = manager.AddLeave(DataUtilities.GetMockedLeaveToAdd());
            uowMock.Verify(x => x.Leaves.Add(It.IsAny<Leave>()), Times.Once());
            uowMock.Verify(x => x.Save(), Times.Once());


        }
        [Test]
        public void ShouldUpdateLeaveStatusTest()
        {
            uowMock.Setup(x => x.Leaves.GetById(It.IsAny<int>()))
                .Returns(DataUtilities.GetListOfMockedLeaves().FirstOrDefault());
            uowMock.Setup(x => x.Leaves.Update(It.IsAny<Leave>()));
            uowMock.Setup(x => x.Save()).Returns(1);
            var manager = new LeaveManager(uowMock.Object, publicHolidaysManager.Object);
            var results = manager.UpdateLeaveStatus(1,Infrastructure.LeaveStatusEnum.Approved);
            Assert.AreEqual(results, 1);

        }
        [Test]
        public void ShouldUpdateLeaveTest()
        {
            uowMock.Setup(x => x.Leaves.Update(It.IsAny<Leave>()));
            uowMock.Setup(x => x.PublicHolidays.GetWhere(It.IsAny<Expression<Func<PublicHoliday, bool>>>()))
                    .Returns(new List<PublicHoliday>().AsQueryable());
            uowMock.Setup(x => x.Save()).Returns(1);
            var manager = new LeaveManager(uowMock.Object, publicHolidaysManager.Object);
            var results = manager.UpdateLeave(DataUtilities.GetListOfMockedLeaves().First());
            Assert.AreEqual(results, 1);

        }
    }
}
