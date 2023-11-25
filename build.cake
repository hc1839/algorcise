var target = Argument("target", "Build");
var configuration = Argument("configuration", "Release");

Task("Clean").WithCriteria(c => HasArgument("rebuild")).Does(() =>
{
    CleanDirectory($"./bin/{configuration}");
    CleanDirectory($"./obj/{configuration}");
});

Task("Build").IsDependentOn("Clean").Does(() =>
{
    DotNetBuild("./algorcise.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test").IsDependentOn("Build").Does(() =>
{
    DotNetTest("./algorcise.sln", new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

RunTarget(target);
