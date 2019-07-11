using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.Types;

namespace TestFramework.Extensions
{
    public static class LocatorExtension
    {
        public static By ToBy(this ElementLocator locator)
        {
            By by;
            switch (locator.Kind)
            {
                case Locator.Id:
                    by = By.Id(locator.Value);
                    break;
                case Locator.ClassName:
                    by = By.ClassName(locator.Value);
                    break;
                case Locator.CssSelector:
                    by = By.CssSelector(locator.Value);
                    break;
                case Locator.LinkText:
                    by = By.LinkText(locator.Value);
                    break;
                case Locator.Name:
                    by = By.Name(locator.Value);
                    break;
                case Locator.PartialLinkText:
                    by = By.PartialLinkText(locator.Value);
                    break;
                case Locator.TagName:
                    by = By.TagName(locator.Value);
                    break;
                case Locator.XPath:
                    by = By.XPath(locator.Value);
                    break;
                default:
                    by = By.Id(locator.Value);
                    break;
            }

            return by;
        }
    }
}
