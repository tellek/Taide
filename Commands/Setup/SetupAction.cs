using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using taide.Helpers;
using taide.Models;

namespace taide.Commands.Setup {
    public class SetupAction {
        private string currentDirectory;
        public SetupAction() {
            Cli.PrintLine ("Setup Initiated.");
            currentDirectory = Directory.GetCurrentDirectory ();
        }
        public void Execute() {
            string[] segments = currentDirectory.Split('\\');
            string folder = segments[segments.Length - 1];
            if (folder.ToLower() != "taide") {
                Cli.PrintLine("Error: You must be in a Terraform project folder root when executing this command.",
                ConsoleColor.Red);
                return;
            }

            Cli.PrintLine("Please enter the following information.");
            Cli.PrintLine("Team Name", ConsoleColor.Yellow);
            string teamName = Console.ReadLine().ToLower();
            Cli.PrintLine("AWS Region", ConsoleColor.Yellow);
            Cli.PrintLine("example: us-east-1");
            string regionValue = Console.ReadLine().ToLower();
            Cli.PrintLine("Your Name", ConsoleColor.Yellow);
            string launchedBy = Console.ReadLine().ToLower();
            Cli.PrintLine("S3 Team Package Bucket Name", ConsoleColor.Yellow);
            Cli.PrintLine("If this has previously been setup enter the correct value, " + 
                "otherwise use something like: vin-TEAM-packages");
            string packageBucket = Console.ReadLine();
            Cli.PrintLine("The Full Project Name", ConsoleColor.Yellow);
            Cli.PrintLine("example: vin-cloudstore-bs-api " + 
                "(note: aws can be picky about what special characters are allowed and where.)");
            string projectName = Console.ReadLine().ToLower();
            Cli.PrintLine("A Shortened Version of the Project Name", ConsoleColor.Yellow);
            Cli.PrintLine("example: vin-cloudstore " + 
                "(note: aws can be picky about what special characters are allowed and where.)");
            string projectAbbreviation = Console.ReadLine().ToLower();

            var globals = new Globals{
                team = teamName,
                region = regionValue,
                launched_by = launchedBy,
                s3_package_bucket = packageBucket,
                projectName = projectName,
                projectAbbreviation = projectAbbreviation
            };

            var collectData = new CollectData();

            var globalList = collectData.MakeVarList($"{currentDirectory}\\terraform\\global.tfvars");
            var devList = collectData.MakeVarList($"{currentDirectory}\\terraform\\dev.tfvars");
            var qaList = collectData.MakeVarList($"{currentDirectory}\\terraform\\qa.tfvars");
            var qtsList = collectData.MakeVarList($"{currentDirectory}\\terraform\\qts.tfvars");
            var prodList = collectData.MakeVarList($"{currentDirectory}\\terraform\\prod.tfvars");















            var vl = new List<string>{
                $"team = \"{teamName}\"",
                $"region = \"{regionValue}\"",
                $"launched_by = \"{launchedBy}\"",
                $"s3_package_bucket = \"{packageBucket}\"",
                $"projectName = \"{projectName}\"",
                $"projectAbbreviation = \"{projectAbbreviation}\""
            };

            
            StringBuilder globalSb = new StringBuilder();
            StringBuilder varsSb = new StringBuilder();

            Regex rgx = new Regex("(?<=\").*(?=\")");
            foreach (string line in File.ReadLines($"{currentDirectory}\\terraform\\global.tfvars")) {
                switch (line.Split(' ')[0].Trim()) {
                    case "team":
                        vl.RemoveAll(x => x.Contains("team ="));
                        vl.Add(rgx.Replace(line, teamName));
                        break;
                    case "region":
                        vl.RemoveAll(x => x.Contains("region ="));
                        vl.Add(rgx.Replace(line, regionValue));
                        break;
                    case "launched_by":
                        vl.RemoveAll(x => x.Contains("launched_by ="));
                        vl.Add(rgx.Replace(line, launchedBy));
                        break;
                    case "s3_package_bucket":
                        vl.RemoveAll(x => x.Contains("s3_package_bucket ="));
                        vl.Add(rgx.Replace(line, packageBucket));
                        break;
                    case "projectName":
                        vl.RemoveAll(x => x.Contains("projectName ="));
                        vl.Add(rgx.Replace(line, projectName));
                        break;
                    case "projectAbbreviation":
                        vl.RemoveAll(x => x.Contains("projectAbbreviation ="));
                        vl.Add(rgx.Replace(line, projectAbbreviation));
                        break;
                    default:
                        if (!string.IsNullOrWhiteSpace(line)) vl.Add(line);
                        break;
                }
            }

            foreach (var item in vl) {
                globalSb.AppendLine(item).AppendLine();
                string varName = "";
                varsSb.AppendLine("variable \"" + varName + "\" {");
                varsSb.AppendLine("  type = \"string\"");
                varsSb.AppendLine("}").AppendLine();;
            }
            
            File.WriteAllText($"{currentDirectory}\\terraform\\global.tfvars", globalSb.ToString());
        }
    }
}
