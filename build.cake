var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var solution = Argument("solution", "src/DialogWeaver.sln");
var publishOutput = "./publish/";


Task("Clean")
	.Does(() =>
	{
		CleanDirectory(publishOutput);
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
	.IsDependentOn("Build")	
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
			 DotNetCoreTest(file.FullPath, settings);
		 }
	});

private void CleanDirectory(string path)
{
	if(System.IO.Directory.Exists(path))
	{
		System.IO.Directory.Delete(path, true);
		System.IO.Directory.CreateDirectory(path);
	}
}

RunTarget(target);