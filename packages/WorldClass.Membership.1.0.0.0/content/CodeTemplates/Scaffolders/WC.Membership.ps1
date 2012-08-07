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

Add-ClassLibraryToSolution $membershipProjectName $path $sln

##############################################################
# Configure Nuget packages for project
##############################################################

if((get-package -ProjectName $membershipProjectName | Select-Object -ExpandProperty ID) -notcontains 'EntityFramework'){
	Write-Host $membershipProjectName Installing : EntityFramework -ForegroundColor DarkGreen
	Install-Package EntityFramework -ProjectName $membershipProjectName
}
else{
	if(((Get-Project $membershipProjectName).Object.References | Select-Object -ExpandProperty Name) -notcontains "EntityFramework"){
		Write-Host $membershipProjectName Installing : EntityFramework -ForegroundColor DarkGreen
		Install-Package EntityFramework -ProjectName $membershipProjectName
	}else{
		Write-Host $membershipProjectName Looking for update : EntityFramework -ForegroundColor DarkGreen
		Update-Package EntityFramework -ProjectName $membershipProjectName
	}
}

##############################################################
# Add system references to Membership
##############################################################

foreach ($ref in @("System.Configuration", "System.Web", "System.ComponentModel.DataAnnotations", "System.Web.Helpers", "System.Web.ApplicationServices")){
	Add-AssemblyReferenceToProject $membershipProjectName $ref
}

##############################################################
# Add project references to Membership
##############################################################

foreach ($ref in @($coreProjectName, $dataProjectName, $serviceProjectName)){
	Add-ProjectReferenceToProject $membershipProjectName $ref
}

##############################################################
# Add membership reference to website
##############################################################

Add-ProjectReferenceToProject $mvcProjectName $membershipProjectName

##############################################################
# Add User object to Models
##############################################################

$outputPath = "Model\User"
$namespace = $coreProjectName + ".Model"

Add-ProjectItemViaTemplate $outputPath -Template User `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added User at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force
	
##############################################################
# Add Role object to Models
##############################################################

$outputPath = "Model\Role"
$namespace = $coreProjectName + ".Model"

Add-ProjectItemViaTemplate $outputPath -Template Role `
	-Model @{ Namespace = $namespace } `
	-SuccessMessage "Added Role at {0}" `
	-TemplateFolders $TemplateFolders -Project $coreProjectName -CodeLanguage $CodeLanguage -Force:$Force
	
##############################################################
# Add Membership Provider
##############################################################

$outputPath = "Providers\CodeFirstMembershipProvider"
$namespace = $membershipProjectName + ".Providers"
$ximports = $coreProjectName + ".Model," + $dataProjectName + "," + $membershipProjectName + ".Helpers"

Add-ProjectItemViaTemplate $outputPath -Template CodeFirstMembershipProvider `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports} `
	-SuccessMessage "Added CodeFirstMembershipProvider at {0}" `
	-TemplateFolders $TemplateFolders -Project $membershipProjectName -CodeLanguage $CodeLanguage -Force:$Force
	
##############################################################
# Add Role Provider
##############################################################

$outputPath = "Providers\CodeFirstRoleProvider"
$namespace = $membershipProjectName + ".Providers"
$ximports = $coreProjectName + ".Model," + $dataProjectName

Add-ProjectItemViaTemplate $outputPath -Template CodeFirstRoleProvider `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports} `
	-SuccessMessage "Added CodeFirstRoleProvider at {0}" `
	-TemplateFolders $TemplateFolders -Project $membershipProjectName -CodeLanguage $CodeLanguage -Force:$Force
	
##############################################################
# Add Membership Helper
##############################################################

$outputPath = "Helpers\MembershipHelper"
$namespace = $membershipProjectName + ".Helpers"
$ximports = $coreProjectName + ".Model," + $dataProjectName + "," + $membershipProjectName + ".Providers"

Add-ProjectItemViaTemplate $outputPath -Template MembershipHelper `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports} `
	-SuccessMessage "Added MembershipHelper at {0}" `
	-TemplateFolders $TemplateFolders -Project $membershipProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Add Membership Helper
##############################################################

$outputPath = "Seeders\MembershipDataSeeder"
$namespace = $dataProjectName + ".Seeders"
$ximports = $coreProjectName + ".Model," + $dataProjectName

Add-ProjectItemViaTemplate $outputPath -Template MembershipDataSeeder `
	-Model @{ Namespace = $namespace; ExtraUsings = $ximports} `
	-SuccessMessage "Added MembershipHelper at {0}" `
	-TemplateFolders $TemplateFolders -Project $dataProjectName -CodeLanguage $CodeLanguage -Force:$Force

##############################################################
# Scaffold the membership objects
##############################################################

Scaffold WC.Model.For User
Scaffold WC.Model.For Role

##############################################################
# Add database seed
##############################################################

Add-CodeToMethod (Get-Project $dataProjectName).Name "\Migrations\" "Configuration.cs" "Configuration" "Seed" "new Seeders.MembershipDataSeeder().Seed(context);"

##############################################################
# Add LoginAuthorize attribute to global filters
##############################################################

Add-CodeToMethod (Get-Project $mvcProjectName).Name "\" "Global.asax.cs" "MvcApplication" "RegisterGlobalFilters" "filters.Add(new Filters.LoginAuthorize());"
