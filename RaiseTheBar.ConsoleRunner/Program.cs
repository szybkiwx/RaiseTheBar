using RiseTheBar;
using System;


namespace RaiseTheBar.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Microsoft.Owin.Hosting.WebApp.Start<Startup>("http://localhost:32056"))
            {
                Console.WriteLine("Listening on http://localhost:3205.");
                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }
    }
}
