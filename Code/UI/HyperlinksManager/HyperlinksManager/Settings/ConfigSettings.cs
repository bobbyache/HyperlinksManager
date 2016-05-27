using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace HyperlinksManager.Settings
{
    public static class ConfigSettings
    {
        private static string registryFolder;
        public static string RegistryPath
        {
            get
            {
                if (string.IsNullOrEmpty(registryFolder))
                    registryFolder = ConfigurationManager.AppSettings["RegistryPath"];

                return registryFolder;
            }
            set
            {
                ConfigurationManager.AppSettings["RegistryPath"] = value;
            }
        }

        private static string applicationTitle;
        public static string ApplicationTitle
        {
            get
            {
                if (string.IsNullOrEmpty(applicationTitle))
                    applicationTitle = ConfigurationManager.AppSettings["ApplicationTitle"];

                return applicationTitle;
            }
        }

        private static string dataConnectionsFile;
        public static string DataConnectionsFile
        {
            get
            {
                if (string.IsNullOrEmpty(dataConnectionsFile))
                    dataConnectionsFile = ConfigurationManager.AppSettings["ConnectionFile"];

                return dataConnectionsFile;
            }
        }

        public static void Refresh()
        {
            registryFolder = ConfigurationManager.AppSettings["RegistryPath"];
            dataConnectionsFile = ConfigurationManager.AppSettings["ConnectionFile"];
            applicationTitle = ConfigurationManager.AppSettings["ApplicationTitle"];
        }

    }
}
