# FirebaseAdminSdkForDotnetUnofficial
Firebase Admin Sdk For Dotnet (C#) Unofficial

Firebase Admin SDK supports Node.js, Java, Phyton, Go. 
But i needed Firebase .Net SDK for C#.net Core.

How to add it to the project ?

Open GoogleOAuthService.cs file. You will see 3 Methods;

GetToken()

SetUserClaims(string accessToken, string userId, int roleId)

CreateUser(string accessToken, string email, string password)

First, you need to Google auth sdk for get auth token. You can install it with nuget package manager. 
Package name is "Google.Apis.Auth"

After install it, you must add "serviceAccountKey.json" to your project.
You can find it on your Firebase Console > Your Project > Settings > Service Accounts Tab 
And click "Create private key" button and download the json file. And add json file to your project.

How To Use:

var service = new GoogleOAuthService();

var accessToken = service.GetToken().Result;

service.SetUserClaims(accessToken, user.Id, user.RoleId);

var id = service.CreateUser(accessToken, "yunuskaygun@gmail.com", "123456");


Note1: If you need to more methods like "listAllUsers", "deleteUser" you can use firebase rest api endpoinds and parameters.

Note2: If you need to more scopes, add scopes to credential array where GetToken() method.


