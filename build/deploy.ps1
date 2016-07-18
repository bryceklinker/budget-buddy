$budgetBuddyDir = [environment]::GetEnvironmentVariable('budget-buddy-dir');
$budgetBuddyServiceName = 'Budget-Buddy'

function Copy-Api
{
	pushd ".\src\Api\bin\Release\net461\win7-x64\publish"

	Write-Host "Coping api files..."
	Copy-Item ".\*" $budgetBuddyDir -Recurse
	Write-Host "Finished copying api files."

	popd
}

function Copy-Client 
{
	pushd ".\src\Web\dist"
	
	Write-Host "Copying client files..."
	Copy-Item ".\*" "$budgetBuddyDir\wwwroot" -Recurse
	Write-Host "Finished copying client files."
		
	popd
}

function Ensure-Budget-Buddy-Exists 
{
	if(Get-Service -Name $budgetBuddyServiceName -ErrorAction SilentlyContinue) {
		return;
	}

	Write-Host "Creating $budgetBuddyServiceName..."
	New-Service -Name $budgetBuddyServiceName -BinaryPathName "$budgetBuddyDir\Api.exe" -StartupType Automatic
	Write-Host "Finished creating $budgetBuddyServiceName."
}

function Stop-Budget-Buddy
{
	Ensure-Budget-Buddy-Exists

	Write-Host "Stopping service..."
	Stop-Service -Name $budgetBuddyServiceName
	Write-Host "Finished stopping service."
}

function Start-Budget-Buddy
{
	Ensure-Budget-Buddy-Exists

	Write-Host "Starting service..."
	Start-Service -Name $budgetBuddyServiceName
	Write-Host "Finished starting service."
}

Stop-Budget-Buddy
Copy-Api
Copy-Client
Start-Budget-Buddy