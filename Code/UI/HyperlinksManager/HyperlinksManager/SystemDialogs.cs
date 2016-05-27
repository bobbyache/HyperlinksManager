using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows;
using HyperlinksManager.Settings;

namespace HyperlinksManager
{
    public class SystemDialogs
    {
        public static string ProjectFilter = "Hyperlink Files *.xml (*.xml)|*.xml";
        public static string ProjectExtension = "*.xml";

        public static bool OpenFile(Window owner, string initialDirectory, out string filePath)
        {
            filePath = string.Empty;
            
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = string.Format("{0} {1}", ConfigSettings.ApplicationTitle, SystemDialogs.ProjectFilter);
            openDialog.DefaultExt = SystemDialogs.ProjectExtension;
            openDialog.Title = string.Format("Open {0} file", ConfigSettings.ApplicationTitle);
            openDialog.InitialDirectory = initialDirectory;
            openDialog.AddExtension = true;
            openDialog.FilterIndex = 0;

            Nullable<bool> result = openDialog.ShowDialog(owner);
            if (result.HasValue)
            {
                if (result.Value)
                {
                    filePath = openDialog.FileName;
                }
                return result.Value;
            }
            return false;
        }

        public static bool SaveFile(Window owner, string initialDirectory, out string filePath)
        {
            filePath = string.Empty;

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = string.Format("{0} {1}", ConfigSettings.ApplicationTitle, SystemDialogs.ProjectFilter);
            saveDialog.DefaultExt = SystemDialogs.ProjectExtension;
            saveDialog.Title = string.Format("Save {0} file", ConfigSettings.ApplicationTitle);
            saveDialog.InitialDirectory = initialDirectory;
            saveDialog.AddExtension = true;
            saveDialog.FilterIndex = 0;
            saveDialog.OverwritePrompt = true;
            saveDialog.FileName = "";

            Nullable<bool> result = saveDialog.ShowDialog(owner);
            if (result.HasValue)
            {
                if (result.Value)
                {
                    filePath = saveDialog.FileName;
                }
                return result.Value;
            }
            return false;
        }
    }
}
