using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using taide.Actions;
using taide.Helpers;
using taide.Models;

namespace taide.Actions {
    public class VariableControl {
        private NewVariable newVarInfo;
        private VarLists varLists;
        
        private FileControl fileControl;

        public VariableControl (string currentDirectory) {
            fileControl = new FileControl (currentDirectory);
        }

        public void UpsertVariable (NewVariable newVariableInfo, VarLists variableLists) {
            newVarInfo = newVariableInfo;
            varLists = variableLists;
            var ad = new AggregateData ();

            if (newVariableInfo.isEnvVar) {
                bool matchFound = false;
                ad.GlobalData = CreateVariableFileContents (varLists.GlobalList, newVarInfo.GlobalValue, out matchFound, true);
                if (matchFound) throw new Exception ("Variable meant for environment variables already exists in global variables.");
                ad.DevData = CreateVariableFileContents (varLists.DevList, newVarInfo.DevValue, out matchFound);
                ad.QaData = CreateVariableFileContents (varLists.QaList, newVarInfo.QaValue, out matchFound);
                ad.QtsData = CreateVariableFileContents (varLists.QtsList, newVarInfo.QtsValue, out matchFound);
                ad.ProdData = CreateVariableFileContents (varLists.ProdList, newVarInfo.ProdValue, out matchFound);

                // Convert lists to strings
                ad.GlobalText = CreateVariableText(ad.GlobalData);
                ad.DevText = CreateVariableText(ad.DevData);
                ad.QaText = CreateVariableText(ad.QaData);
                ad.QtsText = CreateVariableText(ad.QtsData);
                ad.ProdText = CreateVariableText(ad.ProdData);

                // Generate VARS files
                ad.VariableNames = MergeAllListData(ad);
                ad.VariableText = CreateVarsFileData(ad.VariableNames);

                fileControl.SaveFileWithContent (@"dev.tfvars", ad.DevText);
                fileControl.SaveFileWithContent (@"qa.tfvars", ad.QaText);
                fileControl.SaveFileWithContent (@"qts.tfvars", ad.QtsText);
                fileControl.SaveFileWithContent (@"prod.tfvars", ad.ProdText);
                fileControl.SaveFileWithContent (@"dev\vars.tf", ad.VariableText);
                fileControl.SaveFileWithContent (@"qa\vars.tf", ad.VariableText);
                fileControl.SaveFileWithContent (@"qts\vars.tf", ad.VariableText);
                fileControl.SaveFileWithContent (@"prod\vars.tf", ad.VariableText);
            } else {
                bool matchFound = false;
                ad.DevData = CreateVariableFileContents (varLists.DevList, newVarInfo.DevValue, out matchFound, true);
                ad.QaData = CreateVariableFileContents (varLists.QaList, newVarInfo.QaValue, out matchFound, true);
                ad.QtsData = CreateVariableFileContents (varLists.QtsList, newVarInfo.QtsValue, out matchFound, true);
                ad.ProdData = CreateVariableFileContents (varLists.ProdList, newVarInfo.ProdValue, out matchFound, true);
                if (matchFound) throw new Exception ("Variable meant for global variables already exists in environment variables.");
                ad.GlobalData = CreateVariableFileContents (varLists.GlobalList, newVarInfo.GlobalValue, out matchFound);

                // Convert lists to strings
                ad.GlobalText = CreateVariableText(ad.GlobalData);
                // Generate VARS files
                ad.VariableNames = MergeAllListData(ad);
                ad.VariableText = CreateVarsFileData(ad.VariableNames);

                if (string.IsNullOrWhiteSpace(ad.GlobalText) || string.IsNullOrWhiteSpace(ad.VariableText)) throw new Exception("Something went horribly wrong! No files saved.");
                fileControl.SaveFileWithContent (@"global.tfvars", ad.GlobalText);
                fileControl.SaveFileWithContent (@"dev\vars.tf", ad.VariableText);
                fileControl.SaveFileWithContent (@"qa\vars.tf", ad.VariableText);
                fileControl.SaveFileWithContent (@"qts\vars.tf", ad.VariableText);
                fileControl.SaveFileWithContent (@"prod\vars.tf", ad.VariableText);
            }
        }

        private string CreateVarsFileData(List<string> data) {
            var sb = new StringBuilder();
            foreach (var item in data)
            {
                sb.AppendLine("variable \"environment\" {");
                if (newVarInfo.varType == VarTypes.IntegerType) {
                    sb.AppendLine("type = \"integer\"");
                }
                else if (newVarInfo.varType == VarTypes.StringType) {
                    sb.AppendLine("type = \"string\"");
                }
                sb.AppendLine("}").AppendLine();
            }
            return sb.ToString();
        }

        private List<string> MergeAllListData(AggregateData ad){
            List<string> devAndQa = ad.DevData.Union(ad.QaData).ToList();
            List<string> qtsAndProd = ad.QtsData.Union(ad.ProdData).ToList();
            List<string> environments = devAndQa.Union(qtsAndProd).ToList();
            List<string> fullVarsList = ad.GlobalData.Union(environments).ToList();
            return fullVarsList;
        }

        private string CreateVariableText (List<string> data) {
            var sb = new StringBuilder ();
            foreach (var item in data) {
                sb.AppendLine (item);
            }
            return sb.ToString();
        }

        private List<string> CreateVariableFileContents (List<string> theList, string theValue, out bool matchFound, bool skipAdd = false) {
            Regex fieldRegex = new Regex (".*(?= =)|.*(?==)");
            Regex integerRegex = new Regex ("(?<=)[0-9]*");
            Regex stringRegex = new Regex ("(?<=\").*(?=\")");
            bool isMatch = false;
            var aggregate = new List<string> ();
            foreach (var item in theList) {
                string line = item;
                Match fieldMatch = fieldRegex.Match (line);
                string field = fieldMatch.Success ? fieldMatch.Value.ToLower ().Trim () : "";

                if (field == newVarInfo.varName) {
                    if (!skipAdd && newVarInfo.varType == VarTypes.StringType) { stringRegex.Replace (line, theValue); }
                    if (!skipAdd && newVarInfo.varType == VarTypes.IntegerType) { integerRegex.Replace (line, theValue); }
                    isMatch = true;
                }
                aggregate.Add (line);
            }
            if (!isMatch) { 
                if (!skipAdd && newVarInfo.varType == VarTypes.StringType) { aggregate.Add ($"{newVarInfo.varName} = \"{theValue}\""); }
                if (!skipAdd && newVarInfo.varType == VarTypes.IntegerType) { aggregate.Add ($"{newVarInfo.varName} = {theValue}"); }
            }
            matchFound = isMatch;
            return aggregate;
        }

    }
}