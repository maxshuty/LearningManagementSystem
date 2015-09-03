using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BootcampLMS.Data
{
    public class Settings
    {
        private static string _connectionString;

        private static string _machineSpecificConnection;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    //if (System.Diagnostics.Debugger.IsAttached == false)
                    //    System.Diagnostics.Debugger.Launch();

                    if (System.Net.Dns.GetHostName() == "20150116KINGA1")
                        _machineSpecificConnection = "BootcampLMSAndy";
                    else if (System.Net.Dns.GetHostName() == "Lilo")
                        _machineSpecificConnection = "BootcampLMSAndy";
                    else
                    {
                        _machineSpecificConnection = "BootcampLMSMax";
                    }

                    _connectionString = ConfigurationManager.ConnectionStrings[_machineSpecificConnection].ConnectionString;
                    Console.WriteLine(_connectionString);
                }

                return _connectionString;
            }
        }
    }
}