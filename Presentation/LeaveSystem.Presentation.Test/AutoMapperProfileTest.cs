using AutoMapper;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveSystem.Presentation.Test
{
    [TestFixture]
    public class AutoMapperProfileTest
    {
        private static MapperConfiguration config;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
        }

        [Test()]
        public void WebApiMapperConfigurationIsValidTest()
        {
            config.AssertConfigurationIsValid();
        }
    }
}
