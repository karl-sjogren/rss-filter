using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shorthand.RssFilter.Contracts;
using Shorthand.RssFilter.Models;
using Shorthand.RssFilter.Serialization;
using Shorthand.RssFilter.Services;

namespace Shorthand.RssFilter {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().AddJsonOptions(o => {
                o.SerializerSettings.Converters.Add(new StringEnumConverter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpClient();

            services
                .AddSingleton<IRssFilterService, RssFilterService>()
                .AddSingleton<IRssService, RssService>()
                .AddSingleton<IActionContextAccessor, ActionContextAccessor>()
                .AddScoped<IUrlHelper>(x => x
                    .GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

            services.Configure<ApplicationConfiguration>(options => {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new FilterBaseConverter());

                var json = File.ReadAllText("feeds.json");
                JsonConvert.PopulateObject(json, options, settings);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } 

            var rnd = new Random();
            app.Use(async (ctx, next) => {
                if(ctx.Request.Path != PathString.FromUriComponent("/")) {
                    await next();
                    return;
                }

                var animals = new[] {"🦒", "🦓", "🦙", "🐘", "🐅" };

                var animal = animals[rnd.Next(animals.Length)];

                await ctx.Response.WriteAsync(@"<html><head><meta charset=""utf-8""><style type=""text/css"">body { margin: 0; } .animal { width: 100vw; height: 100vh; } .animal span { position: absolute; top: 50%; left: 50%; transform: translate(-50%, -50%); font-size: 300px; } @media (-webkit-min-device-pixel-ratio: 1.25), (min-resolution: 120dpi) { .animal span { font-size: 146px; } }</style></head><body><div class=""animal""><span>" + animal + "</span></div></body></html>", Encoding.UTF8);
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
