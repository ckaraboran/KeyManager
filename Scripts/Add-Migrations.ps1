[string]$MigrationName = $null
while ([string]::IsNullOrWhitespace($MigrationName)) {
	$MigrationName = Read-Host "Migration name"
}

dotnet ef migrations add $MigrationName --project ../Libraries/KeyManager.Infrastructure -s ../Presentation/KeyManager.Api/KeyManager.Api.csproj

dotnet ef database update --project ../Libraries/KeyManager.Infrastructure -s ../Presentation/KeyManager.Api/KeyManager.Api.csproj