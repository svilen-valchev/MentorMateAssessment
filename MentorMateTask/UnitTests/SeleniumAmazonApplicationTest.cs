using MentorMateTask.Selenium;
using MentorMateTask.UnitTestsModel;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMateTask.UnitTests
{
    [TestFixture]
    class SeleniumAmazonApplicationTest
    {
        #region Private Members
        private const string URL = "http://www.amazon.co.uk";
        private const string _MenuNavigation = "Electronics & Computers > Laptops";
        private IWebDriverWrapper _Driver;
        #endregion

        #region Initialize
        [SetUp]
        public void Init()
        {
            _Driver = new WebDriverWrapper(new ChromeDriver());
            _Driver.NavigateTo(URL);
        }
        #endregion

        #region Test Methods
        [Test]
        public void FindComputerWithLowestPriceAndVerify()
        {
            VerifyComputer computer = new VerifyComputer(_Driver);

            bool result = computer.MenuNavigation(_MenuNavigation);
            Assert.IsTrue(result, computer.GetLastError());

            result = computer.FilterBy(FilterBy.DisplaySize(FilterConstants.DisplaySize.Size_15_16_inch));
            Assert.IsTrue(result, computer.GetLastError());

            result = computer.FilterBy(FilterBy.CPUType(FilterConstants.CPUType.Intel_Core_i5));
            Assert.IsTrue(result, computer.GetLastError());

            result = computer.FilterBy(FilterBy.StorageType(FilterConstants.StorageType.SSD));
            Assert.IsTrue(result, computer.GetLastError());

            result = computer.FindComputers(new PostFilter() { Rating = "5", Sort = SortOrder.LowestPrice });
            Assert.IsTrue(result, computer.GetLastError());

            Product actualResult = computer.GetComputerDetails();

            Assert.IsNotNull(actualResult);
            Assert.AreEqual(computer.ExpectedResult.Name, actualResult.Name);
            Assert.AreEqual(computer.ExpectedResult.Price, actualResult.Price);
        }
        #endregion

        #region Clean Up
        [TearDown]
        public void CleanUp()
        {
            if (_Driver != null)
                _Driver.Dispose();
        }
        #endregion
    }
}
