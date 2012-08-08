param($installPath, $toolsPath, $package)

Import-Module (Join-Path $toolsPath "Modules\DTETools.psm1")

##############################################################
# Get Solution and Path
##############################################################
$global:sln = [System.IO.Path]::GetFilename($dte.DTE.Solution.FullName)
$global:path = $dte.DTE.Solution.FullName.Replace($sln,'').Replace('\\','\')
$global:sln = Get-Interface $dte.Solution ([EnvDTE80.Solution2])