[T4Scaffolding.Scaffolder()][CmdletBinding()]
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

Add-ClassLibraryToSolution $securityProjectName $path $sln

##############################################################
# Add system references to Membership
##############################################################

foreach ($ref in @("System.Configuration", "System.Web", "System.Web.Abstractions", "System.Web.Mvc")){
	Add-AssemblyReferenceToProject $securityProjectName $ref
}

##############################################################
# Add membership reference to website
##############################################################

Add-ProjectReferenceToProject $mvcProjectName $securityProjectName

##############################################################
# Add DevLevel
##############################################################

$outputPath = "DevLevel"

Add-ProjectItemViaTemplate $outputPath -Template DevLevel `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added DevLevel at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force	

##############################################################
# Add EnvironmentSection
##############################################################

$outputPath = "EnvironmentSection"

Add-ProjectItemViaTemplate $outputPath -Template EnvironmentSection `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added EnvironmentSection at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force	

##############################################################
# Add ErrorPagesSection
##############################################################

$outputPath = "ErrorPagesSection"

Add-ProjectItemViaTemplate $outputPath -Template ErrorPagesSection `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added ErrorPagesSection at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add GlobalSettings
##############################################################

$outputPath = "GlobalSettings"

Add-ProjectItemViaTemplate $outputPath -Template GlobalSettings `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added GlobalSettings at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add MonitoringSection
##############################################################

$outputPath = "MonitoringSection"

Add-ProjectItemViaTemplate $outputPath -Template MonitoringSection `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added MonitoringSection at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add SecureActionFilter
##############################################################

$outputPath = "SecureActionFilter"

Add-ProjectItemViaTemplate $outputPath -Template SecureActionFilter `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added SecureActionFilter at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add SecurityOption
##############################################################

$outputPath = "SecurityOption"

Add-ProjectItemViaTemplate $outputPath -Template SecurityOption `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added SecurityOption at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add SecuritySection
##############################################################

$outputPath = "SecuritySection"

Add-ProjectItemViaTemplate $outputPath -Template SecuritySection `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added SecuritySection at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add UnsecureAttribute
##############################################################

$outputPath = "UnsecureAttribute"

Add-ProjectItemViaTemplate $outputPath -Template UnsecureAttribute `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added UnsecureAttribute at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add WebmasterSection
##############################################################

$outputPath = "WebmasterSection"

Add-ProjectItemViaTemplate $outputPath -Template WebmasterSection `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added WebmasterSection at {0}" `
	-TemplateFolders $TemplateFolders -Project $securityProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add database seed
##############################################################

Add-CodeToMethod (Get-Project $mvcProjectName).Name "\" "Global.asax.cs" "MvcApplication" "RegisterGlobalFilters" "filters.Add(new Security.SecureActionFilter());"

