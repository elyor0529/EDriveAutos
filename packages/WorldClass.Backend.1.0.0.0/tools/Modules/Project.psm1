function Add-AssemblyReferenceToProject($project, $assembly){
	(Get-Project $project).Object.References.Add($assembly)
}

function Add-ProjectReferenceToProject($project, $reference){
	(Get-Project $project).Object.References.AddProject((Get-Project $reference))
}

function Has-Package($project, $package){
	return (get-package | Select-Object -ExpandProperty ID) -contains $package;
}

function Add-ClassLibraryToSolution($projectName, $path, $sln){	
	if(($DTE.Solution.Projects | Select-Object -ExpandProperty Name) -notcontains $projectName){
		Write-Host "Adding new project - " $projectName
		$templatePath = $sln.GetProjectTemplate("ClassLibrary.zip","CSharp")						
		$sln.AddFromTemplate($templatePath, $path+$projectName,$projectName)
		$file = Get-ProjectItem "Class1.cs" -Project $projectName
		$file.Remove()	
		$testPath = $path + $projectName + "\Class1.cs"
		Write-Host $testPath
		if(Test-Path $testPath){
			Write-Host "Remove file..."
			Remove-Item $testPath
		}
	}	
}

Export-ModuleMember Add-ProjectReferenceToProject
Export-ModuleMember Add-AssemblyReferenceToProject
Export-ModuleMember Has-Package
Export-ModuleMember Add-ClassLibraryToSolution
