using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Shorthand.RssFilter.UI {
    public class Program {
        public static void Main(string[] args) {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))
                .Build();

            var rollingFileFormat = Path.Combine(Directory.GetCurrentDirectory(), "logs" + Path.DirectorySeparatorChar + "web-.log");

            var host = WebHost.CreateDefaultBuilder()
                .UseConfiguration(config)
                .UseKestrel(options => {
                    options.AddServerHeader = false;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                /* This one is great to find out namespaces for message filtering
                .ConfigureLogging((hostingContext, logging) => {
                    logging.AddConsole();
                }) */
                .UseSerilog((ctx, loggerConfig) => loggerConfig
                    .ReadFrom.Configuration(config)
                    .WriteTo.ColoredConsole()
                    .WriteTo.File(rollingFileFormat,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{RequestId}] {Message:lj}{NewLine}{Exception}",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        fileSizeLimitBytes: 25 * 1024 * 1024,
                        retainedFileCountLimit: 50,
                        buffered: true,
                        encoding: Encoding.UTF8))
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
