dotnet restore;

$testDirectories = Get-ChildItem -Path ".\test" | ? { $_.PSIsContainer } | Select-Object FullName;
Write-Host $testDirectories;

$hasError = $false;

pushd "src/Api"
foreach ($testDirectory in $testDirectories) {
	dotnet test $testDirectory.FullName
	if ($LASTEXITCODE -ne 0) {
		$hasError = $true;
	}
}
popd

pushd "src/Web"

npm install;

npm run test:single;

npm run build;

popd

if ($hasError) {
	exit 1;
}

dotnet publish src\Api\project.json -c Release;