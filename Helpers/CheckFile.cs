using System;

namespace taide.Helpers {
    public static class CheckFile {
        public static bool CurrentFolderIsEqualTo (string folderName, string currentDirectory) {
            string[] segments = currentDirectory.Split ('\\');
            string folder = segments[segments.Length - 1];
            if (folder.ToLower ().Trim() == folderName.ToLower ().Trim ()) return true;
            return false;
        }
    }

}