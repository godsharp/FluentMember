using Nuke.Common.CI.GitHubActions;

[GitHubActions(
    "continuous.build",
    GitHubActionsImage.WindowsLatest,
    GitHubActionsImage.UbuntuLatest,
    GitHubActionsImage.MacOsLatest,
    AutoGenerate = true,
    PublishArtifacts = true,
    OnPushIncludePaths = new string[] { "**" },
    OnPushExcludePaths = new string[] { ".release" },
    InvokedTargets = new[] { nameof(Compile), nameof(Artifacts) },
    CacheKeyFiles = new string[0]
)]
[GitHubActions(
    "continuous",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    PublishArtifacts = true,
    OnPushIncludePaths = new string[] { "'.release'" },
    OnPushBranches = new[] { "main" },
    InvokedTargets = new[] { nameof(Deploy) },
    ImportSecrets = new[] { nameof(NuGetApiKey), nameof(MyGetApiKey) },
    CacheKeyFiles = new string[0]
)]
internal partial class Build
{
}