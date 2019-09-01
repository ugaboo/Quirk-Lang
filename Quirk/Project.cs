using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Quirk
{
    class Project
    {
        private string projectFolder = "";

        public int TotalErrors { get; protected set; } = 0;

        public string ProjectFolder {
            get => projectFolder;
            set => projectFolder = Path.GetFullPath(value);
        }
    }
}
