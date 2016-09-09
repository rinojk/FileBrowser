using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FileBrowser.Infrastructure
{
    public class Answer
    {
        private IEnumerable<string> _files;
        private IEnumerable<string> _folders;
        private string _currentFolder;
        private string _parentFolder;

        public IEnumerable<string> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        public IEnumerable<string> Folders
        {
            get { return _folders; }
            set { _folders = value; }
        }

        public string CurrentFolder
        {
            get { return _currentFolder; }
            set { _currentFolder = value; }
        }

        public string ParentFolder
        {
            get { return _parentFolder; }
            set { _parentFolder = value; }
        }
    }
}