param($installPath, $toolsPath, $package, $project)

$appPluginsFolder = $project.ProjectItems | Where-Object { $_.Name -eq "App_Plugins" }
$umbracoCloudToolkitFolder = $appPluginsFolder.ProjectItems | Where-Object { $_.Name -eq "UmbracoCloudToolkit" }

if (!$umbracoCloudToolkitFolder)
{
	$newPackageFiles = "$installPath\Content\App_Plugins\UmbracoCloudToolkit"

	$projFile = Get-Item ($project.FullName)
	$projDirectory = $projFile.DirectoryName
	$projectPath = Join-Path $projDirectory -ChildPath "App_Plugins"
	$projectPathExists = Test-Path $projectPath

	if ($projectPathExists) {
		Write-Host "Updating Umbraco Cloud Toolkit App_Plugin files using powershell as they have been excluded from the project"
		Copy-Item $newPackageFiles $projectPath -Recurse -Force
	}
}