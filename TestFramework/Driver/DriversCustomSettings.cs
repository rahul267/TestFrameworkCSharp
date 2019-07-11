using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Driver
{
    public static class DriversCustomSettings
    {
        private static Dictionary<IWebDriver, bool> driversAngularSynchronizationEnable =
    new Dictionary<IWebDriver, bool>();

        /// <summary>
        /// Method return true or false is driver is synchronized with angular.
        /// </summary>
        /// <param name="driver">Provide driver.</param>
        /// <returns>If driver is synchornized with angular return true if not return false.</returns>
        public static bool IsDriverSynchronizationWithAngular(IWebDriver driver)
        {
            return driversAngularSynchronizationEnable.ContainsKey(driver) && driversAngularSynchronizationEnable[driver];
        }

        /// <summary>
        /// Set angular synchronization for driver.
        /// </summary>
        /// <param name="driver">Provide driver.</param>
        /// <param name="enable">Set true to enable.</param>
        public static void SetAngularSynchronizationForDriver(IWebDriver driver, bool enable)
        {
            if (!enable && driversAngularSynchronizationEnable.ContainsKey(driver))
            {
                driversAngularSynchronizationEnable.Remove(driver);
            }

            if (enable && !driversAngularSynchronizationEnable.ContainsKey(driver))
            {
                driversAngularSynchronizationEnable.Add(driver, true);
            }

            if (enable && driversAngularSynchronizationEnable.ContainsKey(driver))
            {
                driversAngularSynchronizationEnable[driver] = true;
            }
        }

    }
}
