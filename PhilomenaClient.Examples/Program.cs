using System;
using Philomena.Client;

namespace Philomena.Client.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            PhilomenaClient client = new PhilomenaClient();
            client.Search();
            Console.WriteLine("Hello World!");
        }
    }
}
