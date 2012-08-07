##############################################################################
# Copyright (c) 2012
# Ulf Björklund
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
$baseProjectName = (Get-Project $Project).Name

##############################################################
# Add add references to base project
##############################################################

# add reference to core, data, and services
foreach ($ref in ($dataProjectName, $coreProjectName, $serviceProjectName)){
	Add-ProjectReferenceToProject $baseProjectName $ref
}

##############################################################
# Add App_Data folder to BaseProject
##############################################################

$App_Data = $path + $Project + "\App_Data"
if(!(Test-Path $App_Data)){
	Write-Host "Adding App_Data to" $Project
	(Get-Project $Project).ProjectItems.AddFolder("App_Data")
}

