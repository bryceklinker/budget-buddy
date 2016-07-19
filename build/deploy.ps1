$budgetBuddyDir = [environment]::GetEnvironmentVariable('budget-buddy-dir');

function Copy-Api
{
	pushd ".\src\Api\bin\Release\net461\win7-x64\publish"

	Write-Host "Coping api files..."
	Copy-Item ".\*" $budgetBuddyDir -Recurse -Force
	Write-Host "Finished copying api files."

	popd
}


Copy-Api