[T4Scaffolding.Scaffolder()][CmdletBinding()]
param(     
	[parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true)][string]$TemplateType, 
    [string]$Project,
	[string]$CodeLanguage,
	[string[]]$TemplateFolders,
	[switch]$Force = $false
)

$namespaces = $DTE.Documents | ForEach{$_.ProjectItem.FileCodeModel.CodeElements | Where-Object{$_.Kind -eq 5}}
	
$classes = $namespaces | ForEach{$_.Children}

$classes | ForEach{
	$current = $_
	$_.Bases | ForEach{
		if($_.Name -eq "PersistentEntity"){
			Scaffold WC.Controller.For $current.Name -TemplateType $TemplateType -Force:$Force
		}
	}		
}

scaffold WC.Ninject