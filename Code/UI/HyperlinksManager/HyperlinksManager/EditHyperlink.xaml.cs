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
using System.Windows.Shapes;
using XmlHyperlinks_Test;

namespace HyperlinksManager
{
    /// <summary>
    /// Interaction logic for EditHyperlink.xaml
    /// </summary>
    public partial class EditHyperlink : Window
    {
        public EditHyperlink()
        {
            InitializeComponent();
        }

        private HyperlinkItem currentHyperlink;
        public HyperlinkItem CurrentHyperlink
        {
            get { return this.currentHyperlink; }
            set
            {
                this.currentHyperlink = value;
                dataGrid.DataContext = value;
                RefreshKeywordOutput();
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string text = keywordTextBox.Text.Trim();
            if (text != string.Empty)
            {
                this.currentHyperlink.AddKeyPhrases(text);
                RefreshKeywordOutput();
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                this.currentHyperlink.RemovePhrases(new string[] { listBox.SelectedItem.ToString() });
                listBox.Items.Remove(listBox.SelectedItem);
                keywordListTextBlock.Text = this.currentHyperlink.DelimitedKeyPhraseList;
            }
        }

        private void RefreshKeywordOutput()
        {
            listBox.Items.Clear();
            foreach (string item in this.currentHyperlink.KeyPhrases)
            {
                if (!string.IsNullOrEmpty(item))
                    listBox.Items.Add(item);
            }
            keywordTextBox.Clear();
            keywordListTextBlock.Text = this.currentHyperlink.DelimitedKeyPhraseList;
        }
    }
}
