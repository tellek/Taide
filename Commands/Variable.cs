using System;
using System.Linq;
using taide.Actions;
using taide.Helpers;
using taide.Models;
using System.Collections.Generic;

namespace taide.Commands {
    public class VariableAction {
        private string currentDirectory;
        private CollectData collectData;
        private NewVariable varInfo;

        public VariableAction () {
            Console.WriteLine ("Variable Initiated.");
            currentDirectory = @"F:\CodeProjects\Taide\terraform";
            //currentDirectory = Directory.GetCurrentDirectory ();
            collectData = new CollectData ();
            varInfo = new NewVariable();
        }

        public void Execute () {
            if (!CheckFile.CurrentFolderIsEqualTo ("terraform", currentDirectory)) {
                Cli.PrintLine ("Error: You must be in a Terraform project folder root when executing this command.", ConsoleColor.Red);
                return;
            }

            Cli.PrintLine ("Please enter the following information.");
            varInfo.varName = GetVariableName();
            varInfo.varType = GetVariableType();
            varInfo.isEnvVar = GetIsEnvVariable();
            if (varInfo.isEnvVar) {
                varInfo.DevValue = GetVariableValue("Dev"); 
                varInfo.QaValue = GetVariableValue("QA"); 
                varInfo.QtsValue = GetVariableValue("QTS"); 
                varInfo.ProdValue = GetVariableValue("Production"); 
            }
            else { varInfo.GlobalValue = GetVariableValue("Global"); }

            var varLists = collectData.PopulateVarLists (currentDirectory);
            CreateNewVariable(varInfo, varLists);
        }

        public void CreateNewVariable(NewVariable newVariableInfo, VarLists variableLists) {
            
        }

        private string GetVariableName() {
            Cli.PrintLine ("Variable Name", ConsoleColor.Yellow);
            string variableName = Console.ReadLine ().ToLower ().Trim ();
            while (string.IsNullOrWhiteSpace (variableName)) {
                Cli.PrintLine ("Error: You must enter a value, please try again.", ConsoleColor.Red);
                variableName = Console.ReadLine ().ToLower ().Trim ();
            }
            return variableName;
        }

        private VarTypes GetVariableType() {
            Cli.PrintLine ("Variable Type (string/integer)", ConsoleColor.Yellow);
            Cli.PrintLine ("Note: List and Map types are currently not supported.", ConsoleColor.DarkGray);
            string variableType = null;
            bool validInputMade = false;
            while (!validInputMade) {
                variableType = Console.ReadLine ().ToLower ().Trim ();
                switch (variableType) {
                    case "s":
                    case "str":
                    case "string":
                        validInputMade = true;
                        return VarTypes.StringType;
                    case "i":
                    case "int":
                    case "integer":
                        validInputMade = true;
                        return VarTypes.IntegerType;
                    default:
                        Cli.PrintLine ("Error: You must enter string or integer, please try again.", ConsoleColor.Red);
                        break;
                }
            }
            return VarTypes.StringType;
        }

        private bool GetIsEnvVariable() {
            Cli.PrintLine ("Is this an environment specific variable? (yes/no)", ConsoleColor.Yellow);
            bool isEnvVariable = false;
            string response = null;
            bool validInputMade = false;
            while (!validInputMade) {
                response = Console.ReadLine ().ToLower ().Trim ();
                switch (response) {
                    case "y":
                    case "yes":
                        validInputMade = true;
                        isEnvVariable = true;
                        break;
                    case "n":
                    case "no":
                        validInputMade = true;
                        isEnvVariable = false;
                        break;
                    default:
                        Cli.PrintLine ("Error: You must enter yes or no, please try again.", ConsoleColor.Red);
                        break;
                }
            }
            return isEnvVariable;
        }

        private string GetVariableValue(string env) {
            Cli.PrintLine ($"{env} {varInfo.varName} Variable Value", ConsoleColor.Yellow);
            string variableValue = Console.ReadLine ().ToLower ().Trim ();
            return variableValue;
        }

        private void LoopThroughVarListFor(List<string> env){
            foreach (string line in env) {

            }
        }
    }
}