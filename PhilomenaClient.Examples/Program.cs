using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Philomena.Client.Api;
using Philomena.Client.Api.Models;
using Philomena.Client.Examples.Api;
using Philomena.Client.Examples.Client;

namespace Philomena.Client.Examples
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            List<IExample> examples = new List<IExample>
            {
                new SearchImages(),
                new DownloadImageToFile(),
                new EnumerateSearchQuery(),
            };

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
