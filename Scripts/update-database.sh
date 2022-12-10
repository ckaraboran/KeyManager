#!/usr/bin/env bash

dotnet ef database update --project ../Libraries/KeyManager.Infrastructure -s ../Presentation/KeyManager.Api/KeyManager.Api.csproj
echo "Database has been updated"