using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlHyperlinks_Test
{
    public class Keyphrases
    {
        private List<string> keyPhrases = new List<string>();

        public Keyphrases(string delimitedKeyPhrases)
        {
            this.keyPhrases = splitKeyPhrases(delimitedKeyPhrases);
        }

        public Keyphrases(List<string> keyPhrases)
        {
            this.keyPhrases = keyPhrases;
        }

        public bool PhraseExists(string phrase)
        {
            if (keyPhrases.Contains(phrase.Trim().ToUpper()))
                return true;
            return false;
        }

        public bool Exist
        {
            get
            {
                if (this.keyPhrases.Count == 0)
                    return false;
                else
                {
                    var results = keyPhrases.Where(k => !string.IsNullOrEmpty(k));
                    return (results.Count() > 0);
                }
            }
        }

        public string[] KeyPhrases
        {
            get
            {
                return keyPhrases.Where(k => !string.IsNullOrEmpty(k)).OrderBy(k => k).ToArray();
            }
        }

        public string DelimitKeyPhraseList()
        {
            return string.Join(",", keyPhrases.OrderBy(k => k).ToArray());
        }

        public void AddKeyPhrases(string[] keyPhraseList)
        {
            foreach (string keyphrase in keyPhraseList)
            {
                string trimmed = keyphrase.Trim();
                if (!keyPhrases.Contains(trimmed.ToUpper()))
                    keyPhrases.Add(trimmed.ToUpper());
            }
        }

        public void AddKeyPhrases(string keyPhraseDelimitedList)
        {
            AddKeyPhrases(splitKeyPhrases(keyPhraseDelimitedList).ToArray());
        }

        public void RemovePhrases(string[] keyPhraseList)
        {
            foreach (string keyphrase in keyPhraseList)
            {
                if (keyPhrases.Contains(keyphrase))
                    keyPhrases.Remove(keyphrase);
            }
        }

        private List<string> splitKeyPhrases(string keyPhrases)
        {
            string[] phrases = keyPhrases.Split(new char[] { ',' });
            var ph = phrases.Select(p => p.Trim());

            return ph.OrderBy(k => k).ToList();
        }
    }
}
