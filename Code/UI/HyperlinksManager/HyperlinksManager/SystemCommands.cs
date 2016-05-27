using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace HyperlinksManager
{
    public class SystemCommands
    {
        private static RoutedUICommand editLinkCommand;
        private static RoutedUICommand addLinkCommand;
        private static RoutedUICommand searchCommand;

        static SystemCommands()
        {
            //InputGestureCollection editLinkInputs = new InputGestureCollection();
            //editLinkInputs.Add(new KeyGesture(Key.R, ModifierKeys.Control, "Edit Link"));
            //editLinkCommand = new RoutedUICommand("Edit Link", "EditLink", typeof(SystemCommands), editLinkInputs);

            editLinkCommand = new RoutedUICommand("Edit Link", "EditLink", typeof(SystemCommands));
            searchCommand = new RoutedUICommand("Search", "Search", typeof(SystemCommands));
            addLinkCommand = new RoutedUICommand("Add Link", "AddLink", typeof(SystemCommands));
        }

        public static RoutedUICommand EditLink
        {
            get { return editLinkCommand; }
        }

        public static RoutedUICommand Search
        {
            get { return searchCommand; }
        }

        public static RoutedUICommand AddLink
        {
            get { return addLinkCommand; }
        }
    }
}
