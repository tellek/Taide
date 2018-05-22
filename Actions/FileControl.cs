using System;
using System.IO;

namespace taide.Actions {
    public class FileControl {
        private string currentDirectory;

        public FileControl(string dir) {
            currentDirectory = dir;
        }

        public void SaveFileWithContent(string file, string content) {
            File.WriteAllText ($"{currentDirectory}\\{file}", content);
        }
    }
}