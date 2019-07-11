using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Driver
{
   public  class DriverOptionsSetEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DriverOptionsSetEventArgs" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public DriverOptionsSetEventArgs(DriverOptions options)
        {
            this.DriverOptions = options;
        }

        /// <summary>
        /// Gets the current capabilities
        /// </summary>
        public DriverOptions DriverOptions { get; }
    }

}

