using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorMateTask.Selenium
{
    public interface IWebDriverWrapper : IDisposable
    {
        /// <summary>
        /// Default timeout in milliseconds
        /// </summary>
        int DefaultTimeout { get; set; }
        /// <summary>
        /// Html page source
        /// </summary>
        string PageSource { get; }
        bool ClickElementBy(By b);
        void NavigateTo(string url);
        IWebElement FindElementBy(By b);
        void ImplicitlyWait(int milliseconds);
        IWebElement WaitUntil(int millisecondsTimeout, Func<IWebDriver, IWebElement> condition);
    }
}
