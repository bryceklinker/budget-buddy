$budgetBuddyDir = [environment]::GetEnvironmentVariable('budget-buddy-dir');
$budgetBuddyServiceName = 'Budget-Buddy'

function Copy-Client
{
	pushd "./src/Api/bin/Release/net461"

	Copy-Item "./*" $budgetBuddyDir

	popd
}

function Copy-Api 
{
	pushd "./src/Web/dist"
	
	Copy-Item "./*" "$budgetBuddyDir/wwwroot"
		
	popd
}

function Ensure-Service-Exists 
{
	if(Get-Service -Name $budgetBuddyServiceName -ErrorAction SilentlyContinue) {
		New-Service -Name $budgetBuddyServiceName -BinaryPathName "$budgetBuddyDir\api.exe" -StartupType Automatic
	}
}

function Stop-Service
{
	Ensure-Service-Exists

	Stop-Service $budgetBuddyServiceName
}

function Start-Service
{
	Ensure-Service-Exists

	Start-Service $budgetBuddyServiceName
}

Stop-Service
Copy-Api
Copy-Client
Start-Service