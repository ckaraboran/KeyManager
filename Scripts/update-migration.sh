#!/usr/bin/env bash

Help()
{
   # Display Help
   echo "This script updates a migration of the project."
   echo
   echo "Syntax: update-migration MigrationName"
   echo
}

while getopts ":h" option; do
   case $option in
      h) # display Help
         Help
         exit;;
     \?) # incorrect option
         echo "Error: Invalid option"
         exit;;
   esac
done

MigrationName=${1?Error: no migration name given}

dotnet ef migrations update "$MigrationName" --project ../Libraries/KeyManager.Infrastructure -s ../Presentation/KeyManager.Api/KeyManager.Api.csproj
echo "Migration has been updated"
