using LeaveSystem.Business.Interfaces;
using LeaveSystem.Data.Uow;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Business.Tests.Managers
{
    public class LeaveManagerTestBase
    {
        public Mock<IUnitOfWork> uowMock;
        public Mock<IPublicHolidaysManager> publicHolidaysManager;

        [SetUp]
        public void SetUp()
        {
            uowMock = new Mock<IUnitOfWork>();
            publicHolidaysManager = new Mock<IPublicHolidaysManager>();
        }
    }
}
