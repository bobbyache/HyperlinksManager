using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace XmlHyperlinks_Test
{
    public class HyperlinksRepository
    {
        private string xmlFile = "";
        private const string HLINK_ATTR_TOPIC = "Topic";
        private const string HLINK_ATTR_URL = "Url";
        private const string HLINK_ATTR_DESC = "Description";
        private const string HLINK_ATTR_STATE = "State";
        private const string HLINK_ELEMENT_PHRASES = "KeyPhraseList";
        private const string HLINK_ATTR_ID = "ID";
        private const string HYPERLINKS = "Hyperlinks";
        private const string HYPERLINK = "Hyperlink";
        private const string KEYPHRASES = "KeyPhrases";
        private const string KEYPHRASE = "KeyPhrase";
        private const string REPOSITORY_ROOT = "OfflineSource";

        private string itemElementName;
        private string itemCollectionElementName;

        public static void CreateHyperlinkRepository(string xmlFile)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement root = xmlDocument.CreateElement(REPOSITORY_ROOT);
            xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.DocumentElement);
            xmlDocument.AppendChild(root);
            root.AppendChild(xmlDocument.CreateElement(HYPERLINKS));
            root.AppendChild(xmlDocument.CreateElement(KEYPHRASES));
            xmlDocument.Save(xmlFile);
        }

        public static void SaveProjectAs(string sourcePath, string destinationFilePath)
        {
            File.Copy(sourcePath, destinationFilePath, true);
        }

        public HyperlinksRepository(string xmlFile)
        {
            this.xmlFile = xmlFile;
            this.itemElementName = HYPERLINK;
            this.itemCollectionElementName = HYPERLINKS;
        }

        public HyperlinksRepository(string xmlFile, string itemCollectionElementName, string itemElementName)
        {
            this.xmlFile = xmlFile;
            this.itemElementName = itemElementName;
            this.itemCollectionElementName = itemCollectionElementName;
        }

        public string LoadKeyPhrases()
        {
            XElement offline = XElement.Load(xmlFile);
            XElement hyperlinkElement =  offline.Element(KEYPHRASES);

            return ((string)hyperlinkElement.Value).Trim();
        }

        public void SaveKeyPhrases(string keyPhrases)
        {
            XElement offline = XElement.Load(xmlFile);
            XElement hyperlinkElement = offline.Element(KEYPHRASES);
            hyperlinkElement.Value = keyPhrases;
            offline.Save(xmlFile);
        }

        public List<HyperlinkItem> FindHyperlinks(string topicSegment)
        {
            XElement offline = XElement.Load(xmlFile);

            IEnumerable<HyperlinkItem> hyperlinks = from h in offline.Elements(itemCollectionElementName).Elements(itemElementName)
                                                where 
                                                    ((string)h.Attribute(HLINK_ATTR_TOPIC)).ToUpper()
                                                    .Contains(topicSegment.ToUpper())
                                                    select new HyperlinkItem((string)h.Attribute(HLINK_ATTR_ID), (string)h.Element(HLINK_ELEMENT_PHRASES).Value)
                                                {
                                                    Topic = (string)h.Attribute(HLINK_ATTR_TOPIC),
                                                    Url = (string)h.Attribute(HLINK_ATTR_URL),
                                                    State = stateFromString((string)h.Attribute(HLINK_ATTR_STATE)),
                                                    Description = (string)h.Element(HLINK_ATTR_DESC)
                                                };

            return hyperlinks.ToList<HyperlinkItem>();
        }

        public List<HyperlinkItem> LoadAllHyperlinks()
        {
            XElement offline = XElement.Load(xmlFile);

            IEnumerable<HyperlinkItem> hyperLinks = from h in offline.Element(itemCollectionElementName).Elements(itemElementName)
                                                    select new HyperlinkItem((string)h.Attribute(HLINK_ATTR_ID), (string)h.Element(HLINK_ELEMENT_PHRASES).Value)
                                                {
                                                    Topic = (string)h.Attribute(HLINK_ATTR_TOPIC),
                                                    Url = (string)h.Attribute(HLINK_ATTR_URL),
                                                    State = stateFromString((string)h.Attribute(HLINK_ATTR_STATE)),
                                                    Description = (string)h.Element(HLINK_ATTR_DESC)
                                                };
            return hyperLinks.ToList<HyperlinkItem>();
        }

        public HyperlinkItem GetHyperlink(string id)
        {
            XElement offline = XElement.Load(xmlFile);

            XElement hyperlinkElement =  offline.Element(itemCollectionElementName).Elements(itemElementName)
                .Where(xh => (string)xh.Attribute(HLINK_ATTR_ID).Value == id).FirstOrDefault();

            HyperlinkItem hyperlink = new HyperlinkItem((string)hyperlinkElement.Attribute(HLINK_ATTR_ID), (string)hyperlinkElement.Element(HLINK_ELEMENT_PHRASES).Value)
            {
                Topic = (string)hyperlinkElement.Attribute(HLINK_ATTR_TOPIC),
                Url = (string)hyperlinkElement.Attribute(HLINK_ATTR_URL),
                State = stateFromString((string)hyperlinkElement.Attribute(HLINK_ATTR_STATE)),
                Description = (string)hyperlinkElement.Element(HLINK_ATTR_DESC)
            };

            return hyperlink;
        }

        public void RemoveHyperlink(HyperlinkItem hyperlink)
        {
            XElement offline = XElement.Load(xmlFile);

            if (hyperlinkExists(hyperlink.Id, offline))
            {
                offline.Element(itemCollectionElementName).Elements(itemElementName)
                    .Where(hyp =>
                        (string)hyp.Attribute(HLINK_ATTR_ID).Value == hyperlink.Id)
                        .Remove();
                offline.Save(xmlFile);
            }
        }

        public void SaveHyperlink(HyperlinkItem hyperlink)
        {
            XElement offline = XElement.Load(xmlFile);


            if (hyperlinkExists(hyperlink.Id, offline))
            {
                // update the hyperlink.
                XElement hyperlinkElement = offline.Element(itemCollectionElementName).Elements(itemElementName)
                    .Where(xh => (string)xh.Attribute(HLINK_ATTR_ID).Value == hyperlink.Id).FirstOrDefault();

                hyperlinkElement.Attribute(HLINK_ATTR_TOPIC).Value = hyperlink.Topic;
                hyperlinkElement.Attribute(HLINK_ATTR_URL).Value = hyperlink.Url;
                hyperlinkElement.Element(HLINK_ELEMENT_PHRASES).Value = hyperlink.DelimitedKeyPhraseList;
                hyperlinkElement.Element(HLINK_ATTR_DESC).ReplaceNodes(new XCData(hyperlink.Description));
            }
            else
            {
                // create a new hyperlink
                XElement xHyperlink = new XElement(itemElementName);

                xHyperlink.Add(new XAttribute(HLINK_ATTR_ID, hyperlink.Id));
                xHyperlink.Add(new XAttribute(HLINK_ATTR_TOPIC, hyperlink.Topic));
                xHyperlink.Add(new XAttribute(HLINK_ATTR_URL, hyperlink.Url));
                xHyperlink.Add(new XAttribute(HLINK_ATTR_STATE, hyperlink.State.ToString()));
                xHyperlink.Add(new XElement(HLINK_ELEMENT_PHRASES, hyperlink.DelimitedKeyPhraseList));
                xHyperlink.Add(new XElement(HLINK_ATTR_DESC, new XCData(hyperlink.Description)));

                offline.Element(itemCollectionElementName).Add(xHyperlink);
            }

            offline.Save(xmlFile);
        }

        private HyperlinkStateEnum stateFromString(string state)
        {
            return HyperlinkStateEnum.New;
        }

        private bool hyperlinkExists(string id, XElement offline)
        {
            //XElement offline = XElement.Load(xmlFile);

            int count = offline.Element(itemCollectionElementName).Elements(itemElementName)
                .Where(xh => (string)xh.Attribute(HLINK_ATTR_ID).Value == id).Count();

            return (count == 1);
        }
    }
}
