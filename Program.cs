using System;
using Microsoft.Extensions.CommandLineUtils;
using taide.Commands;

namespace taide {
    class Program {
        private const string helpValues = "-?|-h|--help";

        static void Main (string[] args) {
            var app = new CommandLineApplication ();
            app.Name = "taide";
            app.Description = ".NET Core console app with argument parsing.";
            app.HelpOption (helpValues);

            var basicOption = app.Option ("-o|--option<optionvalue>",
                "Some option value",
                CommandOptionType.SingleValue);

            app.OnExecute (() => {
                if (basicOption.HasValue ()) {
                    Console.WriteLine ("Option was selected, value: {0}", basicOption.Value ());
                } else {
                    app.ShowHint ();
                }

                return 0;
            });

            app.Command ("setup", (command) => {
                command.Description = "Creates commonly used variables and modules in Terraform project.";
                command.HelpOption (helpValues);
                command.OnExecute (() => {
                    var action = new SetupAction ();
                    action.Execute ();
                    return 0;
                });
            });

            app.Command ("create", (command) => {
                command.Description = "Creates a new Terraform project folder in the current directory.";
                command.HelpOption (helpValues);
                command.OnExecute (() => {
                    var action = new CreateAction ();
                    action.Execute ();
                    return 0;
                });
            });

            app.Command ("module", (command) => {
                command.Description = "";
                command.HelpOption (helpValues);
                command.OnExecute (() => {
                    var action = new ModuleAction ();
                    action.Execute ();
                    return 0;
                });
            });

            app.Command ("variable", (command) => {
                command.Description = "";
                command.HelpOption (helpValues);
                command.OnExecute (() => {
                    var action = new VariableAction ();
                    action.Execute ();
                    return 0;
                });
            });

            app.Command ("test", (command) => {
                command.Description = "";
                command.HelpOption (helpValues);
                command.OnExecute (() => {
                    var action = new TestAction ();
                    action.Execute ();
                    return 0;
                });
            });

            app.Execute (args);
            #if DEBUG
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
            #endif
        }
    }
}

