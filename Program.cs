using System;
using Microsoft.Extensions.CommandLineUtils;

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

            app.Command("setup", (command) =>
            {
                command.Description = "";
                command.HelpOption(helpValues);

                command.OnExecute(() =>
                {
                    Console.WriteLine("Setup Initiated.");
                    return 0;
                });
            });

            app.Command("create", (command) =>
            {
                command.Description = "";
                command.HelpOption(helpValues);

                command.OnExecute(() =>
                {
                    Console.WriteLine("Create Initiated.");
                    return 0;
                });
            });

            app.Command("module", (command) =>
            {
                command.Description = "";
                command.HelpOption(helpValues);

                command.OnExecute(() =>
                {
                    Console.WriteLine("Module Initiated.");
                    return 0;
                });
            });

            app.Command("variable", (command) =>
            {
                command.Description = "";
                command.HelpOption(helpValues);

                command.OnExecute(() =>
                {
                    Console.WriteLine("Variable Initiated.");
                    return 0;
                });
            });

            app.Command("test", (command) =>
            {
                command.Description = "";
                command.HelpOption(helpValues);

                command.OnExecute(() =>
                {
                    Console.WriteLine("Test Initiated.");
                    return 0;
                });
            });

            app.Execute (args);
        }
    }
}