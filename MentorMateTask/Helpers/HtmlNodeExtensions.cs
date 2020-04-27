using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MentorMateTask.Helpers
{
    internal static class HtmlNodeExtensions
    {
        /// <summary>
        /// Get an attribute value from an HtmlNode attribute
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static string GetHtmlNodeAttributeValue(this HtmlNode node, string attributeName)
        {
            if (node == null || node.Attributes[attributeName] == null)
                return String.Empty;

            return HttpUtility.HtmlDecode(node.Attributes[attributeName].Value).Trim();
        }

        /// <summary>
        /// Get inner text from HtmlNode
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetHtmlText(this HtmlNode node)
        {
            if (node == null)
                return String.Empty;

            return DecodeHtmlText(node.InnerText);
        }

        /// <summary>
        /// Decode html text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string DecodeHtmlText(string text)
        {
            return HttpUtility.HtmlDecode(text).Trim();
        }
    }
}
