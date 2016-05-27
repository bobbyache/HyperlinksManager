using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlHyperlinks_Test
{
    public enum HyperlinkStateEnum
    {
        New,
        Source,
        Modified
    }

    public class HyperlinkItem
    {
        private string id;
        private Keyphrases keyPhrases;

        public string Topic { get; set; }
        public string Url { get; set; }

        private string description = string.Empty;
        public string Description 
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public string DomainServer 
        { 
            get 
            {
                Uri uri = new Uri(this.Url);
                return uri.Host;
            } 
        }
        public HyperlinkStateEnum State { get; set; }

        public string Id
        {
            get { return this.id; }
        }

        public HyperlinkItem(string id, string keyPhrases)
        {
            this.id = id;
            this.keyPhrases = new Keyphrases(keyPhrases);
        }

        public HyperlinkItem(string keyPhrases)
        {
            this.id = Guid.NewGuid().ToString();
            this.keyPhrases = new Keyphrases(keyPhrases);
        }

        public HyperlinkItem()
        {
            this.id = Guid.NewGuid().ToString();
            this.keyPhrases = new Keyphrases(string.Empty);
        }

        public string[] KeyPhrases 
        {
            get { return keyPhrases.KeyPhrases; } 
        }

        public bool KeyPhraseExists(string phrase)
        {
            return this.keyPhrases.PhraseExists(phrase);
        }

        public string DelimitedKeyPhraseList
        {
            get { return this.keyPhrases.DelimitKeyPhraseList(); }
        }

        public void AddKeyPhrases(string delimitedKeyPhraseText)
        {
            XmlHyperlinks_Test.Keyphrases newKeyPhrases = new Keyphrases(delimitedKeyPhraseText);
            keyPhrases.AddKeyPhrases(newKeyPhrases.KeyPhrases);
        }

        public void AddKeyPhrases(string[] keyPhraseList)
        {
            keyPhrases.AddKeyPhrases(keyPhraseList);
        }

        public void RemovePhrases(string[] keyPhraseList)
        {
            keyPhrases.RemovePhrases(keyPhraseList);
        }
    }
}
