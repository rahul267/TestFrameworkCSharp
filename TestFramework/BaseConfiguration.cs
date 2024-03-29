﻿using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.Factories;

namespace TestFramework
{
   public class BaseConfiguration
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly NLog.Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the Driver.
        /// </summary>
        /// <example>How to use it: <code>
        /// if (BaseConfiguration.TestBrowser == BrowserType.Firefox)
        ///     {
        ///     this.Driver.GetElement(this.fileLink.Format(fileName), "Click on file").Click();
        ///     };
        /// </code></example>
        public static BrowserType TestBrowser
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Browser value from App.config '{0}'", ConfigurationManager.AppSettings["browser"]);
                BrowserType browserType;
                bool supportedBrowser = Enum.TryParse(ConfigurationManager.AppSettings["browser"], out browserType);

                if (supportedBrowser)
                {
                    return browserType;
                }

                return BrowserType.None;
            }
        }

        /// <summary>
        /// Gets the Driver capabilities.
        /// </summary>
        public static BrowserType TestBrowserCapabilities
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Driver Capabilities value from App.config '{0}'", ConfigurationManager.AppSettings["DriverCapabilities"]);
                BrowserType browserType;
                bool supportedBrowser = Enum.TryParse(ConfigurationManager.AppSettings["DriverCapabilities"], out browserType);

                if (supportedBrowser)
                {
                    return browserType;
                }

                return BrowserType.None;
            }
        }

        /// <summary>
        /// Gets the path to firefox profile.
        /// </summary>
        public static string PathToFirefoxProfile
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Gets the path to firefox profile from App.config '{0}'", ConfigurationManager.AppSettings["PathToFirefoxProfile"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PathToFirefoxProfile"]))
                {
                    return string.Empty;
                }

                return ConfigurationManager.AppSettings["PathToFirefoxProfile"];
            }
        }

        /// <summary>
        /// Gets the application protocol (http or https).
        /// </summary>
        public static string Protocol
        {
            get { return ConfigurationManager.AppSettings["protocol"]; }
        }

        /// <summary>
        /// Gets the application host name.
        /// </summary>
        public static string Host
        {
            get { return ConfigurationManager.AppSettings["host"]; }
        }

        /// <summary>
        /// Gets the url.
        /// </summary>
        public static string Url
        {
            get { return ConfigurationManager.AppSettings["url"]; }
        }

        /// <summary>
        /// Gets the browser proxy.
        /// </summary>
        public static string Proxy
        {
            get { return ConfigurationManager.AppSettings["proxy"]; }
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public static string Username
        {
            get { return ConfigurationManager.AppSettings["username"]; }
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        public static string Password
        {
            get { return ConfigurationManager.AppSettings["password"]; }
        }

        /// <summary>
        /// Gets the java script or ajax waiting time [seconds].
        /// </summary>
        /// <example>How to use it: <code>
        /// this.Driver.IsElementPresent(this.statusCodeHeader, BaseConfiguration.MediumTimeout);
        /// </code></example>
        public static double MediumTimeout
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["mediumTimeout"], CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Gets the page load waiting time [seconds].
        /// </summary>
        /// <example>How to use it: <code>
        /// element.GetElement(locator, BaseConfiguration.LongTimeout, e => e.Displayed, customMessage);
        /// </code></example>
        public static double LongTimeout
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["longTimeout"], CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Gets the assertion waiting time [seconds].
        /// </summary>
        /// <example>How to use it: <code>
        /// this.Driver.IsElementPresent(this.downloadPageHeader, BaseConfiguration.ShortTimeout);
        /// </code></example>
        public static double ShortTimeout
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["shortTimeout"], CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Gets the Implicitly Wait time [milliseconds].
        /// </summary>
        public static double ImplicitlyWaitMilliseconds
        {
            get { return Convert.ToDouble(ConfigurationManager.AppSettings["ImplicitlyWaitMilliseconds"], CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Gets the path and file name of the Firefox browser executable
        /// </summary>
        public static string FirefoxBrowserExecutableLocation
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Gets the path and file name of the Firefox browser executable from App.config '{0}'", ConfigurationManager.AppSettings["FirefoxBrowserExecutableLocation"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["FirefoxBrowserExecutableLocation"]))
                {
                    return string.Empty;
                }

                return ConfigurationManager.AppSettings["FirefoxBrowserExecutableLocation"];
            }
        }

        /// <summary>
        /// Gets the path and file name of the Chrome browser executable
        /// </summary>
        public static string ChromeBrowserExecutableLocation
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Gets the path and file name of the Chrome browser executable from App.config '{0}'", ConfigurationManager.AppSettings["FirefoxBrowserExecutableLocation"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ChromeBrowserExecutableLocation"]))
                {
                    return string.Empty;
                }

                return ConfigurationManager.AppSettings["ChromeBrowserExecutableLocation"];
            }
        }

        /// <summary>
        /// Gets the Remote Web Driver hub url
        /// </summary>
        public static Uri RemoteWebDriverHub
        {
            get
            {
                return new Uri(ConfigurationManager.AppSettings["RemoteWebDriverHub"]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether enable full desktop screen shot. False by default.
        /// </summary>
        public static bool FullDesktopScreenShotEnabled
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Full Desktop Screen Shot Enabled value from App.config '{0}'", ConfigurationManager.AppSettings["FullDesktopScreenShotEnabled"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["FullDesktopScreenShotEnabled"]))
                {
                    return false;
                }

                if (ConfigurationManager.AppSettings["FullDesktopScreenShotEnabled"].ToLower(CultureInfo.CurrentCulture).Equals("true"))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets specified path to the directory containing InternetExplorer Driver.
        /// </summary>
        public static string PathToInternetExplorerDriverDirectory
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Path to the directory containing Internet Explorer Driver from App.config '{0}'", ConfigurationManager.AppSettings["PathToInternetExplorerDriverDirectory"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PathToInternetExplorerDriverDirectory"]))
                {
                    return string.Empty;
                }

                return ConfigurationManager.AppSettings["PathToInternetExplorerDriverDirectory"];
            }
        }

        /// <summary>
        /// Gets specified path to the directory containing Edge Driver.
        /// </summary>
        public static string PathToEdgeDriverDirectory
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Path to the directory containing Edge Driver from App.config '{0}'", ConfigurationManager.AppSettings["PathToEdgeDriverDirectory"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PathToEdgeDriverDirectory"]))
                {
                    return string.Empty;
                }

                return ConfigurationManager.AppSettings["PathToEdgeDriverDirectory"];
            }
        }

        /// <summary>
        /// Gets specified path to the directory containing ChromeDriver.
        /// </summary>
        public static string PathToChromeDriverDirectory
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Path to the directory containing Chrome Driver from App.config '{0}'", ConfigurationManager.AppSettings["PathToChromeDriverDirectory"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PathToChromeDriverDirectory"]))
                {
                    return string.Empty;
                }

                return ConfigurationManager.AppSettings["PathToChromeDriverDirectory"];
            }
        }

        /// <summary>
        /// Gets specified path to the directory containing Firefox Driver.
        /// </summary>
        public static string PathToFirefoxDriverDirectory
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Path to the directory containing Firefox Driver from App.config '{0}'", ConfigurationManager.AppSettings["PathToFirefoxDriverDirectory"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["PathToFirefoxDriverDirectory"]))
                {
                    return string.Empty;
                }

                return ConfigurationManager.AppSettings["PathToFirefoxDriverDirectory"];
            }
        }

        /// <summary>
        /// Gets a value indicating whether logs JavaScript errors from a browser. False by default.
        /// </summary>
        public static bool JavaScriptErrorLogging
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "JavaScript error logging value from App.config '{0}'", ConfigurationManager.AppSettings["JavaScriptErrorLogging"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["JavaScriptErrorLogging"]))
                {
                    return false;
                }

                if (ConfigurationManager.AppSettings["JavaScriptErrorLogging"].ToLower(CultureInfo.CurrentCulture).Equals("true"))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets JavaScript error types from a browser. "SyntaxError,EvalError,ReferenceError,RangeError,TypeError,URIError,Refused to display,Internal Server Error,Cannot read property" by default.
        /// </summary>
        public static Collection<string> JavaScriptErrorTypes
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "JavaScript error logging value from App.config '{0}'", ConfigurationManager.AppSettings["JavaScriptErrorTypes"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["JavaScriptErrorTypes"]))
                {
                    return new Collection<string>
                    {
                        "SyntaxError",
                        "EvalError",
                        "ReferenceError",
                        "RangeError",
                        "TypeError",
                        "URIError",
                        "Refused to display",
                        "Internal Server Error",
                        "Cannot read property"
                    };
                }

                return new Collection<string>(ConfigurationManager.AppSettings["JavaScriptErrorTypes"].Split(new char[] { ',' }));
            }
        }

        /// <summary>
        /// Gets a value indicating whether enable legacy implementation for Firefox.
        /// </summary>
        public static bool FirefoxUseLegacyImplementation
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Firefox Use Legacy Implementation Enabled value from App.config '{0}'", ConfigurationManager.AppSettings["FirefoxUseLegacyImplementation"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["FirefoxUseLegacyImplementation"]))
                {
                    return false;
                }

                if (ConfigurationManager.AppSettings["FirefoxUseLegacyImplementation"].ToLower(CultureInfo.CurrentCulture).Equals("true"))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether enable full desktop screen shot. True by default.
        /// </summary>
        public static bool SeleniumScreenShotEnabled
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Selenium Screen Shot Enabled value from App.config '{0}'", ConfigurationManager.AppSettings["SeleniumScreenShotEnabled"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SeleniumScreenShotEnabled"]))
                {
                    return true;
                }

                if (ConfigurationManager.AppSettings["SeleniumScreenShotEnabled"].ToLower(CultureInfo.CurrentCulture).Equals("true"))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether enable EventFiringWebDriver.
        /// </summary>
        public static bool EnableEventFiringWebDriver
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Enable EventFiringWebDriver from App.config '{0}'", ConfigurationManager.AppSettings["Enable EventFiringWebDriver"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["EnableEventFiringWebDriver"]))
                {
                    return false;
                }

                if (ConfigurationManager.AppSettings["EnableEventFiringWebDriver"].ToLower(CultureInfo.CurrentCulture).Equals("true"))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether use CurrentDirectory for path where assembly files are located.
        /// </summary>
        public static bool UseCurrentDirectory
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Use Current Directory value from App.config '{0}'", ConfigurationManager.AppSettings["UseCurrentDirectory"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["UseCurrentDirectory"]))
                {
                    return false;
                }

                if (ConfigurationManager.AppSettings["UseCurrentDirectory"].ToLower(CultureInfo.CurrentCulture).Equals("true"))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [get page source enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [get page source enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool GetPageSourceEnabled
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Get Page Source Enabled value from App.config '{0}'", ConfigurationManager.AppSettings["GetPageSourceEnabled"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["GetPageSourceEnabled"]))
                {
                    return true;
                }

                if (ConfigurationManager.AppSettings["GetPageSourceEnabled"].ToLower(CultureInfo.CurrentCulture).Equals("true"))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the download folder key value
        /// </summary>
        public static string DownloadFolder
        {
            get { return ConfigurationManager.AppSettings["DownloadFolder"]; }
        }

        /// <summary>
        /// Gets the screen shot folder key value
        /// </summary>
        public static string ScreenShotFolder
        {
            get { return ConfigurationManager.AppSettings["ScreenShotFolder"]; }
        }

        /// <summary>
        /// Gets the page source folder key value
        /// </summary>
        public static string PageSourceFolder
        {
            get { return ConfigurationManager.AppSettings["PageSourceFolder"]; }
        }

        /// <summary>
        /// Gets the URL value 'Protocol://HostURL'.
        /// </summary>
        /// <example>How to use it: <code>
        /// var url = BaseConfiguration.GetUrlValue;
        /// </code></example>
        public static string GetUrlValue
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture, "{0}://{1}{2}", Protocol, Host, Url);
            }
        }

        /// <summary>
        /// Gets the URL value with user credentials 'Protocol://Username:Password@HostURL'.
        /// </summary>
        /// <example>How to use it: <code>
        /// var url = BaseConfiguration.GetUrlValueWithUserCredentials;
        /// </code></example>
        public static string GetUrlValueWithUserCredentials
        {
            get
            {
                return string.Format(
                    CultureInfo.CurrentCulture,
                    "{0}://{1}:{2}@{3}{4}",
                    Protocol,
                    Username,
                    Password,
                    Host,
                    Url);
            }
        }

        /// <summary>
        /// Gets a value indicating whether enable AngularJS synchronization. False by default.
        /// </summary>
        public static bool SynchronizationWithAngularEnabled
        {
            get
            {
                Logger.Trace(CultureInfo.CurrentCulture, "Angular synchronization Enabled value from App.config '{0}'", ConfigurationManager.AppSettings["SynchronizationWithAngularEnabled"]);
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["SynchronizationWithAngularEnabled"]))
                {
                    return false;
                }

                if (ConfigurationManager.AppSettings["SynchronizationWithAngularEnabled"].ToLower(CultureInfo.CurrentCulture).Equals("true"))
                {
                    return true;
                }

                return false;
            }
        }

    }
}
