#!/usr/bin/env bash

Help()
{
   # Display Help
   echo "This script adds a new migration to the project."
   echo
   echo "Syntax: add-migration MigrationName"
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

dotnet ef migrations add "$MigrationName" --project ../Libraries/KeyManager.Infrastructure -s ../Presentation/KeyManager.Api/KeyManager.Api.csproj
echo "Migration has been added"
