using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Collections;

namespace HyperlinksManager.RecentFiles
{
    public class RecentFiles : IRecentFiles
    {
        #region Private Members

        private List<RecentFile> recentFileList = null;
        private string currentDirectory = string.Empty;
        private const string regEntryName = "file";  // entry name to keep MRU (file0, file1...)

        #endregion

        #region Public Properties

        public List<RecentFile> FileList
        {
            get { return this.recentFileList; }
        }

        public string CurrentDirectory
        {
            get { return this.currentDirectory; }
        }

        public int MaxDisplayNameLength
        {
            get;
            set;
        }

        public int MaxNoOfFiles
        {
            get;
            set;
        }

        public string RegistryPath
        {
            get;
            set;
        }

        public string RegistrySubFolder
        {
            get;
            set;
        }

        #endregion

        public RecentFiles()
        {
            this.currentDirectory = Directory.GetCurrentDirectory();
        }

        #region Public Procedures

        private void WriteList()
        {
            if (string.IsNullOrEmpty(this.RegistrySubFolder))
                throw new ApplicationException("Cannot write to an non-existent registry sub-folder");

            int i, n;

            RegistryKey key = Registry.CurrentUser.CreateSubKey(this.RegistryPath + "\\" + this.RegistrySubFolder);

            if (key != null)
            {
                n = recentFileList.Count;

                for (i = 0; i < this.MaxNoOfFiles; i++)
                {
                    key.DeleteValue(regEntryName + i.ToString(), false);
                }

                for (i = 0; i < n; i++)
                {
                    key.SetValue(regEntryName + i.ToString(), recentFileList[i].FullPath);
                }
            }
        }

        public void LoadList()
        {
            string sKey, s;

            try
            {
                if (recentFileList == null)
                    recentFileList = new List<RecentFile>();
                else
                    recentFileList.Clear();

                RegistryKey key = Registry.CurrentUser.OpenSubKey(this.RegistryPath + "\\" + this.RegistrySubFolder);

                if (key != null)
                {
                    for (int i = 0; i < this.MaxNoOfFiles; i++)
                    {
                        sKey = regEntryName + i.ToString();

                        s = (string)key.GetValue(sKey, "");

                        if (s.Length == 0)
                            break;

                        recentFileList.Add(new RecentFile(sKey, s));
                    }
                }
            }
            catch (Exception ex)
            {
                //Trace.WriteLine("Loading MRU from Registry failed: " + ex.Message);
            }
        }

        public void Add(string file)
        {
            Remove(file);

            // if array has maximum length, remove last element
            if (recentFileList.Count == this.MaxNoOfFiles)
                recentFileList.RemoveAt(this.MaxNoOfFiles - 1);

            // add new file name to the start of array
            recentFileList.Insert(0, new RecentFile("", file));

            WriteList();
            LoadList();
        }

        public void Remove(string file)
        {
            int i = 0;

            IEnumerator myEnumerator = recentFileList.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                if (((RecentFile)myEnumerator.Current).FullPath == file)
                {
                    recentFileList.RemoveAt(i);
                    WriteList();
                    LoadList();
                    return;
                }

                i++;
            }
        }

        #endregion
    }
}
