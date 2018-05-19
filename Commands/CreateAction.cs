using System;
using System.IO;
using taide.Helpers;

namespace taide.Commands {
    public class CreateAction {
        private string currentDirectory;

        public CreateAction () {
            currentDirectory = Directory.GetCurrentDirectory ();
            Cli.PrintLine ($"Current path = {currentDirectory}");
        }

        public void Execute () {
            var dir = Directory.GetCurrentDirectory ();
            Cli.PrintLine ("Do you wish to create a new Terraform project here? (Yes/No)", ConsoleColor.Yellow);
            if (Console.ReadLine ().ToLower () != "yes") return;

            if (Directory.Exists ($"{currentDirectory}\\terraform")) {
                Cli.PrintLine ("Error: A 'terraform' folder already exists here.", ConsoleColor.Red);
                return;
            }

            Directory.CreateDirectory ($"{currentDirectory}\\terraform");
            Cli.PrintLine ("Terraform project folder created.");

            Directory.CreateDirectory ($"{currentDirectory}\\terraform\\dev");
            Directory.CreateDirectory ($"{currentDirectory}\\terraform\\qa");
            Directory.CreateDirectory ($"{currentDirectory}\\terraform\\qts");
            Directory.CreateDirectory ($"{currentDirectory}\\terraform\\prod");
            Directory.CreateDirectory ($"{currentDirectory}\\terraform\\setup");

            Cli.PrintLine ("Environment folders created.");

            File.Create ($"{currentDirectory}\\terraform\\dev\\main.tf");
            File.Create ($"{currentDirectory}\\terraform\\dev\\vars.tf");
            File.Create ($"{currentDirectory}\\terraform\\qa\\main.tf");
            File.Create ($"{currentDirectory}\\terraform\\qa\\vars.tf");
            File.Create ($"{currentDirectory}\\terraform\\qts\\main.tf");
            File.Create ($"{currentDirectory}\\terraform\\qts\\vars.tf");
            File.Create ($"{currentDirectory}\\terraform\\prod\\main.tf");
            File.Create ($"{currentDirectory}\\terraform\\prod\\vars.tf");
            File.Create ($"{currentDirectory}\\terraform\\setup\\main.tf");
            File.Create ($"{currentDirectory}\\terraform\\setup\\vars.tf");
            Cli.PrintLine ("Environment main and vars tf files created.");

            File.Create ($"{currentDirectory}\\terraform\\global.tfvars");
            File.Create ($"{currentDirectory}\\terraform\\dev.tfvars");
            File.Create ($"{currentDirectory}\\terraform\\qa.tfvars");
            File.Create ($"{currentDirectory}\\terraform\\qts.tfvars");
            File.Create ($"{currentDirectory}\\terraform\\prod.tfvars");
            Cli.PrintLine ("Environment tf variable files created.");

            File.Create ($"{currentDirectory}\\terraform\\readme.md");
            Cli.PrintLine ("Terraform project created!", ConsoleColor.Green);
        }
    }
}