using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace HyperlinksManager.Settings
{
    public class RegistrySettings
    {
        public static string InitialDirectoryFolder = "InitDir";
        public static string RecentFilesFolder = "Recent";

        private string rootPath;
        public RegistrySettings(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public string InitialDirectory
        {
            get { return GetInitialDirectory(); }
            set { SetInitialDirectory(value); }
        }

        private string GetInitialDirectory()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(this.rootPath);

            string initialDirectory = (string)key.GetValue(
                RegistrySettings.InitialDirectoryFolder,                          // value name
                Directory.GetCurrentDirectory());                                 // default value
            return initialDirectory;
        }

        private void SetInitialDirectory(string initialDirectory)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(this.rootPath);
            key.SetValue(RegistrySettings.InitialDirectoryFolder, initialDirectory);
        }
    }
}
