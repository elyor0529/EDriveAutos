##############################################################################
# Copyright (c) 2012 
# Ulf Bj?rklund
# http://average-uffe.blogspot.com/
# http://twitter.com/codeplanner
# http://twitter.com/ulfbjo
#
# Permission is hereby granted, free of charge, to any person obtaining
# a copy of this software and associated documentation files (the
# "Software"), to deal in the Software without restriction, including
# without limitation the rights to use, copy, modify, merge, publish,
# distribute, sublicense, and/or sell copies of the Software, and to
# permit persons to whom the Software is furnished to do so, subject to
# the following conditions:
#
# The above copyright notice and this permission notice shall be
# included in all copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
# EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
# MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
# NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
# LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
# OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
# WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
##############################################################################
[T4Scaffolding.Scaffolder(Description = "Enter a description of CodePlanner.MVC.SinglePage.For here")][CmdletBinding()]
param(     
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,   
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

Add-NamespacesToHost $Project

##############################################################
# Info about ModelType
##############################################################
$foundModelType = Get-ProjectType $ModelType -Project $coreProjectName
if (!$foundModelType) { return }
$primaryKeyName = [string](Get-PrimaryKey $foundModelType.FullName -Project $coreProjectName)

# Get the related entities
if ($foundModelType) { $classRelations = [Array](Get-RelatedEntities $ModelType -Project $coreProjectName) }
if (!$classRelations ) { $classRelations = @() }

#Get ManyToManyRelations
$manyToManyRelations = @()
Get-RelatedEntities $ModelType | Where-Object{$_.RelationType -eq "Child"} | ForEach{	 
		$NN = $_.RelatedEntityType.Name
		Get-RelatedEntities $_.RelatedEntityType.Name | 
		Where-Object{$_.RelationType -eq "Child" -and $_.RelatedEntityType.Name -eq $ModelType} | ForEach{$manyToManyRelations += $NN}
}

##############################################################
# Create the index, create View in the MvcApp
##############################################################

foreach ($view in @("Index", "Details", "Create", "Edit")){
	$outputPath = "Views\" + $ModelType + "\" + $view
	Write-Host Creating new $ModelType View $_ -ForegroundColor DarkGreen
	Add-ProjectItemViaTemplate $outputPath -Template $view `
	-Model @{ 	
		Namespace = $namespace; 
		PrimaryKeyName = $primaryKeyName; 
		DataType = [MarshalByRefObject]$foundModelType;	
		DataTypeName = $foundModelType.Name; 
		RelatedEntities = $classRelations; 
		ManyToManyEntities = $manyToManyRelations;
	} `
	-SuccessMessage "Added view $view to $ModelType" `
	-TemplateFolders $TemplateFolders -Project $mvcProjectName -CodeLanguage $CodeLanguage -Force:$Force `
}