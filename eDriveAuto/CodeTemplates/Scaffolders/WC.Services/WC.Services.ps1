[T4Scaffolding.Scaffolder(Description = "WC.Architecture - Setup of projects and references in solution.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

Add-NamespacesToHost $Project

##############################################################
# Configure the solution and add the correct references
##############################################################

Add-ClassLibraryToSolution $serviceProjectName $path $sln

foreach ($ref in @($dataProjectName, $coreProjectName)){
	Add-ProjectReferenceToProject $serviceProjectName $ref
}

##############################################################
# Add references to Service
##############################################################

foreach ($ref in @("System.ServiceModel", "System.Runtime.Serialization", "System.ComponentModel.DataAnnotations")){
	Add-AssemblyReferenceToProject $serviceProjectName $ref
}

##############################################################
# Add Service Service - BaseService
##############################################################
$outputPath = "BaseService"
$namespace = $serviceProjectName
$ximports = $coreProjectName + ".Model," + $coreProjectName + ".Interfaces.Data," + $coreProjectName + ".Interfaces.Service," + $coreProjectName + ".Common.Validation," + $coreProjectName + ".Interfaces.Validation," + $coreProjectName + ".Interfaces.Paging"

Add-ProjectItemViaTemplate $outputPath -Template BaseService `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports } `
	-SuccessMessage "Added BaseService at {0}" `
	-TemplateFolders $TemplateFolders -Project $serviceProjectName -CodeLanguage $CodeLanguage -Force:$Force