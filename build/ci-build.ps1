dotnet restore;

$testDirectories = Get-ChildItem -Path ".\test" | ? { $_.PSIsContainer } | Select-Object FullName;
Write-Host $testDirectories;

$hasError = $false;

Set-Location "src/Api";
foreach ($testDirectory in $testDirectories) {
	dotnet test $testDirectory.FullName
	if ($LASTEXITCODE -ne 0) {
		$hasError = $true;
	}
}
Set-Location "../..";

if ($hasError) {
	exit 1;
}

dotnet publish src\Api\project.json;