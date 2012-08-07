[T4Scaffolding.Scaffolder(Description = "WC.Architecture - Setup of projects and references in solution.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

Add-NamespacesToHost $Project

##############################################################
# Add Core projcet to solution
##############################################################

Add-ClassLibraryToSolution $coreProjectName $path $sln

##############################################################
# Add references to Core
##############################################################

foreach ($ref in @("System.ServiceModel", "System.Runtime.Serialization", "System.ComponentModel.DataAnnotations")){
	Add-AssemblyReferenceToProject $coreProjectName $ref
}

##############################################################
# Configure Nuget packages for project
##############################################################

if((get-package -ProjectName $coreProjectName | Select-Object -ExpandProperty ID) -contains 'EntityFramework'){
	Write-Host $coreProjectName Looking for update : EntityFramework -ForegroundColor DarkGreen
	Update-Package EntityFramework -ProjectName $coreProjectName
}
else{
	Write-Host $coreProjectName Installing : EntityFramework -ForegroundColor DarkGreen
	Install-Package EntityFramework -ProjectName $coreProjectName
}

##############################################################
# Add Data Interface - IRepository
##############################################################
$outputPath = "Interfaces\Data\IRepository"
$namespace = $coreProjectName + ".Interfaces.Data"
$ximports = $coreProjectName + ".Interfaces.Paging"

Add-ProjectItemViaTemplate $outputPath -Template IRepository `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports} `
	-SuccessMessage "Added IRepository at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Data Interface - IUnitOfWork
##############################################################
$outputPath = "Interfaces\Data\IUnitOfWork"
$namespace = $coreProjectName + ".Interfaces.Data"

Add-ProjectItemViaTemplate $outputPath -Template IUnitOfWork `
	-Model @{ Namespace = $namespace; } `
	-SuccessMessage "Added IUnitOfWork at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Data Interface - IDatabaseFactory
##############################################################
$outputPath = "Interfaces\Data\IDatabaseFactory"
$namespace = $coreProjectName + ".Interfaces.Data"

Add-ProjectItemViaTemplate $outputPath -Template IDatabaseFactory `
	-Model @{ Namespace = $namespace; } `
	-SuccessMessage "Added IDatabaseFactory at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force


##############################################################
# Add Data Interface - IDataContext
##############################################################
$outputPath = "Interfaces\Data\IDataContext"
$namespace = $coreProjectName + ".Interfaces.Data"
$ximports = $coreProjectName + ".Model"

Add-ProjectItemViaTemplate $outputPath -Template IDataContext `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports} `
	-SuccessMessage "Added IDataContext at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Service Interface - IService
##############################################################
$outputPath = "Interfaces\Service\IService"
$namespace = $coreProjectName + ".Interfaces.Service"
$ximports = $coreProjectName + ".Interfaces.Validation," + $coreProjectName + ".Interfaces.Paging"

Add-ProjectItemViaTemplate $outputPath -Template IService `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports} `
	-SuccessMessage "Added IService at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Model Entity - PersistentEntity
##############################################################
$outputPath = "Model\PersistentEntity"
$namespace = $coreProjectName + ".Model"

Add-ProjectItemViaTemplate $outputPath -Template PersistentEntity `
	-Model @{ 
		Namespace = $namespace;  
	} `
	-SuccessMessage "Added PersistentEntity at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Validation Interface - IValidationContainer
##############################################################
$outputPath = "Interfaces\Validation\IValidationContainer"
$namespace = $coreProjectName + ".Interfaces.Validation"

Add-ProjectItemViaTemplate $outputPath -Template IValidationContainer `
	-Model @{ Namespace = $namespace; } `
	-SuccessMessage "Added IValidationContainer at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Validation - ValidationContainer
##############################################################
$outputPath = "Common\Validation\ValidationContainer"
$namespace = $coreProjectName + ".Common.Validation"
$ximports = $coreProjectName + ".Interfaces.Validation"

Add-ProjectItemViaTemplate $outputPath -Template ValidationContainer `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports } `
	-SuccessMessage "Added ValidationContainer at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Validation - ValidationEngine
##############################################################
$outputPath = "Common\Validation\ValidationEngine"
$namespace = $coreProjectName + ".Common.Validation"
$ximports = $coreProjectName + ".Interfaces.Validation," + $coreProjectName + ".Model"

Add-ProjectItemViaTemplate $outputPath -Template ValidationEngine `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports } `
	-SuccessMessage "Added ValidationEngine at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Paging Interface - IPage
##############################################################
$outputPath = "Interfaces\Paging\IPage"
$namespace = $coreProjectName + ".Interfaces.Paging"

Add-ProjectItemViaTemplate $outputPath -Template IPage `
	-Model @{ Namespace = $namespace; } `
	-SuccessMessage "Added IPage at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Paging - Page
##############################################################
$outputPath = "Common\Paging\Page"
$namespace = $coreProjectName + ".Common.Paging"
$ximports = $coreProjectName + ".Interfaces.Paging," + $coreProjectName + ".Model"

Add-ProjectItemViaTemplate $outputPath -Template Page `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports } `
	-SuccessMessage "Added Page at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force