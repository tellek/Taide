using System;
using System.Collections.Generic;
using System.IO;
using taide.Models;

namespace taide.Actions {
    public class CollectData {

        public VarLists PopulateVarLists(string currentDirectory){
            var varLists = new VarLists{
                GlobalList = MakeVarList ($"{currentDirectory}\\global.tfvars"),
                DevList = MakeVarList ($"{currentDirectory}\\dev.tfvars"),
                QaList = MakeVarList ($"{currentDirectory}\\qa.tfvars"),
                QtsList = MakeVarList ($"{currentDirectory}\\qts.tfvars"),
                ProdList = MakeVarList ($"{currentDirectory}\\prod.tfvars")
            };
            return varLists;
        }

        private List<string> MakeVarList (string file) {
            var theList = new List<string> ();
            foreach (string line in File.ReadLines (file)) {
                if (!string.IsNullOrWhiteSpace (line)) theList.Add (line);
            }

            return theList;
        }
    }
}