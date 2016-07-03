dotnet restore;

$testDirectories = Get-ChildItem | ? { $_.PSIsContainer } | Select-Object FullName;

$hasError = False;
foreach ($testDirectory in $testDirectories) {
	dotnet test $testDirectory
	if ($LASTEXITCODE -ne 0) {
		$hasError = True;
	}
}

if ($hasError) {
	exit;
}

$apiDirector = ".\"
dotnet publish ".\src\Api\project.json";