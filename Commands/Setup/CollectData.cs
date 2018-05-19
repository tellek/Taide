using System;
using System.Collections.Generic;
using System.IO;

namespace taide.Commands.Setup {
    public class CollectData {
        public List<string> MakeVarList(string file){
            var theList = new List<string>();
            foreach (string line in File.ReadLines(file)) {
                if (!string.IsNullOrWhiteSpace(line)) theList.Add(line);
            }

            return theList;
        }
    }
}