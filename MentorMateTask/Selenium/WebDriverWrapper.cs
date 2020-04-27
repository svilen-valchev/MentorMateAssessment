using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMateTask.Selenium
{
    public class WebDriverWrapper : IWebDriverWrapper
    {
        private IWebDriver _Driver;

        /// <summary>
        /// Default timeout in milliseconds
        /// </summary>
        public int DefaultTimeout { get; set; }

        public string PageSource { get { return _Driver.PageSource; } }

        public WebDriverWrapper(IWebDriver driver)
        {
            _Driver = driver;
        }

        #region Public Methods
        /// <summary>
        /// Navigate to URL
        /// </summary>
        /// <param name="url">Web resource to navigate</param>
        public void NavigateTo(string url)
        {
            _Driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Click web element by selector
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool ClickElementBy(By b)
        {
            IWebElement element = FindElementBy(b);
            if (element == null)
                return false;

            return element.ClickSafe();
        }

        public bool SetElementText(By b, string text)
        {
            IWebElement element = FindElementBy(b);
            if (element == null)
                return false;

            element.SendKeys(text);
            return true;
        }

        /// <summary>
        /// Find web element by selector
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public IWebElement FindElementBy(By b)
        {
            try
            {
                return _Driver.FindElement(b);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public ReadOnlyCollection<IWebElement> FindElementsBy(By b)
        {
            try
            {
                return _Driver.FindElements(b);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        /// <summary>
        /// Wait the specified time
        /// </summary>
        /// <param name="milliseconds"></param>
        public void ImplicitlyWait(int milliseconds)
        {
            _Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Wait until the condition is satisfied
        /// </summary>
        /// <param name="millisecondsTimeout"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IWebElement WaitUntil(int millisecondsTimeout, Func<IWebDriver, IWebElement> condition)
        {
            try
            {
                return (new WebDriverWait(_Driver, TimeSpan.FromMilliseconds(millisecondsTimeout))).Until(condition);
            }
            catch(WebDriverTimeoutException)
            {
                return null;
            }
        }

        /// <summary>
        /// Dispose web driver
        /// </summary>
        public void Dispose()
        {
            if (_Driver != null)
                _Driver.Quit();
        }

       
        #endregion
    }
}
