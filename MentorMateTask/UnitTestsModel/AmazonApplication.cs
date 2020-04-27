using HtmlAgilityPack;
using MentorMateTask.Selenium;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace MentorMateTask.UnitTestsModel
{
    public abstract class AmazonApplication : IErrorLogger, IExpectedResult<Product>
    {
        #region Private Members
        private IList<string> _Errors = new List<string>();
        private const string _XPathMenuItem = "//div[text()='{0}']/parent::a[@class='hmenu-item'] | //a[@class='hmenu-item' and text()='{0}']";
        #endregion

        #region Protected Members
        protected IWebDriverWrapper _Driver;
        protected const int _Timeout = 10000; //In milliseconds
        #endregion

        #region Public Properties
        public Product ExpectedResult { get; set; }
        #endregion

        #region Class Lifecycle
        public AmazonApplication(IWebDriverWrapper driver)
        {
            _Driver = driver;
            _Driver.DefaultTimeout = _Timeout;
            _Driver.ImplicitlyWait(_Timeout);
        }
        #endregion

        #region Public Methods
        public void AddError(string error)
        {
            _Errors.Add(error);
        }

        public string GetLastError()
        {
            return string.Join(";", _Errors);
        }

        public bool MenuNavigation(string navigationPath)
        {
            if (string.IsNullOrEmpty(navigationPath))
                return false;
            
            IWebElement hMenuElem = _Driver.WaitUntil(_Timeout, ExpectedConditions.ElementToBeClickable(By.Id("nav-hamburger-menu")));

            bool result = hMenuElem.ClickSafe();
            if (!result)
            {
                AddError("Failed to open hamburger menu");
                return false;
            }

            string[] menuItems = navigationPath.Split('>').Select(p => p.Trim()).ToArray();

            foreach (string item in menuItems)
            {
                string xpath = string.Format(_XPathMenuItem, item);

                IWebElement menuElem = _Driver.WaitUntil(_Timeout, ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));

                result = menuElem.ClickSafe();
                if (!result)
                {
                    AddError($"Failed to navigate to hamburger menu item '{item}'");
                    return false;
                }
            }

            return true;
        }

        public bool FilterBy(FilterBy b)
        {
            if (b == null)
            {
                throw new ArgumentNullException("FilterBy", "Cannot filter when FilterBy is null.");
            }

            string xpath = string.Format(b.Xpath, b.Criteria);

            IWebElement hMenuElem = _Driver.WaitUntil(_Timeout, ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));

            bool result = hMenuElem.ClickSafe();

            if (!result)
            {
                AddError($"Failed to filter products by '{b.Name}': {b.Criteria}");
                return false;
            }

            return true;
        }
        #endregion
    }

}
