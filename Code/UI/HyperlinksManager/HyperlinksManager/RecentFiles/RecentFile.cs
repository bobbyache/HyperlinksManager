using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace HyperlinksManager.RecentFiles
{
    public class RecentFile
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathCompactPathEx(
            StringBuilder pszOut,
            string pszPath,
            int cchMax,
            int reserved);

        public RecentFile(string identifier, string fullPath)
        {
            this.identifier = identifier;
            this.fullPath = fullPath;

        }

        private string identifier = string.Empty;
        private string fullPath = string.Empty;

        public string FullPath
        {
            get { return this.fullPath; }
        }

        public string Identifier
        {
            get { return this.identifier; }
        }

        /// <summary>
        /// Get display file name from full name.
        /// </summary>
        /// <param name="fullName">Full file name</param>
        /// <returns>Short display name</returns>
        public string GetDisplayText(int maxLength, string currentDirectory)
        {
            // if file is in current directory, show only file name
            FileInfo fileInfo = new FileInfo(this.FullPath);

            if (fileInfo.DirectoryName == currentDirectory)
                return GetShortDisplayName(fileInfo.Name, maxLength);

            return GetShortDisplayName(this.FullPath, maxLength);
        }

        public override string ToString()
        {
            return this.FullPath;
        }


        /// <summary>
        /// Truncate a path to fit within a certain number of characters 
        /// by replacing path components with ellipses.
        /// 
        /// This solution is provided by CodeProject and GotDotNet C# expert
        /// Richard Deeming.
        /// 
        /// </summary>
        /// <param name="longName">Long file name</param>
        /// <param name="maxLen">Maximum length</param>
        /// <returns>Truncated file name</returns>
        private string GetShortDisplayName(string longName, int maxLen)
        {
            StringBuilder pszOut = new StringBuilder(maxLen + maxLen + 2);  // for safety

            if (PathCompactPathEx(pszOut, longName, maxLen, 0))
            {
                return pszOut.ToString();
            }
            else
            {
                return longName;
            }
        }
    }
}
