function Exit-On-Error
{
	if ($LASTEXITCODE -ne 0) {
		exit $LASTEXITCODE;
	}
}

function Build-Api 
{
	dotnet restore;

	$testDirectories = Get-ChildItem -Path ".\test" | ? { $_.PSIsContainer } | Select-Object FullName;
	Write-Host $testDirectories;

	pushd "src/Api"
	foreach ($testDirectory in $testDirectories) {
		dotnet test $testDirectory.FullName
		Exit-On-Error;
	}
	popd
}

function Build-Client
{
	pushd "src/Web";

	npm install;
	Exit-On-Error;

	npm run test:single;
	Exit-On-Error;

	npm run build;
	Exit-On-Error;

	popd;
}

function Package-App 
{
	if (Test-Path "./src/Api/wwwroot") {
		Remove-Item -Path "./src/Api/wwwroot" -Force -Recurse;
	}
	
	Copy-Item -Path "./src/Web/dist" -Destination "./src/Api/wwwroot" -Recurse;
	dotnet publish src\Api\project.json -c Release;

	Remove-Item -Path "./src/Api/wwwroot" -Force -Recurse;
}

Build-Api;
Exit-On-Error;

Build-Client;
Exit-On-Error;

Package-App;