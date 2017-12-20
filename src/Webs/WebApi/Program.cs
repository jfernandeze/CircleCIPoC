using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DialogWeaver.WebApi
{
    /// <summary>
    /// Container for the entry point of the application
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Application entry point
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Builds the web host.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>the built web host</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
