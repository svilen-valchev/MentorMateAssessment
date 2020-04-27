using HtmlAgilityPack;
using MentorMateTask.Helpers;
using MentorMateTask.Selenium;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MentorMateTask.UnitTestsModel
{
    public class VerifyComputer : AmazonApplication
    {
        private const string _XPathProductTitle = "//span[@id='productTitle']";
        private const string _XPathProductPrice = "//span[@id='priceblock_ourprice']";
        private const string _XPathSelectProduct = "//div[@data-asin='{0}']/descendant::h2/a[contains(@class, 'a-link-normal')]";
        private const string _XPath = "";

        public VerifyComputer(IWebDriverWrapper driver) : base(driver)
        {
        }

        public bool FindComputers(PostFilter c)
        {
            SearchResultProcessor resultProcessor = new SearchResultProcessor(_Driver, (IErrorLogger)this,  c);

            bool result = resultProcessor.PreProcess();
            if (!result)
            {
                AddError("Failed to pre process the filtered result.");
                return false;
            }

            do
            {
                result = resultProcessor.Process();
                if (!result)
                {
                    AddError("Failed to process the filtered result.");
                    return false;
                }

                if (resultProcessor.IsCompleted)
                    break;

                result = resultProcessor.NextPage();
                if (!result)
                {
                    AddError("Failed to move to next result page.");
                    return false;
                }
            } while (!resultProcessor.IsLastPageReached);


            if (resultProcessor.Result == null)
            {
                AddError("Unable to find product that satisfy the filtering conditions.");
                return false;
            }

            ExpectedResult = resultProcessor.Result;

            result = SelectTargetProduct(resultProcessor.Result);
            if (!result)
            {
                AddError("Unable to select target product");
                return false;
            }

            return true;
        }

       

        public Product GetComputerDetails()
        {
            //string xpath = "//span[@id='productTitle']";
            IWebElement productTitleElem = _Driver.WaitUntil(_Timeout, ExpectedConditions.ElementExists(By.XPath(_XPathProductTitle)));
            if (productTitleElem == null)
            {
                AddError("Unable find the price of the selected product");
                return null;
            }

            //xpath = "//span[@id='priceblock_ourprice']";
            IWebElement productPriceElem = _Driver.WaitUntil(_Timeout, ExpectedConditions.ElementExists(By.XPath(_XPathProductPrice)));
            if (productTitleElem == null)
            {
                AddError("Unable find the price of the selected product");
                return null;
            }

            decimal price;
            decimal.TryParse(productPriceElem.Text.Substring(1), out price); //Skipp the currency sign

            return new Product
            {
                Name = productTitleElem.Text,
                Price = price 
            };
        }

        private bool SelectTargetProduct(Product targetProd)
        {
            string productAsin = targetProd.Asin; 
            string xpath = string.Format(_XPathSelectProduct, productAsin);
            IWebElement productElem = _Driver.WaitUntil(_Timeout, ExpectedConditions.ElementToBeClickable(By.XPath(xpath)));
            if (productElem == null)
            {
                AddError("Unable find target product");
                return false;
            }

            bool result = productElem.ClickSafe();
            if (!result)
            {
                AddError("Unable to select target product");
                return false;
            }

            Thread.Sleep(500);

            return true;
        }

       
    }
}
