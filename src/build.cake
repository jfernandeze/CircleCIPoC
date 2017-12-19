var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var solution = Argument("solution", "DialogWeaver.sln");

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