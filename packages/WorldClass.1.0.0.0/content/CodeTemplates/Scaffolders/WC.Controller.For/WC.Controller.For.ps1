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
[T4Scaffolding.Scaffolder(Description = "Enter a description of CodePlanner.MVC.JSONController.For here")][CmdletBinding()]
param(
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$ModelType,
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$TemplateType,
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

##############################################################
# Create the JSON Controller in the MvcApp
##############################################################
# Ensure we have a controller name, plus a model type if specified
$foundModelType = Get-ProjectType $ModelType -Project $mvcProjectName
if (!$foundModelType) { return }

$outputPath = "Controllers\" + $ModelType + "Controller"
$ximports = $coreProjectName + ".Model," + $coreProjectName + ".Interfaces.Service" 

Write-Host Creating new $($ModelType)Controller -ForegroundColor DarkGreen
Add-ProjectItemViaTemplate $outputPath -Template Controller `
	-Model @{ 	
	Namespace = $mvcProjectName; 
	DataType = [MarshalByRefObject]$foundModelType;	
	ExtraUsings = $ximports;
	} `
	-SuccessMessage "Added Controller of $ModelType output at {0}" `
	-TemplateFolders $TemplateFolders -Project $mvcProjectName -CodeLanguage $CodeLanguage -Force:$Force

scaffold WC.Service.For $ModelType
scaffold WC.Repository.For $ModelType
scaffold WC.Views.For $ModelType


#Scaffold WC.Pages -ModelType $ModelType -TemplateType $TemplateType
