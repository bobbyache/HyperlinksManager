using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.IO;

namespace HyperlinksManager.RecentFiles
{
    public class RecentProjectMenu
    {
        //rewritten for WPF !!!
        public event EventHandler<RecentProjectEventArgs> RecentProjectOpened;

        private MenuItem menuItem;
        private IRecentFiles recentFiles;

        public string CurrentlyOpenedFile { get; set; }

        public RecentProjectMenu(MenuItem menuItem, IRecentFiles recentFiles)
        {
            this.menuItem = menuItem;
            this.recentFiles = recentFiles;
            this.recentFiles.LoadList();
            this.menuItem.SubmenuOpened += menuItem_SubmenuOpened;
            createDummyRecentItem();
        }

        ~RecentProjectMenu()
        {
            this.menuItem.SubmenuOpened += menuItem_SubmenuOpened;
        }

        private void menuItem_SubmenuOpened(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach (MenuItem item in this.menuItem.Items)
                item.Click -= item_Click;

            this.menuItem.Items.Clear();

            if (this.recentFiles.FileList.Count > 0)
            {
                foreach (RecentFile recentFile in recentFiles.FileList)
                {
                    MenuItem item = new MenuItem();

                    item.Click += new System.Windows.RoutedEventHandler(item_Click);
                    item.Header = recentFile.GetDisplayText(recentFiles.MaxDisplayNameLength, Directory.GetCurrentDirectory());
                    item.Tag = recentFile;
                    this.menuItem.Items.Add(item);
                    if (recentFile.FullPath == this.CurrentlyOpenedFile)
                        item.IsEnabled = false;
                }
            }
            else
            {
                createDummyRecentItem();
            }
        }

        public void Notify(string filePath)
        {
            this.recentFiles.Add(filePath);
        }

        public void Remove(string filePath)
        {
            this.recentFiles.Remove(filePath);
        }

        private void item_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (RecentProjectOpened != null)
                RecentProjectOpened(this, new RecentProjectEventArgs(((MenuItem)e.OriginalSource).Tag as RecentFile));
        }

        private MenuItem createDummyRecentItem()
        {
            this.menuItem.Items.Clear();

            MenuItem dummyItem = new MenuItem();
            dummyItem.Header = "{empty}";
            dummyItem.IsEnabled = false;
            this.menuItem.Items.Add(dummyItem);
            return dummyItem;
        }
    }
}
