using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Sibusten.Philomena.Client.Examples;

namespace Sibusten.Philomena.Client.Examples
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            List<IExample> examples = new List<IExample>
            {
                new DownloadImageToFile(),
                new EnumerateSearchQuery(),
                new DownloadSvgImages(),
                new PauseAndCancelImageDownload(),
                new GetTagsForImage(),
            };

            // Set up logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            Sibusten.Philomena.Client.Logging.Logger.Factory = LoggerFactory.Create(configure =>
            {
                configure.SetMinimumLevel(LogLevel.Debug);
                configure.AddSerilog();
            });

            // Configure retry policies
            PhilomenaClientRetryLogic.UseDefaultHttpRetryLogic();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Philomena Client examples");

                // Write the list of examples
                for (int i = 0; i < examples.Count; i++)
                {
                    Console.WriteLine($"  {i:#0} - {examples[i].Description}");
                }

                // Write the quit option
                Console.WriteLine($"  {examples.Count} - Quit");

                Console.WriteLine();
                Console.WriteLine("Enter the number of the example to run: ");
                string input = Console.ReadLine();

                // Parse the input and validate
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine($"Invalid input: '{input}'");
                    continue;
                }

                // Ensure the choice is in range
                if (choice < 0 || choice > examples.Count)
                {
                    Console.WriteLine($"Invalid choice: {choice}");
                    continue;
                }

                // Check for the quit choice
                if (choice == examples.Count)
                {
                    return;
                }

                // Run the example
                IExample exampleToRun = examples[choice];
                Console.WriteLine();
                Console.WriteLine("----");
                Console.WriteLine(exampleToRun.Description);
                Console.WriteLine("----");
                Console.WriteLine();
                await exampleToRun.RunExample();
            }
        }
    }
}
