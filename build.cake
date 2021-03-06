#addin nuget:?package=Cake.Coverlet&version=2.1.2

using System.Xml.Linq;

var target = Argument("target", "default");
var configuration = Argument("configuration", "Release");
var output = Argument("output", "./artifacts");
var versionSuffix = Argument<string>("versionSuffix", null);
var productVersion = XDocument.Load("./src/Shorthand.RssFilter/Shorthand.RssFilter.csproj").Descendants("VersionPrefix").First().Value;

Information("Target: " + target);
Information("Configuration: " + configuration + ", tests always run under Debug");
Information("Output path: " + output);
Information("Version suffix: " + versionSuffix);

Task("clean")
    .Does(() => {
        CleanDirectories("./src/**/bin/" + configuration);
        CleanDirectories("./src/**/obj/" + configuration);
        CleanDirectories("./test/**/bin/Debug");
        CleanDirectories("./test/**/obj/Debug");
        CleanDirectory("./artifacts");
        CleanDirectory("./coverage");
    });    

Task("build")
    .IsDependentOn("clean")
    .Does(() => {
        var buildSettings = new DotNetCoreBuildSettings {
            VersionSuffix = versionSuffix,
            Configuration = configuration
        };

        DotNetCoreBuild("./src/Shorthand.RssFilter/Shorthand.RssFilter.csproj", buildSettings);
    });

Task("publish")
    .IsDependentOn("clean")
    .Does(() => {
        PublishRuntime("win-x64");
        PublishRuntime("osx.10.12-x64");
    });

Task("test")
    .IsDependentOn("clean")
    .Does(() => {
        var settings = new DotNetCoreTestSettings {
            Logger = "console;verbosity=normal",
            TestAdapterPath = "."
         };

        var coverletSettings = new CoverletSettings {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.cobertura | CoverletOutputFormat.opencover,
            CoverletOutputDirectory = Directory(@".\coverage\"),
            CoverletOutputName = $"results-{DateTime.UtcNow:dd-MM-yyyy-HH-mm-ss-FFF}"
        };
            
        DotNetCoreTest("./test/Shorthand.RssFilter.Tests/Shorthand.RssFilter.Tests.csproj", settings, coverletSettings);
    });

Task("azure-pipeline")
    .IsDependentOn("clean")
    .Does(() => {
        var settings = new DotNetCoreTestSettings {
            Logger = "trx;LogFileName=TestResults.trx"
         };

        var coverletSettings = new CoverletSettings {
            CollectCoverage = true,
            CoverletOutputFormat = CoverletOutputFormat.cobertura,
            CoverletOutputDirectory = Directory(@".\coverage\"),
            CoverletOutputName = "results"
        };
            
        DotNetCoreTest("./test/Shorthand.RssFilter.Tests/Shorthand.RssFilter.Tests.csproj", settings, coverletSettings);
    });

Task("default")
    .IsDependentOn("build");

RunTarget(target);

public void PublishRuntime(string runtime) {
    var settings = new DotNetCorePublishSettings(){
        Configuration = configuration,
        VersionSuffix = versionSuffix,
        Runtime = runtime
    };

    settings.OutputDirectory = output + "/" + runtime;
    DotNetCorePublish("./src/Shorthand.RssFilter/Shorthand.RssFilter.csproj", settings);
    
    DeleteFiles("./artifacts/**/feeds.json");

    Zip(settings.OutputDirectory, String.Format("./artifacts/release-{0}-{1}.zip", runtime, productVersion));
}