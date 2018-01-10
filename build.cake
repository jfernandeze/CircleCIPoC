#addin "Cake.Karma"
#tool "nuget:?package=OpenCover"
#tool "nuget:?package=ReportGenerator"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var solution = Argument("solution", "src/DialogWeaver.sln");
var publishOutput = "./publish/";
var codeCoverageOutput = "./codeCoverage/";
var codeCoverageReportOutput = "./codeCoverage/report/";

Task("Clean")
	.Does(() =>
	{
		CleanDirectory(publishOutput);
		CleanDirectory(codeCoverageOutput);
		EnsureDirectory(codeCoverageReportOutput);
	});


Task("RestorePackages")
	.Does(() =>
	{
		DotNetCoreRestore(solution);
	});

Task("Build")
	.IsDependentOn("RestorePackages")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		var settings = new DotNetCoreMSBuildSettings()
			.WithTarget("Clean")
			.WithTarget("Rebuild")
			.SetConfiguration(configuration);

		DotNetCoreMSBuild(solution, settings);
	});

Task("Publish")
	.Does(() => 
	{
		var websites = System.IO.Directory.EnumerateDirectories("./src/Webs");

		foreach(var web in websites)
		{
			var webLastDirectory = web.Split(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar).Last();
			var outputPath = System.IO.Path.Combine(publishOutput, webLastDirectory);
			var settings = new DotNetCorePublishSettings
			 {
				 Configuration = configuration,
				 OutputDirectory = outputPath
			 };

			DotNetCorePublish(web, settings);
		}
	});

Task("UnitTests")
	.Does(() =>
	{
		var settings = new DotNetCoreTestSettings
		 {
			 Configuration = configuration
		 };

		 var projectFiles = GetFiles("./**/*.Tests.csproj");
		 foreach(var file in projectFiles)
		 {
		   var buildFolder = FindBuildFolder(file);
		   var openCoverSettings = new OpenCoverSettings
			{
				OldStyle = true,
				MergeOutput = true
		    }
			.WithFilter("+[DialogWeaver.*]*")
			.WithFilter("-[*.Tests*]*");

			openCoverSettings.SearchDirectories.Add(buildFolder);

			 OpenCover(tool =>
				{
					tool.DotNetCoreTest(file.FullPath, settings);
				},
				System.IO.Path.Combine(codeCoverageOutput, "result.xml"),
				openCoverSettings);
		 }

		 ReportGenerator(System.IO.Path.Combine(codeCoverageOutput, "result.xml"), codeCoverageReportOutput);
	});

Task("SPATests")
	.Does(() =>
	{
		Information("Running Local Karma");

		var currentDirectory = Environment.CurrentDirectory;

        var settings = new KarmaStartSettings
        {
           ConfigFile = System.IO.Path.Combine(currentDirectory, "src/Webs/WebSpa/ClientApp/test/karma.conf.js"),		                  
		   RunMode = KarmaRunMode.Local,
		   LocalKarmaCli = System.IO.Path.Combine(currentDirectory, "src/Webs/WebSpa/node_modules/karma-cli/bin/karma"),
		   WorkingDirectory = System.IO.Path.Combine(currentDirectory,"src/Webs/WebSpa")
        };

		KarmaStart(settings);
	});

private void CleanDirectory(string path)
{
	if(System.IO.Directory.Exists(path))
	{
		System.IO.Directory.Delete(path, true);
		System.IO.Directory.CreateDirectory(path);
	}
	else
	{
		System.IO.Directory.CreateDirectory(path);
	}
}

private void EnsureDirectory(string path)
{
	if(!System.IO.Directory.Exists(path))
	{
		System.IO.Directory.CreateDirectory(path);
	}
}

private string FindBuildFolder(FilePath projectFilePath)
{
	var folder = System.IO.Path.GetDirectoryName(projectFilePath.ToString());
	var pattern = string.Format(@"bin\{0}\*", configuration);
	return System.IO.Directory.EnumerateDirectories(folder, pattern).FirstOrDefault();
}

RunTarget(target);