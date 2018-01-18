#addin "Cake.Docker"
#addin "Cake.Karma"
#tool "nuget:?package=GitVersion.CommandLine"

var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");
var solution = Argument("solution", "src/DialogWeaver.sln");
var publishOutput = "./publish/";
Cake.Common.Tools.GitVersion.GitVersion version;

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
	.IsDependentOn("UpdateNetcoreVersion")
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
			var contentPath = System.IO.Path.Combine(outputPath, "content");
			var sourceDockerfilePath = System.IO.Path.Combine(web, "Dockerfile_CI");
			var targetDockerfilePath = System.IO.Path.Combine(outputPath, "Dockerfile");

			var settings = new DotNetCorePublishSettings
			 {
				 Configuration = configuration,
				 OutputDirectory = contentPath
			 };

			DotNetCorePublish(web, settings);

			CopyFile(sourceDockerfilePath, targetDockerfilePath);
		}
	});

Task("UnitTests")
	.Does(() =>
	{
		var settings = new DotNetCoreTestSettings
		 {
			 Configuration = configuration,
			 NoBuild = true
		 };

		 var projectFiles = GetFiles("./src/**/*.Tests.csproj");
		 foreach(var file in projectFiles)
		 {
		   
			DotNetCoreTest(file.FullPath, settings);
		 }
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

Task("GetGitVersion")
	.Does(() =>
	{
		version = GitVersion(new GitVersionSettings
			{
				NoFetch=true
			});
    });

Task("UpdateNetcoreVersion")
	.IsDependentOn("GetGitVersion")
	.Does(() =>
	{
		var projectFiles = GetFiles("./src/**/*.csproj");
		foreach(var file in projectFiles)
		{
			Information(file.ToString());
			XmlPoke(file, "/Project/PropertyGroup[1]/AssemblyVersion", version.AssemblySemVer);
			XmlPoke(file, "/Project/PropertyGroup[1]/FileVersion", version.AssemblySemVer);
		}
	});

	Task("ImageBuild")
	.IsDependentOn("GetGitVersion")
	.Does(() =>
	{
		foreach(var publishTarget in System.IO.Directory.EnumerateDirectories(publishOutput))
		{
			var targetName = publishTarget.Split(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar).Last().ToLower();
			var settings = new DockerImageBuildSettings
			{
				Tag = new[] { $"registry.cdpoc/arcmedia/dialogweaver/arcmedia/DW_{targetName}:{version.SemVer}".ToLower(), $"registry.cdpoc/arcmedia/dialogweaver/arcmedia/DW_{targetName}:latest".ToLower() }
			};
			DockerBuild(settings, publishTarget);
			
			
			DockerPush($"registry.cdpoc/arcmedia/dialogweaver/arcmedia/DW_{targetName}:{version.SemVer}".ToLower());
		}
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