$budgetBuddyDir = [environment]::GetEnvironmentVariable('budget-buddy-dir');
$budgetBuddyServiceName = 'Budget-Buddy'

function Copy-Api
{
	pushd ".\src\Api\bin\Release\net461"

	Write-Host "Coping Aai files..."
	Copy-Item ".\*" $budgetBuddyDir
	Write-Host "Finished copying api files."

	popd
}

function Copy-Client 
{
	pushd ".\src\Web\dist"
	
	Write-Host "Copying client files..."
	Copy-Item ".\*" "$budgetBuddyDir/wwwroot"
	Write-Host "Finished copying client files."
		
	popd
}

function Ensure-Service-Exists 
{
	if(Get-Service -Name $budgetBuddyServiceName -ErrorAction SilentlyContinue) {
		Write-Host "Creating $budgetBuddyServiceName..."
		New-Service -Name $budgetBuddyServiceName -BinaryPathName "$budgetBuddyDir\api.exe" -StartupType Automatic
		Write-Host "Finished creating $budgetBuddyServiceName."
	}
}

function Stop-Service
{
	Ensure-Service-Exists

	Write-Host "Stopping service..."
	Stop-Service $budgetBuddyServiceName
	Write-Host "Finished stopping service."
}

function Start-Service
{
	Ensure-Service-Exists

	Write-Host "Starting service..."
	Start-Service $budgetBuddyServiceName
	Write-Host "Finished starting service."
}

Stop-Service
Copy-Api
Copy-Client
Start-Service