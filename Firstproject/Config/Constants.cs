using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelLTSelenium
{
    public static class Constants
    {
        internal static string Username = "Your User Name";
        internal static string AccessKey = "Your Access Key";

        //internal static boolean tunnel = Environment.GetEnvironmentVariable("TUNNEL");
        internal static string seleniumPort = Environment.GetEnvironmentVariable("SELENIUM_PORT");
        internal static string build = "C# Sample App";
        internal static string seleniumHost = Environment.GetEnvironmentVariable("SELENIUM_HOST");

    }
}