# Key Manager

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=ckaraboran_KeyManager)
[![Sonar Cube Static Analysis](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=ncloc)](https://sonarcloud.io/dashboard?id=ckaraboran_KeyManager)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=coverage)](https://sonarcloud.io/summary/new_code?id=ckaraboran_KeyManager)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=ckaraboran_KeyManager)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=ckaraboran_KeyManager)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=ckaraboran_KeyManager)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=ckaraboran_KeyManager)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=bugs)](https://sonarcloud.io/summary/new_code?id=ckaraboran_KeyManager)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=ckaraboran_KeyManager)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=ckaraboran_KeyManager&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=ckaraboran_KeyManager)
[![Build](https://github.com/ckaraboran/KeyManager/actions/workflows/build.yml/badge.svg)](https://github.com/ckaraboran/KeyManager/actions/workflows/build.yml)

## About The Project

The project is assessment project for Clay Solutions.

### Built With

You can find the used technologies below.

<p>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/dot-net/dot-net-plain-wordmark.svg" /></a>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/csharp/csharp-plain.svg" /></a>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/git/git-plain-wordmark.svg" /></a>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/github/github-original-wordmark.svg" /></a>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/jetbrains/jetbrains-original.svg" /></a>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/nuget/nuget-original.svg" /></a>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/sqlite/sqlite-original-wordmark.svg" /></a>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://cdn.jsdelivr.net/gh/devicons/devicon/icons/visualstudio/visualstudio-plain-wordmark.svg" /></a>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://www.svgrepo.com/show/354201/postman.svg" /></a>
	<a href="#"><img height="30" width="30" style="float:left; margin-right: 10px;" src="https://static-00.iconduck.com/assets.00/swagger-icon-256x256-c63r3xzo.png" /></a>
	<a href="#"><img height="30" width="100" style="float:left; margin-right: 10px;" src="https://automapper.org/images/black_logo.png" /></a>
</p>
<br /><br />

## Getting Started

### Prerequisites
* Jetbrains Rider or Visual Studio should be installed in order to reach the code. (https://visualstudio.microsoft.com/tr/downloads/). I used Jetbrains Rider to develop the application.
* .NET 6 (or 7) SDK and .NET Core Runtime should be installed. SDK is for development purposes and runtime is for running the project. (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* Postman should be installed for the collections. (https://www.postman.com/downloads/) Requests are ready to use in collections so you can use it in order to test the API.

### Installation
You can find two ways to start the project below.

First way;
1. Clone the repo
```sh
 git clone https://github.com/ckaraboran/KeyManager
 ```
2. Go to the project folder
```sh
 cd KeyManager/Presentation/KeyManager.Api
 ```
3. Run the project
```sh
 C:\Projects\Clay\KeyManager.Api>dotnet run 
 ```

Second way;

1. Open the solution with Visual Studio or Jetbrains Rider
2. Clone the repo
3. Run the project

## Usage

I created a test collection for Postman. You can find the collection in the **postman** folder. You can import the collection to Postman and use it for testing the API.

Swagger is also available for the API. You can find the swagger page in the following link. (https://localhost:5001/swagger/index.html)

I manually tested the application with these steps:

* Login as OfficeManager (Username: OfficeManager, Password: OfficeManager)
* Change OfficeManager Password to 12345
* Log out
* Login as OfficeManager (Username: OfficeManager, Password: 12345)
* Create a door: Door 1
* Create a door: Door 2
* Create a door: Door 3
* FAIL: Try to create a door: Door 2
* Delete Door 2 (id 2)
* FAIL: Delete Door 2 (id 2)
* Create a door: Door 2 (id 4)
* List Doors
* FAIL: Update door name of Door 2 (id 4) to Door 3
* Update door name of Door 2 (id 4) to Door 2A
* Create a user named ckaraboran
* Create a user named johndoe
* Create a user named janedoe
* Delete user janedoe (id 6)
* FAIL: Delete user janedoe (id 6)
* Create permission for johndoe (id 5) to Door 3 (id 3)
* Create permission for ckaraboran (id 4) to Door 2A (id 4)
* List users
* List current user roles
* Get ckaraboran by user Id (id 4)
* Create a role named OfficeBoy
* Create user role for ckaraboran(id 4) as OfficeUser(id 3)
* FAIL: Try to change predefined OfficeUser role (id 4) to OfficeAnotherUser
* FAIL: Try to open Door 2A (id 4) without any permission
* Log out
* Login as Director (Password: Director)
* Open incidents
* FAIL: Try to create Door 6
* FAIL: Try to update a door (id 4)
* FAIL: Try to delete a door (id 4)
* FAIL: Try to get permissions
* FAIL: Try to create, delete permissions
* Get roles
* FAIL: Try to create, update, delete roles
* FAIL: Try to get, create, update, delete users
* Update password. Make it director1
* Log out director
* Login Director (Password: director1)
* Try to get, create, delete user roles
* Logout
* Login as ckaraboran (Password: ckaraboran)
* Get door list
* Open Door 2A (id 4)
* Log out
* Login as johndoe
* FAIL: Try to get door list
* Open Door 3 (id 3)
* Log out
* Login as Director (Password director1)
* Open incidents
* Log out