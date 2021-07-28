using Microsoft.VisualStudio.TestTools.UnitTesting;
using Halwani.Core.ModelRepositories;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework.Internal;

namespace Halwani.Core.ModelRepositories.Tests
{
    [TestClass()]
    public class UserRepositoryTests
    {
        [TestMethod()]
        public void ReadUsersExcelTest()
        {
            new UserRepository().ReadUsersExcel();
            Assert.Fail();
        }
    }
}