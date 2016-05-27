//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections.ObjectModel;
//using XmlHyperlinks_Test;
//using HyperlinksManager.RecentFiles;
//using HyperlinksManager.Settings;
//using System.Windows;

//namespace HyperlinksManager.ViewModels
//{
//    public class MainWindowViewModel : ViewModelBase
//    {
//        RecentProjectMenu recentProjectMenu;
//        RegistrySettings registrySettings;
//        HyperlinkInterface hyperlinkInterface = new HyperlinkInterface();

//        ObservableCollection<HyperlinkItem> hyperlinkList = new ObservableCollection<HyperlinkItem>();

//        /// <summary>
//        /// Always add a public parameterless constructor for Expression Blend.
//        /// </summary>
//        public MainWindowViewModel()
//        {
//        }

//        public ObservableCollection<HyperlinkItem> HyperlinkList
//        {
//            get { return this.hyperlinkInterface.HyperlinkList; }
//            private set 
//            {
//                this.hyperlinkList = value; 
//                //this.hyperlinkInterface.HyperlinkList = value; //-- yep, underlying class will need to be modified here.
//                RaisePropertyChanged("HyperlinkList");
//            }
//        }

//        public HyperlinkItem SelectedHyperlink
//        {
//            get { return this.hyperlinkInterface.SelectedHyperlink; }
//            set
//            {
//                this.hyperlinkInterface.SelectedHyperlink = value;
//                RaisePropertyChanged("SelectedHyperlink");
//            }
//        }

//        public bool FileLoaded
//        {
//            get { return this.hyperlinkInterface.FileLoaded; }
//        }

//        public string CurrentFile
//        {
//            get { return this.hyperlinkInterface.CurrentFile; }
//        }

//        public string CurrentFileTitle
//        {
//            get { return this.hyperlinkInterface.CurrentFileTitle; }
//        }

//        public string KeywordFilter
//        {
//            get { return this.hyperlinkInterface.KeywordFilter; }
//            set 
//            { 
//                this.hyperlinkInterface.KeywordFilter = value;
//                RaisePropertyChanged("KeywordFilter");
//            }
//        }

//        public string TopicFilter
//        {
//            get { throw new NotImplementedException(); }
//            set
//            {
//                RaisePropertyChanged("TopicFilter");
//                throw new NotImplementedException();
//            }
//        }

//        public void OpenRecentFile()
//        {
//        }

//        public void OpenFile()
//        {
//            string fileName;
//            if (SystemDialogs.OpenFile(null, string.Empty, out fileName))
//            {
//                hyperlinkInterface.OpenFile(fileName);
//                recentProjectMenu.Notify(fileName);
//                //Search();
//                //UpdateStatusInfo();
//            }
//        }

//        public void NewFile()
//        {
//            string fileName;
//            if (SystemDialogs.SaveFile(null, string.Empty, out fileName))
//            {
//                hyperlinkInterface.CreateNewFile(fileName);
//                recentProjectMenu.Notify(fileName);
//                //Search();
//                //UpdateStatusInfo();
//            }
//        }

//        public void SaveFileAs()
//        {
//            string fileName;
//            if (SystemDialogs.SaveFile(null, string.Empty, out fileName))
//            {
//                hyperlinkInterface.SaveFileAs(fileName);
//                recentProjectMenu.Notify(fileName);
//                //Search();
//                //UpdateStatusInfo();
//            }
//        }

//        public void AddLink()
//        {
//            //http://msdn.microsoft.com/en-us/library/aa969773(v=vs.85).aspx
//            EditHyperlink editHyperlink = new EditHyperlink();
//            editHyperlink.Owner = this;
//            editHyperlink.CurrentHyperlink = new HyperlinkItem();
//            editHyperlink.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

//            bool? result = editHyperlink.ShowDialog();
//            if (result.HasValue && result.Value)
//            {
//                hyperlinkInterface.AddHyperlink(editHyperlink.CurrentHyperlink);
//                //this.listBox.ItemsSource = hyperlinkInterface.HyperlinkList;
//                //this.listBox.SelectedItem = hyperlinkInterface.SelectedHyperlink;
//                //this.listBox.ScrollIntoView(this.listBox.SelectedItem);
//            }
//        }

//        public void EditLink()
//        {
//        }

//        public void SearchLinks()
//        {
//        }

//        private void OpenFile(string fileName)
//        {
//            if (hyperlinkInterface.OpenFile(fileName))
//            {
//                recentProjectMenu.Notify(fileName);
//                //Search();
//                //UpdateStatusInfo();
//            }
//            else
//            {
//                MessageBox.Show(null, "Hyperlink Manager could not find this file at the specified path.", 
//                    ConfigSettings.ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Error);
//                recentProjectMenu.Remove(fileName);
//            }
//        }
//    }
//}
