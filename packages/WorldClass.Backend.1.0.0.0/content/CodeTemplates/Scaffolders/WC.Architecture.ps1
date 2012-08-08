[T4Scaffolding.Scaffolder(Description = "WC.Architecture - Setup of projects and references in solution.")][CmdletBinding()]
param(        
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

##############################################################
# Scaffold the projects needed for solution
##############################################################

scaffold WC.Core
scaffold WC.Data
scaffold WC.Services
scaffold WC.Base

##############################################################
# Scaffold empty database
##############################################################

scaffold WC.Model

##############################################################
# Create empty migration record
##############################################################

enable-migrations -ProjectName $dataProjectName -EnableAutomaticMigrations
add-migration  InitialMigration -ProjectName $dataProjectName
update-database -ProjectName $dataProjectName