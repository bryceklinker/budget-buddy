dotnet restore;

$testDirectories = Get-ChildItem | ? { $_.PSIsContainer } | Select-Object FullName;

$hasError = false;
foreach ($testDirectory in $testDirectories) {
	dotnet test $testDirectory
	if ($LASTEXITCODE -ne 0) {
		$hasError = true;
	}
}

if ($hasError) {
	exit;
}

$apiDirector = ".\"
dotnet publish ".\src\Api\project.json";