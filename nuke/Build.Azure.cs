using Nuke.Common.CI.AzurePipelines;

[AzurePipelines(
    null,
    AzurePipelinesImage.WindowsLatest,
    AzurePipelinesImage.UbuntuLatest,
    AzurePipelinesImage.MacOsLatest,
    AutoGenerate = true,
    InvokedTargets = new[] { nameof(Compile), nameof(Artifacts) },
    NonEntryTargets = new[] { nameof(Initial), nameof(Clean), nameof(Restore), nameof(Copy) },
    CacheKeyFiles = new string[0]
)]
internal partial class Build
{
    //[AzurePipelines(
    //    "build",
    //    AzurePipelinesImage.WindowsLatest,
    //    AzurePipelinesImage.UbuntuLatest,
    //    AzurePipelinesImage.MacOsLatest,
    //    AutoGenerate = true,
    //    TriggerPathsInclude = new string[] { "'**'" },
    //    TriggerPathsExclude = new string[] { "'.release'" },
    //    InvokedTargets = new[] { nameof(Compile) },
    //    NonEntryTargets = new[] { nameof(Initial), nameof(Clean), nameof(Restore) },
    //    CacheKeyFiles = new string[0]
    //)]
    //[AzurePipelines(
    //    null,
    //    AzurePipelinesImage.WindowsLatest,
    //    AutoGenerate = true,
    //    TriggerBranchesInclude = new[] { "main" },
    //    TriggerPathsInclude = new string[] { "'.release'" },
    //    InvokedTargets = new[] { nameof(Deploy), nameof(Artifacts) },
    //    NonEntryTargets = new[] { nameof(Initial), nameof(Clean), nameof(Restore), nameof(Compile), nameof(Pack), nameof(Push) },
    //    ImportSecrets = new[] { nameof(NuGetApiKey), nameof(MyGetApiKey) },
    //    CacheKeyFiles = new string[0]
    //)]
}