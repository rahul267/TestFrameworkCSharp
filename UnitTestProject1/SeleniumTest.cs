using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using PageObjects;
using System;


namespace UnitTestProject1
{
    [TestFixture]
    public class Chrome_Sample_test
    {
        private IWebDriver driver;
        public string homeURL;
        

        [SetUp]
        public void SetupTest()
        {
            homeURL = "http://SauceLabs.com";
            driver = new ChromeDriver();
            

        }



        [Test(Description = "Check SauceLab Homepage for Login Link")]
        [Ignore("Check another one")]
        public void Login_is_on_home_page()
        {


            homeURL = "https://www.SauceLabs.com";
            driver.Navigate().GoToUrl(homeURL);
            WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(15));
            wait.Until(driver =>
                     driver.FindElement(By.XPath("//a[@href='/beta/login']")));
            IWebElement element = driver.FindElement(By.XPath("//a[@href='/beta/login']"));

            Assert.AreEqual("Sign In", element.GetAttribute("text"));


        }


        [Test(Description = "Check SauceLab Homepage for Login Link with POM and Page Factory")]
        public void Login_is_on_home_page_POM()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.goToPage();
            Assert.IsTrue(loginPage.SignInLinkPresent());
        }


        [TearDown]
        public void TearDownTest()
        {
            driver.Close();
        }





    }


}
