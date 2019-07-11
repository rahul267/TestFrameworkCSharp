using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Extensions
{
   public static class WebElementExtensions
    {
        /// <summary>
        /// Verify if actual element text equals to expected.
        /// </summary>
        /// <param name="webElement">The web element.</param>
        /// <param name="text">The text.</param>
        /// <returns>
        /// The <see cref="bool" />.
        /// </returns>
        public static bool IsElementTextEqualsToExpected(this IWebElement webElement, string text)
        {
            return webElement.Text.Equals(text);
        }

        /// <summary>
        /// Set element attribute using java script.
        /// </summary>
        /// <example>Sample code to check page title: <code>
        /// this.Driver.SetAttribute(this.username, "attr", "10");
        /// </code></example>
        /// <param name="webElement">The web element.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="attributeValue">The attribute value.</param>
        /// <exception cref="System.ArgumentException">Element must wrap a web driver
        /// or
        /// Element must wrap a web driver that supports java script execution</exception>
        public static void SetAttribute(this IWebElement webElement, string attribute, string attributeValue)
        {
            var javascript = webElement.ToDriver() as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }

            javascript.ExecuteScript(
                "arguments[0].setAttribute(arguments[1], arguments[2])",
                webElement,
                attribute,
                attributeValue);
        }

        /// <summary>
        /// Click on element using java script.
        /// </summary>
        /// <param name="webElement">The web element.</param>
        public static void JavaScriptClick(this IWebElement webElement)
        {
            var javascript = webElement.ToDriver() as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }

            javascript.ExecuteScript("arguments[0].click();", webElement);
        }

        /// <summary>
        /// Returns the textual content of the specified node, and all its descendants regardless element is visible or not.
        /// </summary>
        /// <param name="webElement">The web element</param>
        /// <returns>The attribute</returns>
        /// <exception cref="ArgumentException">Element must wrap a web driver
        /// or
        /// Element must wrap a web driver that supports java script execution</exception>
        public static string GetTextContent(this IWebElement webElement)
        {
            var javascript = webElement.ToDriver() as IJavaScriptExecutor;
            if (javascript == null)
            {
                throw new ArgumentException("Element must wrap a web driver that supports javascript execution");
            }

            var textContent = (string)javascript.ExecuteScript("return arguments[0].textContent", webElement);
            return textContent;
        }

    }
}
