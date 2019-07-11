using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;

namespace PageObjects
{
    public class LoginPage
    {
        
        //Deprecated in c sharp but still used in java pagefactory
        //[FindsBy(How = How.XPath, Using = "//*[@id='search-field-input' or @id='search-input']")]
        //[CacheLookup]
        //public IWebElement SearchField { get; set; }

        //[FindsBy(How = How.XPath, Using = "//a[@href='/beta/login']" ) ]
        //[CacheLookup]
        //public IWebElement SignInLink { get; set; }

        //private IWebDriver driver;



        //public LoginPage(IWebDriver driver){

        //     PageFactory.InitElements(driver, this);
        //    //PageFactory.InitElements(this, new RetryingElementLocator(driver, TimeSpan.FromSeconds(20)));

        //}

        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver) => _driver = driver;

        IWebElement SignInLink => _driver.FindElement(By.XPath("//a[@href='/beta/login']"));
        IWebElement SearchField => _driver.FindElement(By.XPath("//*[@id='search-field-input' or @id='search-input']"));
        



        public LoginPage goToPage()
        {
            _driver.Navigate().GoToUrl("http://SauceLabs.com");
            WebDriverWait wait = new WebDriverWait(_driver, System.TimeSpan.FromSeconds(15));
            wait.Until(Xdriver =>
                    Xdriver.FindElement(By.XPath("//a[@href='/beta/login']")));
            return new LoginPage(_driver);
        }

        public bool SignInLinkPresent()
        {
            if (SignInLink.Displayed)
                return true;
            else
                return false; 
        }

        public void ClickSerach()
        {
            SearchField.Click();
        }
    }
}
