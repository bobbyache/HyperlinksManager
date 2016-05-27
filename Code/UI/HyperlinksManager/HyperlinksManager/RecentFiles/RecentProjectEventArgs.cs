using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HyperlinksManager.RecentFiles
{
    public sealed class RecentProjectEventArgs : EventArgs
    {
        private RecentFile recentFile = null;
        public RecentFile RecentFile
        {
            get { return this.recentFile; }
        }

        public RecentProjectEventArgs(RecentFile recentFile)
        {
            this.recentFile = recentFile;
        }
    }
}
