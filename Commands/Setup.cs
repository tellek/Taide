using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using taide.Helpers;
using taide.Models;
using taide.Actions;

namespace taide.Commands {
    public class SetupAction {
        private string currentDirectory;
        private CollectData collectData;

        public SetupAction () {
            Cli.PrintLine ("Setup Initiated.");
            currentDirectory = @"F:\CodeProjects\Taide\terraform";
            //currentDirectory = Directory.GetCurrentDirectory ();
            collectData = new CollectData();
        }
        public void Execute () {
            string[] segments = currentDirectory.Split ('\\');
            string folder = segments[segments.Length - 1];
            if (folder.ToLower () != "taide") {
                Cli.PrintLine ("Error: You must be in a Terraform project folder root when executing this command.",
                    ConsoleColor.Red);
                return;
            }

            Cli.PrintLine ("Please enter the following information.");
            Cli.PrintLine ("Team Name", ConsoleColor.Yellow);
            string teamName = Console.ReadLine ().ToLower ();
            Cli.PrintLine ("AWS Region", ConsoleColor.Yellow);
            Cli.PrintLine ("example: us-east-1");
            string regionValue = Console.ReadLine ().ToLower ();
            Cli.PrintLine ("Your Name", ConsoleColor.Yellow);
            string launchedBy = Console.ReadLine ().ToLower ();
            Cli.PrintLine ("S3 Team Package Bucket Name", ConsoleColor.Yellow);
            Cli.PrintLine ("If this has previously been setup enter the correct value, " +
                "otherwise use something like: vin-TEAM-packages");
            string packageBucket = Console.ReadLine ();
            Cli.PrintLine ("The Full Project Name", ConsoleColor.Yellow);
            Cli.PrintLine ("example: vin-cloudstore-bs-api " +
                "(note: aws can be picky about what special characters are allowed and where.)");
            string projectName = Console.ReadLine ().ToLower ();
            Cli.PrintLine ("A Shortened Version of the Project Name", ConsoleColor.Yellow);
            Cli.PrintLine ("example: vin-cloudstore " +
                "(note: aws can be picky about what special characters are allowed and where.)");
            string projectAbbreviation = Console.ReadLine ().ToLower ();

            var vl = new List<string> {
                $"team = \"{teamName}\"",
                $"region = \"{regionValue}\"",
                $"launched_by = \"{launchedBy}\"",
                $"s3_package_bucket = \"{packageBucket}\"",
                $"projectName = \"{projectName}\"",
                $"projectAbbreviation = \"{projectAbbreviation}\""
            };

            var varLists = collectData.PopulateVarLists(currentDirectory);

            StringBuilder globalSb = new StringBuilder ();
            StringBuilder devSb = new StringBuilder ();
            StringBuilder qaSb = new StringBuilder ();
            StringBuilder qtsSb = new StringBuilder ();
            StringBuilder prodSb = new StringBuilder ();

            Regex rgx = new Regex ("(?<=\").*(?=\")");
            foreach (string line in varLists.GlobalList) {
                switch (line.Split (' ') [0].Trim ()) {
                    case "team":
                        vl.RemoveAll (x => x.Split (' ') [0].Trim () == "team");
                        vl.Add (rgx.Replace (line, teamName));
                        break;
                    case "region":
                        vl.RemoveAll (x => x.Split (' ') [0].Trim () == "region");
                        vl.Add (rgx.Replace (line, regionValue));
                        break;
                    case "launched_by":
                        vl.RemoveAll (x => x.Split (' ') [0].Trim () == "launched_by");
                        vl.Add (rgx.Replace (line, launchedBy));
                        break;
                    case "s3_package_bucket":
                        vl.RemoveAll (x => x.Split (' ') [0].Trim () == "s3_package_bucket");
                        vl.Add (rgx.Replace (line, packageBucket));
                        break;
                    case "projectName":
                        vl.RemoveAll (x => x.Split (' ') [0].Trim () == "projectName");
                        vl.Add (rgx.Replace (line, projectName));
                        break;
                    case "projectAbbreviation":
                        vl.RemoveAll (x => x.Split (' ') [0].Trim () == "projectAbbreviation");
                        vl.Add (rgx.Replace (line, projectAbbreviation));
                        break;
                    default:
                        if (!string.IsNullOrWhiteSpace (line)) vl.Add (line);
                        break;
                }
            }

            foreach (var item in vl) {
                globalSb.AppendLine (item).AppendLine ();
                //string varName = "";
                // varsSb.AppendLine ("variable \"" + varName + "\" {");
                // varsSb.AppendLine ("  type = \"string\"");
                // varsSb.AppendLine ("}").AppendLine ();;
            }

            File.WriteAllText ($"{currentDirectory}\\terraform\\global.tfvars", globalSb.ToString ());
        }
    }
}