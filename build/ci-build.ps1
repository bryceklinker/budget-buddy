dotnet restore;

$testDirectories = Get-ChildItem -Path ".\test" | ? { $_.PSIsContainer } | Select-Object FullName;
Write-Host $testDirectories;

$hasError = $false;

pushd "src/Core"
foreach ($testDirectory in $testDirectories) {
	dotnet test $testDirectory.FullName
	if ($LASTEXITCODE -ne 0) {
		$hasError = $true;
	}
}
popd

if ($hasError) {
	exit 1;
}

dotnet publish src\Api\project.json -c Release;