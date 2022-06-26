using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;

using Serilog;

using System;
using System.Linq;

using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[ShutdownDotNetAfterServerBuild]
internal partial class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Push);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    private readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("Api key to push packages to nuget.org.")]
    [Secret]
    private string NuGetApiKey;

    [Parameter("Api key to push packages to myget.org.")]
    [Secret]
    private string MyGetApiKey;

    [Solution] private readonly Solution Solution;

    private AbsolutePath SourceDirectory => RootDirectory / "src";
    private AbsolutePath OutputDirectory => RootDirectory / "output";
    private AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    protected override void OnBuildInitialized()
    {
        base.OnBuildInitialized();
        NuGetApiKey ??= Environment.GetEnvironmentVariable(nameof(NuGetApiKey));
        MyGetApiKey ??= Environment.GetEnvironmentVariable(nameof(MyGetApiKey));
    }

    private Target Initial => _ => _
        .Description("Initial")
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
        });

    private Target Clean => _ => _
        .Description("Clean Solution")
        .DependsOn(Initial)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    private Target Restore => _ => _
        .Description("Restore Solution")
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution)
            );
        });

    private Target Compile => _ => _
    .Description("Compile Solution")
    .DependsOn(Restore)
    .Executes(() =>
    {
        DotNetBuild(s => s
            .SetProjectFile(Solution)
            .SetConfiguration(Configuration)
            .EnableNoRestore());
    });

    //private Target Pack => _ => _
    //    .Description("Package Project")
    //    .DependsOn(Compile)
    //    .Executes(() =>
    //    {
    //        new[] { "GodSharp.FluentMember", "GodSharp.FluentMember.Generator" }
    //            .ForEach(x
    //                => DotNetPack(s => s
    //                    .SetProject(Solution.GetProject(x))
    //                    .SetConfiguration(Configuration)
    //                    .SetDescription("Source Generator for Fluent Member produced by GodSharp")
    //                    .SetPackageTags("FluentMember", "SourceGenerator", "Roslyn", "GodSharp")
    //                    .SetOutputDirectory(ArtifactsDirectory)
    //                    .EnableNoRestore()
    //                )
    //            );
    //    });
    private Target Copy => _ => _
        .Description("Copy NuGet Package")
        .OnlyWhenStatic(() => IsServerBuild, () => Configuration.Equals(Configuration.Release))
        .DependsOn(Compile)
        .Executes(() =>
        {
            GlobFiles(OutputDirectory, "**/*.nupkg")
                ?.Where(x => !x.EndsWith(".symbols.nupkg"))
                .ForEach(x => CopyFileToDirectory(x, ArtifactsDirectory / "packages", FileExistsPolicy.OverwriteIfNewer));
        });

    private Target Artifacts => _ => _
        .DependsOn(Copy)
        .OnlyWhenStatic(() => IsServerBuild)
        .Produces(ArtifactsDirectory / "**.nupkg")
        //.OnlyWhenStatic(() => HostType == HostType.AzurePipelines)
        .Description("Upload Artifacts")
        .Executes(() =>
        {
            //Log.Information("Upload artifacts to azure...");
            //AzurePipelines
            //    .UploadArtifacts("artifacts", "artifacts", ArtifactsDirectory);
            //Log.Information("Upload artifacts to azure finished.");
        });

    private Target Push => _ => _
        .Description("Push NuGet Package")
        .OnlyWhenStatic(() => IsServerBuild, () => Configuration.Equals(Configuration.Release))
        .DependsOn(Copy)
        .Requires(() => NuGetApiKey)
        .Requires(() => MyGetApiKey)
        .Executes(() =>
        {
            GlobFiles(ArtifactsDirectory, "**/*.nupkg")
                ?.Where(x => !x.EndsWith(".symbols.nupkg"))
                .ForEach(Nuget);
        });

    private Target Deploy => _ => _
        .Description("Deploy")
        .DependsOn(Push, Artifacts)
        .Executes(() =>
        {
            Log.Information("Deployed");
        });

    private void Nuget(string x)
    {
        Nuget(x, "https://www.myget.org/F/godsharp/api/v2/package", MyGetApiKey);
        Nuget(x, "https://api.nuget.org/v3/index.json", NuGetApiKey);
    }

    private void Nuget(string x, string source, string key) =>
        DotNetNuGetPush(s => s
            .SetTargetPath(x)
            .SetSource(source)
            .SetApiKey(key)
            .SetSkipDuplicate(true)
        );
}