using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XmlHyperlinks_Test;
using System.Collections.ObjectModel;
using System.IO;

namespace HyperlinksManager
{
    public class HyperlinkInterface
    {
        private Keyphrases keyPhraseLibrary = null;
        private Keyphrases keyPhraseFilter = null;
        private HyperlinksRepository repository = null;
        private List<HyperlinkItem> hyperlinkCollection = null;

        private string file = string.Empty;

        public bool FileLoaded
        {
            get
            {
                return this.CurrentFile != string.Empty;
            }
        }

        public string CurrentFileTitle
        {
            get
            {
                return Path.GetFileName(this.CurrentFile);
            }
        }
        public string CurrentFile
        {
            get { return this.file; }
        }

        public string KeywordFilter
        {
            get 
            {
                if (this.keyPhraseFilter != null)
                    return this.keyPhraseFilter.DelimitKeyPhraseList();
                else
                    return string.Empty;
            }
            set { this.keyPhraseFilter = new Keyphrases(value); }
        }

        public string TopicFilter
        {
            get;
            set;
        }

        private HyperlinkItem selectedHyperlink;
        public HyperlinkItem SelectedHyperlink
        {
            get
            {
                if (selectedHyperlink == null && HyperlinkList.Count > 0)
                    selectedHyperlink = HyperlinkList[0];
                return selectedHyperlink;
            }
            set
            {
                if (this.HyperlinkList.Contains(value))
                    selectedHyperlink = value;
                else
                {
                    if (HyperlinkList.Count > 0)
                        selectedHyperlink = HyperlinkList[0];
                    else
                        selectedHyperlink = null;
                }
            }
        }

        public ObservableCollection<HyperlinkItem> HyperlinkList
        {
            get
            {
                return RetrieveHyperlinks();
            }
        }

        public bool OpenFile(string file)
        {
            if (File.Exists(file))
            {
                this.file = file;
                this.keyPhraseFilter = null;
                this.keyPhraseLibrary = null;
                this.repository = null;
                this.hyperlinkCollection = null;

                return true;
            }
            return false;
        }

        public void CreateNewFile(string file)
        {
            HyperlinksRepository.CreateHyperlinkRepository(file);
            OpenFile(file);
        }

        public void SaveFileAs(string file)
        {
            HyperlinksRepository.SaveProjectAs(this.CurrentFile, file);
            OpenFile(file);
        }

        public bool AddHyperlink(HyperlinkItem hyperlink)
        {
            if (string.IsNullOrEmpty(hyperlink.Topic) || string.IsNullOrEmpty(hyperlink.Url) || string.IsNullOrEmpty(hyperlink.DelimitedKeyPhraseList))
                return false;
            else
            {
                EnsureState();
                keyPhraseLibrary.AddKeyPhrases(hyperlink.DelimitedKeyPhraseList);
                repository.SaveKeyPhrases(keyPhraseLibrary.DelimitKeyPhraseList());
                repository.SaveHyperlink(hyperlink);
                RefreshHyperlinks();
                this.selectedHyperlink = this.HyperlinkList.Where(r => r.Id == hyperlink.Id).SingleOrDefault();
                return true;
            }
        }

        public bool UpdateHyperlink(HyperlinkItem hyperlink)
        {
            if (string.IsNullOrEmpty(hyperlink.Topic) || string.IsNullOrEmpty(hyperlink.Url) || string.IsNullOrEmpty(hyperlink.DelimitedKeyPhraseList))
                return false;
            else
            {
                EnsureState();
                keyPhraseLibrary.AddKeyPhrases(hyperlink.DelimitedKeyPhraseList);
                repository.SaveKeyPhrases(keyPhraseLibrary.DelimitKeyPhraseList());
                repository.SaveHyperlink(hyperlink);
                RefreshHyperlinks();
                this.selectedHyperlink = this.HyperlinkList.Where(r => r.Id == hyperlink.Id).SingleOrDefault();
                return true;
            }
        }

        public bool AddHyperlink(string topic, string url, string description, string phraseList) 
        {
            HyperlinkItem item = new HyperlinkItem(phraseList);

            item.Description = description;
            item.Topic = topic;
            item.Url = url;

            return AddHyperlink(item);
        }

        public void RemoveHyperlink() {}

        public ObservableCollection<HyperlinkItem> RetrieveHyperlinks()
        {
            EnsureState();
            List<HyperlinkItem> hyps;

            if (keyPhraseFilter.Exist)
            {
                hyps = (from h in hyperlinkCollection
                        where ContainsPhrase(h.KeyPhrases)
                        select h).ToList();
            }
            else
                hyps = hyperlinkCollection;

            return new ObservableCollection<HyperlinkItem>(hyps);
        }

        private bool ContainsPhrase(string[] keywordCollection)
        {     
            Keyphrases hyperlinkKeywords = new Keyphrases(keywordCollection.ToList());

            if (hyperlinkKeywords.Exist)
            {
                foreach (string keyword in keyPhraseFilter.KeyPhrases)
                {
                    bool result = (from k in hyperlinkKeywords.KeyPhrases
                                   where k.ToUpper().Contains(keyword.ToUpper()) // && keyword != string.Empty
                                   select keyword.Distinct()).Count() > 0;

                    if (!result) // if at any point result is false, return false...
                        return false;
                }
                return true;
            }
            else
                return false;
        }

        private void RefreshHyperlinks()
        {
            if (repository == null)
            {
                repository = new HyperlinksRepository((this.file));
                hyperlinkCollection = new List<HyperlinkItem>(repository.LoadAllHyperlinks());
            }
            else
                hyperlinkCollection = new List<HyperlinkItem>(repository.LoadAllHyperlinks());

            this.keyPhraseLibrary = new Keyphrases(repository.LoadKeyPhrases());
        }

        private void EnsureState()
        {
            
            if (repository == null)
            {
                repository = new HyperlinksRepository((this.file));
                hyperlinkCollection = new List<HyperlinkItem>(repository.LoadAllHyperlinks());
            }
            if (this.keyPhraseFilter == null)
            {
                this.keyPhraseFilter = new Keyphrases(string.Empty);
            }
            if (this.keyPhraseLibrary == null)
            {
                this.keyPhraseLibrary = new Keyphrases(repository.LoadKeyPhrases());
            }
        }
    }
}
