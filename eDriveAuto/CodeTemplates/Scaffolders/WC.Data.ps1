[T4Scaffolding.Scaffolder(Description = "WC.Architecture - Setup of projects and references in solution.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

Add-NamespacesToHost $Project

##############################################################
# Add all the class libraries to solution
##############################################################

Add-ClassLibraryToSolution $dataProjectName $path $sln

Add-ProjectReferenceToProject $dataProjectName $coreProjectName

##############################################################
# Add references to Data
##############################################################

foreach ($ref in @("System.ServiceModel", "System.Runtime.Serialization")){
	Add-AssemblyReferenceToProject $dataProjectName $ref
}

##############################################################
# Configure Nuget packages for project
##############################################################

if((get-package -ProjectName $dataProjectName | Select-Object -ExpandProperty ID) -notcontains 'EntityFramework'){
	Write-Host $dataProjectName Installing : EntityFramework -ForegroundColor DarkGreen
	Install-Package EntityFramework -ProjectName $dataProjectName
}
else{
	if(((Get-Project $dataProjectName).Object.References | Select-Object -ExpandProperty Name) -notcontains "EntityFramework"){
		Write-Host $dataProjectName Installing : EntityFramework -ForegroundColor DarkGreen
		Install-Package EntityFramework -ProjectName $dataProjectName
	}else{
		Write-Host $dataProjectName Looking for update : EntityFramework -ForegroundColor DarkGreen
		Update-Package EntityFramework -ProjectName $dataProjectName
	}
}

##############################################################
# Add Data Repository - BaseRepository
##############################################################
$outputPath = "BaseRepository"
$namespace = $dataProjectName
$ximports = $coreProjectName + ".Model," + $coreProjectName + ".Interfaces.Data," + $coreProjectName + ".Interfaces.Paging," + $coreProjectName + ".Common.Paging"

Add-ProjectItemViaTemplate $outputPath -Template BaseRepository `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports } `
	-SuccessMessage "Added BaseRepository at {0}" `
	-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Data UOW - UnitOfWork
##############################################################
$outputPath = "UnitOfWork"
$namespace = $dataProjectName
$ximports = $coreProjectName + ".Model," + $coreProjectName + ".Interfaces.Data"

Add-ProjectItemViaTemplate $outputPath -Template UnitOfWork `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports } `
	-SuccessMessage "Added UnitOfWork at {0}" `
	-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Data DatabaseFactory - DatabaseFactory
##############################################################
$outputPath = "DatabaseFactory"
$namespace = $dataProjectName
$ximports = $coreProjectName + ".Interfaces.Data"

Add-ProjectItemViaTemplate $outputPath -Template DatabaseFactory `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports } `
	-SuccessMessage "Added DatabaseFactory at {0}" `
	-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Data DataContext - DataContext
##############################################################
$outputPath = "DataContext"
$namespace = $dataProjectName
$ximports = $coreProjectName + ".Model," + $coreProjectName + ".Interfaces.Data"

Add-ProjectItemViaTemplate $outputPath -Template DataContext `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports } `
	-SuccessMessage "Added DataContext at {0}" `
	-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force