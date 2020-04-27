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
    public class SearchResultProcessor
    {
        #region Private Members
        private const string _XPathResultList = "//div[contains(@class, 's-result-list')]";
        private const string _XPathResultRows = "//div[contains(@class, 's-result-list')]/div[@data-uuid]";
        private const string _XPathNextResultPage = "//ul[@class='a-pagination']/li[last()]/a";
        private const string _XPathProductTitle = "descendant::h2/a/span[contains(@class, 'a-text-normal')]";
        private const string _XPathProductPrice = "descendant::span[@class='a-price']/span[@class='a-offscreen']";
        private const string _XPathProductRating = "descendant::a[starts-with(@class, 'a-popover-trigger')]/i/span";
        

        private PostFilter _PostFilter;
        private IWebDriverWrapper _Driver;
        private IErrorLogger _ErrorLog;
        #endregion

        #region Public Properties
        public bool IsCompleted { get; private set; }
        public bool IsLastPageReached { get; private set; }
        public Product Result { get; set; }
        #endregion

        #region Class Lifecycle
        public SearchResultProcessor(IWebDriverWrapper driver, IErrorLogger errorlog, PostFilter postFilterCriteria)
        {
            _Driver = driver;
            _ErrorLog = errorlog;
            _PostFilter = postFilterCriteria;
          
        }
        #endregion

        #region Public Properties
        public bool PreProcess()
        {
            return SortComputers(_PostFilter.Sort);
        }

        public bool Process()
        {
            //string xpath = "//div[contains(@class, 's-result-list')]";
            IWebElement resultElem = _Driver.WaitUntil(_Driver.DefaultTimeout, ExpectedConditions.ElementExists(By.XPath(_XPathResultList)));
            if (resultElem == null)
            {
                _ErrorLog.AddError("Products result list not found");
                return false;
            }

            Thread.Sleep(1500);

            HtmlNodeCollection resultItems = GetAllResultItems();
            if (resultItems == null || resultItems.Count == 0)
            {
                _ErrorLog.AddError("Products result list is empty");
                return false;
            }

            HtmlNode targetNode = FindTargetNode(resultItems, _PostFilter.Rating);
            if (targetNode != null)
            {
                Result = HtmlNodeToProduct(targetNode);
                IsCompleted = true;
            }

            return true;
        }

        public bool NextPage()
        {
            //string xpath = "//ul[@class='a-pagination']/li[last()]/a";

            if (CanMoveNext(_XPathNextResultPage))
            {
                IWebElement nextPageElem = _Driver.WaitUntil(_Driver.DefaultTimeout, ExpectedConditions.ElementToBeClickable(By.XPath(_XPathNextResultPage)));
                if (nextPageElem == null)
                {
                    _ErrorLog.AddError("Sort result element (dropdown) not found");
                    return false;
                }

                nextPageElem.ClickSafe();
            }
            else
            {
                IsLastPageReached = true;
            }

            return true;
        }

        #endregion

        #region Private Members
        private bool CanMoveNext(string xpath)
        {
            string html = _Driver.PageSource;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode nextPageNode = doc.DocumentNode.SelectSingleNode(xpath);

            return nextPageNode != null;
        }
        private Product HtmlNodeToProduct(HtmlNode node)
        {
            Product prd = new Product();

            HtmlNode productTitleNode = node.SelectSingleNode(_XPathProductTitle);
            HtmlNode priceNode = node.SelectSingleNode(_XPathProductPrice);
            string htmlPrice = priceNode.GetHtmlText();

            prd.Name = productTitleNode.GetHtmlText();
            prd.Asin = node.GetHtmlNodeAttributeValue("data-asin");

            decimal price;
            if (decimal.TryParse(htmlPrice.Substring(1), out price))
                prd.Price = price;

            return prd;
        }
        private HtmlNode FindTargetNode(HtmlNodeCollection resultItems, string rating)
        {
            foreach (HtmlNode node in resultItems)
            {
                HtmlNode ratingNode = node.SelectSingleNode(_XPathProductRating);
                HtmlNode priceNode = node.SelectSingleNode(_XPathProductPrice);

                if (ratingNode != null && priceNode != null)
                {
                    string htmlRating = ratingNode.GetHtmlText();
                    string htmlPrice = priceNode.GetHtmlText();
                    if (htmlRating.StartsWith(rating) && decimal.TryParse(htmlPrice.Substring(1), out _))
                    {
                        return node;
                    }
                }
            }

            return null;
        }
        private HtmlNodeCollection GetAllResultItems()
        {
            string html = _Driver.PageSource;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            //string resultItemsXPath = "//div[contains(@class, 's-result-list')]/div[@data-uuid]";
            HtmlNodeCollection resultItems = doc.DocumentNode.SelectNodes(_XPathResultRows);
            return resultItems;
        }
        private bool SortComputers(SortOrder sort)
        {
            if (sort == SortOrder.None)
                return true;

            IWebElement sortElem = _Driver.WaitUntil(_Driver.DefaultTimeout, ExpectedConditions.ElementExists(By.Id("s-result-sort-select")));
            if (sortElem == null)
            {
                 _ErrorLog.AddError("Sort result element (dropdown) not found");
                return false;
            }

            if (sort == SortOrder.LowestPrice && !sortElem.SetText("price-asc-rank"))
            {
                _ErrorLog.AddError("Failed to sort the result by Lowest Price");
                return false;
            }

            if (sort == SortOrder.HighestPrice && !sortElem.SetText("price-desc-rank"))
            {
                _ErrorLog.AddError("Failed to sort the result by Highest Price");
                return false;
            }

            return true;
        }
        #endregion
    }
}
