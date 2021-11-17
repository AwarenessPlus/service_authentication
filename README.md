# service_authenticatión
This API REST service made in .NET CORE manage the authentication services for the web app. This app allows to make a login or signup if it's required

The service exposes the followings endpoins: 

## Endpoints

###  Ping

 > PING /api/authentications/ping

This endpoint allows the client proof if the service are working goood.


###  Log In 
 > POST /api/authentications/auth

This endpoint allow to user to access to the web app

### Sign Up
 > POST /api/authentications/signup

This endpoint allow to the user to create a new account in the system

### Delete Account
 > DELETE /api/authentications/{username}

This endpoint allow to the user to delete his account from the system

### Update Password
 > PUT /api/authentications/{username}

This endpoint allows to the user to change or update his password

# Installation 

## Enviroment
you need to downlowad or check the following things depending on your development environment

### Visual Studio
- Visual Studio 2019 with the ASP.NET and web development workload.


### Visual Studio Code

- Visual Studio Code
- C# for Visual Studio
- .NET 5.0 SDK

### Visual Studio for Mac

- For Visual Studio for Mac .NET 5.0.

## On visual studio 

### NuGet

You need install the following NuGets for work properly: 

* Microsoft.EntityFrameworkCore [from Microsoft](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
* System.IdentityModel.Tokens.JWT [from .NET](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/)
* Microsoft.AspNetCore.Authentication.JwtBearer [from ASP.NET Core](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/)
* Swashbuckle.AspNetCore [from domaindrivendev ](https://www.nuget.org/packages/Swashbuckle.AspNetCore/5.6.3?_src=template)
* System.Drawing.Common [ from Microsoft ](https://www.nuget.org/packages/System.Drawing.Common/5.0.2?_src=template )


## For documentation 


The service  include Swagger which allows you to view and test each endpoint. [Swagger]( https://swagger.io/) 

