#addin "Cake.Npm"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var solution = Argument("solution", "DialogWeaver.sln");
var publishOutput = "./publish/";



Task("RestorePackages")
	.Does(() =>
	{
		DotNetCoreRestore();
	});

Task("Build")
	.IsDependentOn("RestorePackages")
	.Does(() =>
	{
		var settings = new DotNetCoreMSBuildSettings()
			.WithTarget("Clean")
			.WithTarget("Rebuild")
			.SetConfiguration(configuration);

		DotNetCoreMSBuild(solution, settings);
	});

Task("Publish")
	.IsDependentOn("Build")	
	.Does(() => 
	{
		var websites = System.IO.Directory.EnumerateDirectories("./Webs");

		foreach(var web in websites)
		{
			Information(web);
			var webLastDirectory = System.IO.Path.GetDirectoryName(web).Split(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar).Last();
			Information(webLastDirectory);
			var outputPath = System.IO.Path.Combine(publishOutput, webLastDirectory);
			Information(outputPath);
			var settings = new DotNetCorePublishSettings
			 {
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
			 DotNetCoreTest(file.FullPath, settings);
		 }
	});

RunTarget(target);