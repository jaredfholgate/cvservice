//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define Paths and Package Sources
var solutionFiles = GetFiles("./*.sln").ToList();
var solutionPath = solutionFiles[0].FullPath;

//Define Version
var buildNumber = EnvironmentVariable("BUILD_BUILDNUMBER") ?? "1.1.1.1";
var versionArgs = " /p:AssemblyVersion=" + buildNumber + " /p:FileVersion=" + buildNumber + " /p:Version=" + buildNumber;

//Output Variables
Console.WriteLine("Solution: " + solutionPath);
Console.WriteLine("Verison: " + buildNumber);
Console.WriteLine("Configuration: " + configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
	CleanDirectory("./artifacts");
    var resultFiles = GetFiles("./**/*TestResults.xml");
    foreach(var file in resultFiles)
    {
        DeleteFile(file.FullPath);
    }
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore(solutionPath);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
     var settings = new DotNetCoreBuildSettings
     {
		 ArgumentCustomization = args => args.Append(versionArgs),
		 Configuration = configuration,
         NoRestore = true
     };
     DotNetCoreBuild(solutionPath, settings);
});

Task("Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var testResultFile = "UnitTestResults.xml";
    var settings = new DotNetCoreTestSettings
    {
        ArgumentCustomization = args => args.Append("--logger \"trx;LogFileName=" + testResultFile + "\""),
		Configuration = configuration,
		NoBuild = true,
		NoRestore = true
    };
    var projectFiles = GetFiles("./**/*.unittests.csproj");
    foreach(var file in projectFiles)
    {
        DotNetCoreTest(file.FullPath, settings);
    }
});

Task("Integration-Tests")
    .IsDependentOn("Unit-Tests")
    .Does(() =>
{

	var testResultFile = "IntnTestResults.xml";
	var settings = new DotNetCoreTestSettings
	{
		ArgumentCustomization = args => args.Append("--logger \"trx;LogFileName=" + testResultFile + "\""),
		Configuration = configuration,
		NoBuild = true,
		NoRestore = true
	};
	var projectFiles = GetFiles("./**/*.intntests.csproj");
	foreach(var file in projectFiles)
	{
		DotNetCoreTest(file.FullPath, settings);
	}
});

Task("Publish")
 .IsDependentOn("Integration-Tests")
    .Does(() =>
{
	var settings = new DotNetCorePublishSettings
     {
	     ArgumentCustomization = args => args.Append(versionArgs),
         Configuration = configuration,
         OutputDirectory = "./artifacts/CvServiceApi",
		 NoBuild = true,
		 NoRestore = true
     };
	DotNetCorePublish("./CvService.Api/CvService.Api.csproj", settings);
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
