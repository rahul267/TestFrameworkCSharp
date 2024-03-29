﻿using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.Factories;
using TestFramework.Helper;
using TestFramework.Logging;
using TestFramework.Types;

namespace TestFramework.Driver
{
  public partial  class DriverContext
    {
        private static readonly NLog.Logger Logger = LogManager.GetLogger("DRIVER");
        private readonly Collection<ErrorDetail> verifyMessages = new Collection<ErrorDetail>();

        /// <summary>
        /// Gets or sets the handle to current driver.
        /// </summary>
        /// <value>
        /// The handle to driver.
        /// </value>
        private IWebDriver driver;

        private TestLogger logTest;

        /// <summary>
        /// Occurs when [driver options set].
        /// </summary>
        public event EventHandler<DriverOptionsSetEventArgs> DriverOptionsSet;

        /// <summary>
        /// Gets instance of Performance PerformanceMeasures class
        /// </summary>
        public PerformanceHelper PerformanceMeasures { get; } = new PerformanceHelper();

        /// <summary>
        /// Gets or sets the test title.
        /// </summary>
        /// <value>
        /// The test title.
        /// </value>
        public string TestTitle { get; set; }

        /// <summary>
        /// Gets or sets the Environment Browsers from App.config
        /// </summary>
        public string CrossBrowserEnvironment { get; set; }

        /// <summary>
        /// Gets Sets Folder name for ScreenShot
        /// </summary>
        public string ScreenShotFolder
        {
            get
            {
                return FilesHelper.GetFolder(BaseConfiguration.ScreenShotFolder, this.CurrentDirectory);
            }
        }

        /// <summary>
        /// Gets Sets Folder name for Download
        /// </summary>
        public string DownloadFolder
        {
            get
            {
                return FilesHelper.GetFolder(BaseConfiguration.DownloadFolder, this.CurrentDirectory);
            }
        }

        /// <summary>
        /// Gets Sets Folder name for PageSource
        /// </summary>
        public string PageSourceFolder
        {
            get
            {
                return FilesHelper.GetFolder(BaseConfiguration.PageSourceFolder, this.CurrentDirectory);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [test failed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [test failed]; otherwise, <c>false</c>.
        /// </value>
        public bool IsTestFailed { get; set; }

        /// <summary>
        /// Gets or sets test logger
        /// </summary>
        public TestLogger LogTest
        {
            get
            {
                return this.logTest ?? (this.logTest = new TestLogger());
            }

            set
            {
                this.logTest = value;
            }
        }

        /// <summary>
        /// Gets driver Handle
        /// </summary>
        public IWebDriver Driver
        {
            get
            {
                return this.driver;
            }
        }

        /// <summary>
        /// Gets all verify messages
        /// </summary>
        public Collection<ErrorDetail> VerifyMessages
        {
            get
            {
                return this.verifyMessages;
            }
        }

        /// <summary>
        /// Gets or sets directory where assembly files are located
        /// </summary>
        public string CurrentDirectory { get; set; }

        private FirefoxOptions FirefoxOptions
        {
            get
            {
                FirefoxOptions options = new FirefoxOptions();

                if (!string.IsNullOrEmpty(BaseConfiguration.PathToFirefoxProfile))
                {
                    try
                    {
                        var pathToCurrentUserProfiles = BaseConfiguration.PathToFirefoxProfile; // Path to profile
                        var pathsToProfiles = System.IO.Directory.GetDirectories(pathToCurrentUserProfiles, "*.default", SearchOption.TopDirectoryOnly);

                        options.Profile = new FirefoxProfile(pathsToProfiles[0]);
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        Logger.Error(CultureInfo.CurrentCulture, "problem with loading firefox profile {0}", e.Message);
                    }
                }

                options.SetPreference("toolkit.startup.max_resumed_crashes", "999999");
                options.SetPreference("network.automatic-ntlm-auth.trusted-uris", BaseConfiguration.Host ?? string.Empty);

                // retrieving settings from config file
                var firefoxPreferences = ConfigurationManager.GetSection("FirefoxPreferences") as NameValueCollection;
                var firefoxExtensions = ConfigurationManager.GetSection("FirefoxExtensions") as NameValueCollection;

                // preference for downloading files
                options.SetPreference("browser.download.dir", this.DownloadFolder);
                options.SetPreference("browser.download.folderList", 2);
                options.SetPreference("browser.download.managershowWhenStarting", false);
                options.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/vnd.ms-excel, application/x-msexcel, application/pdf, text/csv, text/html, application/octet-stream");

                // disable Firefox's built-in PDF viewer
                options.SetPreference("pdfjs.disabled", true);

                // disable Adobe Acrobat PDF preview plugin
                options.SetPreference("plugin.scan.Acrobat", "99.0");
                options.SetPreference("plugin.scan.plid.all", false);

                options.UseLegacyImplementation = BaseConfiguration.FirefoxUseLegacyImplementation;

                // set browser proxy for Firefox
                if (!string.IsNullOrEmpty(BaseConfiguration.Proxy))
                {
                    options.Proxy = this.CurrentProxy();
                }

                // if there are any extensions
                if (firefoxExtensions != null)
                {
                    // loop through all of them
                    for (var i = 0; i < firefoxExtensions.Count; i++)
                    {
                        Logger.Trace(CultureInfo.CurrentCulture, "Installing extension {0}", firefoxExtensions.GetKey(i));
                        try
                        {
                            options.Profile.AddExtension(firefoxExtensions.GetKey(i));
                        }
                        catch (FileNotFoundException)
                        {
                            Logger.Trace(CultureInfo.CurrentCulture, "Installing extension {0}", this.CurrentDirectory + FilesHelper.Separator + firefoxExtensions.GetKey(i));
                            options.Profile.AddExtension(this.CurrentDirectory + FilesHelper.Separator + firefoxExtensions.GetKey(i));
                        }
                    }
                }

                options = this.AddFirefoxArguments(options);

                // custom preferences
                // if there are any settings
                if (firefoxPreferences == null)
                {
                    return options;
                }

                // loop through all of them
                for (var i = 0; i < firefoxPreferences.Count; i++)
                {
                    Logger.Trace(CultureInfo.CurrentCulture, "Set custom preference '{0},{1}'", firefoxPreferences.GetKey(i), firefoxPreferences[i]);

                    // and verify all of them
                    switch (firefoxPreferences[i])
                    {
                        // if current settings value is "true"
                        case "true":
                            options.SetPreference(firefoxPreferences.GetKey(i), true);
                            break;

                        // if "false"
                        case "false":
                            options.SetPreference(firefoxPreferences.GetKey(i), false);
                            break;

                        // otherwise
                        default:
                            int temp;

                            // an attempt to parse current settings value to an integer. Method TryParse returns True if the attempt is successful (the string is integer) or return False (if the string is just a string and cannot be cast to a number)
                            if (int.TryParse(firefoxPreferences.Get(i), out temp))
                            {
                                options.SetPreference(firefoxPreferences.GetKey(i), temp);
                            }
                            else
                            {
                                options.SetPreference(firefoxPreferences.GetKey(i), firefoxPreferences[i]);
                            }

                            break;
                    }
                }

                return options;
            }
        }

        private ChromeOptions ChromeOptions
        {
            get
            {
                ChromeOptions options = new ChromeOptions();

                // retrieving settings from config file
                var chromePreferences = ConfigurationManager.GetSection("ChromePreferences") as NameValueCollection;
                var chromeExtensions = ConfigurationManager.GetSection("ChromeExtensions") as NameValueCollection;
                var chromeArguments = ConfigurationManager.GetSection("ChromeArguments") as NameValueCollection;

                options.AddUserProfilePreference("profile.default_content_settings.popups", 0);
                options.AddUserProfilePreference("download.default_directory", this.DownloadFolder);
                options.AddUserProfilePreference("download.prompt_for_download", false);

                // set browser proxy for chrome
                if (!string.IsNullOrEmpty(BaseConfiguration.Proxy))
                {
                    options.Proxy = this.CurrentProxy();
                }

                // if there are any extensions
                if (chromeExtensions != null)
                {
                    // loop through all of them
                    for (var i = 0; i < chromeExtensions.Count; i++)
                    {
                        Logger.Trace(CultureInfo.CurrentCulture, "Installing extension {0}", chromeExtensions.GetKey(i));
                        try
                        {
                            options.AddExtension(chromeExtensions.GetKey(i));
                        }
                        catch (FileNotFoundException)
                        {
                            Logger.Trace(CultureInfo.CurrentCulture, "Installing extension {0}", this.CurrentDirectory + FilesHelper.Separator + chromeExtensions.GetKey(i));
                            options.AddExtension(this.CurrentDirectory + FilesHelper.Separator + chromeExtensions.GetKey(i));
                        }
                    }
                }

                // if there are any arguments
                if (chromeArguments != null)
                {
                    // loop through all of them
                    for (var i = 0; i < chromeArguments.Count; i++)
                    {
                        Logger.Trace(CultureInfo.CurrentCulture, "Setting Chrome Arguments {0}", chromeArguments.GetKey(i));
                        options.AddArgument(chromeArguments.GetKey(i));
                    }
                }

                // custom preferences
                // if there are any settings
                if (chromePreferences == null)
                {
                    return options;
                }

                // loop through all of them
                for (var i = 0; i < chromePreferences.Count; i++)
                {
                    Logger.Trace(CultureInfo.CurrentCulture, "Set custom preference '{0},{1}'", chromePreferences.GetKey(i), chromePreferences[i]);

                    // and verify all of them
                    switch (chromePreferences[i])
                    {
                        // if current settings value is "true"
                        case "true":
                            options.AddUserProfilePreference(chromePreferences.GetKey(i), true);
                            break;

                        // if "false"
                        case "false":
                            options.AddUserProfilePreference(chromePreferences.GetKey(i), false);
                            break;

                        // otherwise
                        default:
                            int temp;

                            // an attempt to parse current settings value to an integer. Method TryParse returns True if the attempt is successful (the string is integer) or return False (if the string is just a string and cannot be cast to a number)
                            if (int.TryParse(chromePreferences.Get(i), out temp))
                            {
                                options.AddUserProfilePreference(chromePreferences.GetKey(i), temp);
                            }
                            else
                            {
                                options.AddUserProfilePreference(chromePreferences.GetKey(i), chromePreferences[i]);
                            }

                            break;
                    }
                }

                return options;
            }
        }

        private InternetExplorerOptions InternetExplorerOptions
        {
            get
            {
                // retrieving settings from config file
                var internetExplorerPreferences = ConfigurationManager.GetSection("InternetExplorerPreferences") as NameValueCollection;
                var options = new InternetExplorerOptions
                {
                    EnsureCleanSession = true,
                    IgnoreZoomLevel = true,
                };

                // set browser proxy for IE
                if (!string.IsNullOrEmpty(BaseConfiguration.Proxy))
                {
                    options.Proxy = this.CurrentProxy();
                }

                // custom preferences
                // if there are any settings
                if (internetExplorerPreferences == null)
                {
                    return options;
                }

                this.GetInternetExplorerPreferences(internetExplorerPreferences, options);

                return options;
            }
        }

        private EdgeOptions EdgeOptions
        {
            get
            {
                var options = new EdgeOptions();

                // set browser proxy for Edge
                if (!string.IsNullOrEmpty(BaseConfiguration.Proxy))
                {
                    options.Proxy = this.CurrentProxy();
                }

                options.UseInPrivateBrowsing = true;

                return options;
            }
        }

        private SafariOptions SafariOptions
        {
            get
            {
                var options = new SafariOptions();
                options.AddAdditionalCapability("cleanSession", true);

                return options;
            }
        }

        /// <summary>
        /// Takes the screenshot.
        /// </summary>
        /// <returns>An image of the page currently loaded in the browser.</returns>
        public Screenshot TakeScreenshot()
        {
            try
            {
                var screenshotDriver = (ITakesScreenshot)this.driver;
                var screenshot = screenshotDriver.GetScreenshot();
                return screenshot;
            }
            catch (NullReferenceException)
            {
                Logger.Error("Test failed but was unable to get webdriver screenshot.");
            }
            catch (UnhandledAlertException)
            {
                Logger.Error("Test failed but was unable to get webdriver screenshot.");
            }

            return null;
        }

        /// <summary>
        /// Starts the specified Driver.
        /// </summary>
        /// <exception cref="NotSupportedException">When driver not supported</exception>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Driver disposed later in stop method")]
        public void Start()
        {
            switch (BaseConfiguration.TestBrowser)
            {
                case Factories.BrowserType.Firefox:
                    if (!string.IsNullOrEmpty(BaseConfiguration.FirefoxBrowserExecutableLocation))
                    {
                        this.FirefoxOptions.BrowserExecutableLocation = BaseConfiguration.FirefoxBrowserExecutableLocation;
                    }

                    this.driver = string.IsNullOrEmpty(BaseConfiguration.PathToFirefoxDriverDirectory) ? new FirefoxDriver(this.SetDriverOptions(this.FirefoxOptions)) : new FirefoxDriver(BaseConfiguration.PathToFirefoxDriverDirectory, this.SetDriverOptions(this.FirefoxOptions));
                    break;
                case Factories.BrowserType.InternetExplorer:
                case BrowserType.IE:
                    this.driver = string.IsNullOrEmpty(BaseConfiguration.PathToInternetExplorerDriverDirectory) ? new InternetExplorerDriver(this.SetDriverOptions(this.InternetExplorerOptions)) : new InternetExplorerDriver(BaseConfiguration.PathToInternetExplorerDriverDirectory, this.SetDriverOptions(this.InternetExplorerOptions));
                    break;
                case BrowserType.Chrome:
                    if (!string.IsNullOrEmpty(BaseConfiguration.ChromeBrowserExecutableLocation))
                    {
                        this.ChromeOptions.BinaryLocation = BaseConfiguration.ChromeBrowserExecutableLocation;
                    }

                    this.driver = string.IsNullOrEmpty(BaseConfiguration.PathToChromeDriverDirectory) ? new ChromeDriver(this.SetDriverOptions(this.ChromeOptions)) : new ChromeDriver(BaseConfiguration.PathToChromeDriverDirectory, this.SetDriverOptions(this.ChromeOptions));
                    break;
                case BrowserType.Safari:
                    this.driver = new SafariDriver(this.SetDriverOptions(this.SafariOptions));
                    this.CheckIfProxySetForSafari();
                    break;
                case BrowserType.RemoteWebDriver:
                    var driverCapabilitiesConf = ConfigurationManager.GetSection("DriverCapabilities") as NameValueCollection;
                    NameValueCollection settings = ConfigurationManager.GetSection("environments/" + this.CrossBrowserEnvironment) as NameValueCollection;
                    var browserType = this.GetBrowserTypeForRemoteDriver(settings);

                    switch (browserType)
                    {
                        case BrowserType.Firefox:
                            FirefoxOptions firefoxOptions = new FirefoxOptions();
                            this.SetRemoteDriverBrowserOptions(driverCapabilitiesConf, settings, firefoxOptions);
                            this.driver = new RemoteWebDriver(BaseConfiguration.RemoteWebDriverHub, this.SetDriverOptions(firefoxOptions).ToCapabilities());
                            break;
                        case BrowserType.Android:
                        case BrowserType.Chrome:
                            ChromeOptions chromeOptions = new ChromeOptions();
                            this.SetRemoteDriverBrowserOptions(driverCapabilitiesConf, settings, chromeOptions);
                            this.driver = new RemoteWebDriver(BaseConfiguration.RemoteWebDriverHub, this.SetDriverOptions(chromeOptions).ToCapabilities());
                            break;
                        case BrowserType.Iphone:
                        case BrowserType.Safari:
                            SafariOptions safariOptions = new SafariOptions();
                            this.SetRemoteDriverOptions(driverCapabilitiesConf, settings, safariOptions);
                            this.driver = new RemoteWebDriver(BaseConfiguration.RemoteWebDriverHub, this.SetDriverOptions(safariOptions).ToCapabilities());
                            break;
                        case BrowserType.Edge:
                            EdgeOptions egEdgeOptions = new EdgeOptions();
                            this.SetRemoteDriverOptions(driverCapabilitiesConf, settings, egEdgeOptions);
                            this.driver = new RemoteWebDriver(BaseConfiguration.RemoteWebDriverHub, this.SetDriverOptions(egEdgeOptions).ToCapabilities());
                            break;
                        case BrowserType.IE:
                        case BrowserType.InternetExplorer:
                            InternetExplorerOptions internetExplorerOptions = new InternetExplorerOptions();
                            this.SetRemoteDriverBrowserOptions(driverCapabilitiesConf, settings, internetExplorerOptions);
                            this.driver = new RemoteWebDriver(BaseConfiguration.RemoteWebDriverHub, this.SetDriverOptions(internetExplorerOptions).ToCapabilities());
                            break;
                        default:
                            throw new NotSupportedException(
                                string.Format(CultureInfo.CurrentCulture, "Driver {0} is not supported", this.CrossBrowserEnvironment));
                    }

                    break;
                case BrowserType.Edge:
                    this.driver = new EdgeDriver(EdgeDriverService.CreateDefaultService(BaseConfiguration.PathToEdgeDriverDirectory, "MicrosoftWebDriver.exe", 52296), this.SetDriverOptions(this.EdgeOptions));
                    break;
                default:
                    throw new NotSupportedException(
                        string.Format(CultureInfo.CurrentCulture, "Driver {0} is not supported", BaseConfiguration.TestBrowser));
            }

            if (BaseConfiguration.EnableEventFiringWebDriver)
            {
               // this.driver = new MyEventFiringWebDriver(this.driver);
            }
        }

        /// <summary>
        /// Maximizes the current window if it is not already maximized.
        /// </summary>
        public void WindowMaximize()
        {
            this.driver.Manage().Window.Maximize();
        }

        /// <summary>
        /// Deletes all cookies from the page.
        /// </summary>
        public void DeleteAllCookies()
        {
            this.driver.Manage().Cookies.DeleteAllCookies();
        }

        /// <summary>
        /// Stop browser instance.
        /// </summary>
        public void Stop()
        {
            if (this.driver != null)
            {
                this.driver.Quit();
            }
        }
    }

}

