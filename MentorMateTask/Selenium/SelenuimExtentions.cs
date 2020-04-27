using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace MentorMateTask.Selenium
{
    internal static class SelenuimExtentions
    {
        public static bool ClickSafe(this IWebElement element)
        {
            if (element == null)
                return false;

            try
            {
                element.Click();
                return true;
            }
            catch (StaleElementReferenceException) { return false; }
            catch (ElementNotVisibleException) { return false; }
        }

        public static bool SetText(this IWebElement element, string text)
        {
            try
            {
                string tagName = element.TagName.ToLower();
                if (tagName == "input")
                {
                    string type = element.GetAttribute("type").ToLower();
                    if (type == "text" || type == "password" || type == "numeric" || type == "number")
                    {
                        element.Clear();
                        element.SendKeys(text);
                        return true;
                    }
                    else if (type == "radio")
                    {
                        return element.ClickSafe();
                    }
                    else if (type == "checkbox")
                    {
                        if (text == "1" && !element.Selected)
                            return element.ClickSafe();

                        if (text == "0" && element.Selected)
                            return element.ClickSafe();

                        return false;
                    }
                    else
                    {
                        //Handle other input types
                        return false;
                    }
                }
                else if (tagName == "select")
                {
                    SelectElement dropDown = new SelectElement(element);
                    dropDown.SetOption(text);
                    return true;
                }
                else if (tagName == "textarea")
                {
                    element.Clear();
                    element.SendKeys(text);
                    return true;
                }
                else
                {
                    //Handle other html elements
                    return false;
                }
            }
            catch (UnhandledAlertException)
            {
                return false;
            }
        }

        private static bool SetOption(this SelectElement element, string text)
        {
            if (!SetOptionByValue(element, text))
            {
                if (!SetOptionByText(element, text))
                {
                    return SetOptionByIndex(element, text);
                }
            }

            return true;
        }

        private static bool SetOptionByText(SelectElement dropDown, string text)
        {
            try
            {
                dropDown.SelectByText(text);
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }

        private static bool SetOptionByValue(SelectElement dropDown, string value)
        {
            try
            {
                dropDown.SelectByValue(value);
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }

        private static bool SetOptionByIndex(SelectElement dropDown, string value)
        {
            try
            {
                int index;
                if (!int.TryParse(value, out index))
                    return false;

                dropDown.SelectByIndex(index);
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return true;
        }
    }
}
