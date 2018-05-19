using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using taide.Helpers;

namespace taide.Commands {
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

            var vl = new List<string>{
                $"team = \"{teamName}\"",
                $"region = \"{regionValue}\"",
                $"launched_by = \"{launchedBy}\"",
                $"s3_package_bucket = \"{packageBucket}\"",
                $"projectName = \"{projectName}\"",
                $"projectAbbreviation = \"{projectAbbreviation}\""
            };

            
            StringBuilder sb = new StringBuilder();
            Regex rgx = new Regex("(?<=\").*(?=\")");
            foreach (string line in File.ReadLines($"{currentDirectory}\\terraform\\global.tfvars")) {
                switch (line.Split(' ')[0].Trim()) {
                    case "team":
                        sb.AppendLine(rgx.Replace(line, teamName));
                        vl.RemoveAll(x => x.Contains("team ="));
                        break;
                    case "region":
                        sb.AppendLine(rgx.Replace(line, regionValue));
                        vl.RemoveAll(x => x.Contains("region ="));
                        break;
                    case "launched_by":
                        sb.AppendLine(rgx.Replace(line, launchedBy));
                        vl.RemoveAll(x => x.Contains("launched_by ="));
                        break;
                    case "s3_package_bucket":
                        sb.AppendLine(rgx.Replace(line, packageBucket));
                        vl.RemoveAll(x => x.Contains("s3_package_bucket ="));
                        break;
                    case "projectName":
                        sb.AppendLine(rgx.Replace(line, projectName));
                        vl.RemoveAll(x => x.Contains("projectName ="));
                        break;
                    case "projectAbbreviation":
                        sb.AppendLine(rgx.Replace(line, projectAbbreviation));
                        vl.RemoveAll(x => x.Contains("projectAbbreviation ="));
                        break;
                    default:
                        sb.AppendLine(line);
                        break;
                }
            }

            foreach (var item in vl) {
                sb.AppendLine(item);
            }
            
            File.WriteAllText($"{currentDirectory}\\terraform\\global.tfvars", sb.ToString());
        }
    }
}