using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.Driver;
using TestFramework.Helper;
using TestFramework.Types;
using TestFramework.Extensions;
using TestFramework;

namespace PageObjects
{
    class AuthPageUsingFramework : BasePage
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Locators for elements
        /// </summary>
        private readonly ElementLocator pageHeader = new ElementLocator(Locator.XPath, "//h3[.='Basic Auth']"),
                                        congratulationsInfo = new ElementLocator(Locator.CssSelector, ".example>p");

        public AuthPageUsingFramework(DriverContext driverContext)
            : base(driverContext)
        {
            Logger.Info("Waiting for page to open");
            this.Driver.IsElementPresent(this.pageHeader, BaseConfiguration.ShortTimeout);
        }

        public string GetCongratulationsInfo
        {
            get
            {
                var text = this.Driver.GetElement(this.congratulationsInfo, "Trying to get congratulations Info").Text;
                Logger.Info(CultureInfo.CurrentCulture, "Text from page '{0}'", text);
                return text;
            }
        }

        public object FilesHelper { get; private set; }

        public string SaveSourcePage()
        {
            return this.DriverContext.SavePageSource(this.DriverContext.TestTitle);
        }

        public void CheckIfPageSourceSaved()
        {
            //var name = this.DriverContext.TestTitle + FilesHelper.ReturnFileExtension(FileType.Html);
            //FilesHelper.WaitForFileOfGivenName(5, name, this.DriverContext.PageSourceFolder);
        }
    }

}

