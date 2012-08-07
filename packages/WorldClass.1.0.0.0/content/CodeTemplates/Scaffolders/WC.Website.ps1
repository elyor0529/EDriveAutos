[T4Scaffolding.Scaffolder(Description = "WC.Architecture - Setup of projects and references in solution.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

Add-NamespacesToHost $Project

##############################################################
# Project Names
##############################################################
$mvcProjectName = (Get-Project $Project).Name


$outputPath = "Views\Shared\_Layout"
Add-ProjectItemViaTemplate $outputPath -Template _Layout `
	-Model @{  } `
	-SuccessMessage "Added Layout output at {0}" `
	-TemplateFolders $TemplateFolders `
	-Project $mvcProjectName -Force

