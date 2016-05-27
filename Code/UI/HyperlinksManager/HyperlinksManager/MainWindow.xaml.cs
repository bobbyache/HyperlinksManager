using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using XmlHyperlinks_Test;
using HyperlinksManager.Settings;
using HyperlinksManager.RecentFiles;
using System.Diagnostics;

namespace HyperlinksManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RecentProjectMenu recentProjectMenu;
        RegistrySettings registrySettings;
        HyperlinkInterface hyperlinkInterface = new HyperlinkInterface();

        public MainWindow()
        {
            InitializeComponent();
            registrySettings = new RegistrySettings(ConfigSettings.RegistryPath);

            RecentFiles.RecentFiles recentFiles = new RecentFiles.RecentFiles();
            recentFiles.MaxNoOfFiles = 15;
            recentFiles.RegistryPath = ConfigSettings.RegistryPath;
            recentFiles.RegistrySubFolder = RegistrySettings.RecentFilesFolder;
            recentFiles.MaxDisplayNameLength = 80;

            recentProjectMenu = new RecentProjectMenu(RecentFilesMenu, recentFiles);
            recentProjectMenu.RecentProjectOpened += recentProjectMenu_RecentProjectOpened;
        }

        private void recentProjectMenu_RecentProjectOpened(object sender, RecentProjectEventArgs e)
        {
            OpenFile(e.RecentFile.FullPath);
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hyperlinkInterface.SelectedHyperlink = listBox.SelectedItem as HyperlinkItem;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string fileName;
            if (SystemDialogs.SaveFile(this, string.Empty, out fileName))
            {
                hyperlinkInterface.CreateNewFile(fileName);
                recentProjectMenu.Notify(fileName);
                Search();
                UpdateStatusInfo();
            }
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string fileName;
            if (SystemDialogs.OpenFile(this, string.Empty, out fileName))
            {
                hyperlinkInterface.OpenFile(fileName);
                recentProjectMenu.Notify(fileName);
                Search();
                UpdateStatusInfo();
            }
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string fileName;
            if (SystemDialogs.SaveFile(this, string.Empty, out fileName))
            {
                hyperlinkInterface.SaveFileAs(fileName);
                recentProjectMenu.Notify(fileName);
                Search();
                UpdateStatusInfo();
            }
        }

        private void AddLink_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddLink();
        }

        private void EditLink_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditLink();
        }

        private void Search_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Search();
        }

        private void FileOperation_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = hyperlinkInterface.FileLoaded;
            if (navigationHyperlink != null)
                navigationHyperlink.IsEnabled = e.CanExecute;
        }

        private void Search()
        {
            hyperlinkInterface.KeywordFilter = txtKeywords.Text;
            this.listBox.ItemsSource = hyperlinkInterface.HyperlinkList;
            this.listBox.SelectedItem = hyperlinkInterface.SelectedHyperlink;
            this.listBox.ScrollIntoView(this.listBox.SelectedItem);
        }

        private void AddLink()
        {
            //http://msdn.microsoft.com/en-us/library/aa969773(v=vs.85).aspx
            EditHyperlink editHyperlink = new EditHyperlink();
            editHyperlink.Owner = this;
            editHyperlink.CurrentHyperlink = new HyperlinkItem();
            editHyperlink.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            bool? result = editHyperlink.ShowDialog();
            if (result.HasValue && result.Value)
            {
                hyperlinkInterface.AddHyperlink(editHyperlink.CurrentHyperlink);
                this.listBox.ItemsSource = hyperlinkInterface.HyperlinkList;
                this.listBox.SelectedItem = hyperlinkInterface.SelectedHyperlink;
                this.listBox.ScrollIntoView(this.listBox.SelectedItem);
            }
        }

        private void EditLink()
        {
            if (listBox.SelectedItem != null)
            {
                EditHyperlink editHyperlink = new EditHyperlink();
                //editHyperlink.Owner = Application.Current.MainWindow;
                editHyperlink.Owner = this;
                editHyperlink.CurrentHyperlink = listBox.SelectedItem as HyperlinkItem;
                editHyperlink.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                bool? result = editHyperlink.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    //hyperlinkInterface.AddHyperlink(editHyperlink.CurrentHyperlink);
                    hyperlinkInterface.UpdateHyperlink(editHyperlink.CurrentHyperlink);
                    this.listBox.ItemsSource = hyperlinkInterface.HyperlinkList;
                    this.listBox.SelectedItem = hyperlinkInterface.SelectedHyperlink;
                    this.listBox.ScrollIntoView(this.listBox.SelectedItem);
                }
            }
        }

        private void OpenFile(string fileName)
        {
            if (hyperlinkInterface.OpenFile(fileName))
            {
                recentProjectMenu.Notify(fileName);
                Search();
                UpdateStatusInfo();
            }
            else
            {
                MessageBox.Show(this, "Hyperlink Manager could not find this file at the specified path.", ConfigSettings.ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Error);
                recentProjectMenu.Remove(fileName);
            }
        }

        private void UpdateStatusInfo()
        {
            this.Title = string.Format("{0} = {1}", ConfigSettings.ApplicationTitle, hyperlinkInterface.CurrentFileTitle);
            statusBarFilePath.Content = hyperlinkInterface.CurrentFile;
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;

        }
    }
}
