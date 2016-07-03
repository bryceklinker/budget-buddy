dotnet restore;

$testDirectories = Get-ChildItem -Path ".\test" | ? { $_.PSIsContainer } | Select-Object FullName;
Write-Host $testDirectories;

$hasError = $false;
foreach ($testDirectory in $testDirectories) {
	dotnet test $testDirectory
	if ($LASTEXITCODE -ne 0) {
		$hasError = $true;
	}
}

if ($hasError) {
	exit;
}

$apiDirector = ".\"
dotnet publish ".\src\Api\project.json";