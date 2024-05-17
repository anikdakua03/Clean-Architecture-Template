### Start : [Reference Link](https://www.youtube.com/watch?v=fhM0V2N1GpY&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k)

## Commands

```cs
dotnet new sln -o DinnerBooking

cd DinnerBooking

dotnet new webapi -n DinnerBooking.Api -controllers

dotnet new classlib -n DinnerBooking.Contracts

dotnet new classlib -n DinnerBooking.Infrastructure

dotnet new classlib -n DinnerBooking.Application

dotnet new classlib -n DinnerBooking.Domain
```

## Now also need to add these to our solution because we are adding via command line

```cs
dotnet sln add (ls -r **\*.csproj)
```

### Now adding the proper project reference according to our main **clean architecture** and **Domain Driven Design** Principle.

```cs
dotnet add .\DinnerBooking.Api\ reference .\DinnerBooking.Contracts\ .\DinnerBooking.Application\

dotnet add .\DinnerBooking.Infrastructure\ reference .\DinnerBooking.Application\

dotnet add .\DinnerBooking.Application\ reference .\DinnerBooking.Domain\

dotnet add .\DinnerBooking.Api\ reference .\DinnerBooking.Infrastructure\
```

#### For running
```cs
dotnet run --project .\DinnerBooking.Api\
```

### So when creating services and working dependencies , each layers may some have its own dependencies , so each layer should have its dependency injection file , where all dependencies are nicely injected.

1. So we are adding a static class where using the IServiceCollection services to add all dependencies for that layer.
For to use IService Collection we need `Microsoft.Extensions.DependencyInjection.Abstractions` nuget package.
```cs
dotnet add .\DinnerBooking.Application\ package Microsoft.Extensions.DependencyInjection.Abstractions
```

Then add that in our api main `program.cs` .
2. Same for our infrastructure layer.


### Break : 2 [Reference Link](https://www.youtube.com/watch?v=38bQNWKh0dk&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=2)

1. So now creating JWT token generator . To implement the method need claims , for that need this package `System.IdentityModel.Tokens.Jwt` .
```cs
dotnet add .\DinnerBooking.Infrastructure\ package System.IdentityModel.Tokens.Jwt
```

After implementing , add the dependencies to our DI class.

*** Note : While setting secret key , because of using HMACSHA256 algorithm which takes 256 bits of key , our secret key also must be 256 bits or greater.

2. Next instead of using DateTime everywhere needed, we will use it like a service like a service which gives us some time . In this way it will be consistent while getting the time for application irrespective of user time zone , or system  being used.

Now it has become a dependent service, so also need to add in our DI.

3. Now instead of using hardcoded values , we will use "Options Pattern" , which is basically reading value from our appsettings.json file.

Next to be available this configuration we need to add these configuration to our DI also.
SO to add with our DI , we need to add `Microsoft.Extensions.Options.ConfigurationExtensions` --> 
```cs
dotnet add .\DinnerBooking.Infrastructure\ package Microsoft.Extensions.Options.ConfigurationExtensions
```

4. Now instead of using credentials or important settings , we can use or should use dotnet secrets.
    a. To initialize secrets
    ```cs
    dotnet user-secrets init --project .\DinnerBooking.Api
    ```
    b. Next setting secrets as key value
    ```cs
    dotnet user-secrets set --project .\DinnerBooking.Api "JwtSettings:Secret" "super-secret-key-ghdyhduyiu-need-256-bits"
    ```
    c. Next to view the secrets
    ```cs
    dotnet user-secrets list --project .\DinnerBooking.Api
    ```

### Break : 3 [Reference Link](https://www.youtube.com/watch?v=ZwQf_JQUUCQ&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=3)

1. We are creating our `User` domain model.
2. Create IUserRepository for persisting user data in the application layer then implement the interface in the infrastructure layer. Then also add this services dependency in DI.


### Break : 4 [Reference Link](https://www.youtube.com/watch?v=gMwAhKddHYQ&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=4)

1. So handling exceptions in our applications is very critical what ever the reason is for ourself debugging , preventing sensitive information to be visible to the client.
 So , for this we can create our `custom middleware`.
- Approach - 1 : As normal
    - After implementing the middleware also add that to the application pipeline to be a part of the flow.
<br/>
- Approach - 2 : Using Filters
    - Now will  be using `ExceptionFilterAttribute` class into implement this.
    - Override the `OnException` method and implement.
    - This filter to be in action we need to add this to controller level as an attribute.
    - Or to avoid adding to every controller , instead can use program cs file to add there the filter.
<br/>
- Approach - 3 : Using Error models with specific errors with Problem Details object
    - So now we need to modify our error result as `ProblemDetails` class like.
    - In the problem details now we can specify Types [REFC] , Title, Status, etc.
    - But there may occur different types of exception which also have different types of REFC standard link , in this case we can use *approach - 4*.
<br/>
- Approach - 4 : Using Global Exception Handling using re-routing to error controller which handles all types of exception with proper Types ref link.
    - Now first adding the exception handler with the route in our pipeline like this 
    ```cs
    app.UseExceptionHandler("/error");
    ```
    - Add new Errors controller.
    - Get the exception from HttpContext itself using *IExceptionHandlerFeature* .
    - Then return as Problem() class , which automatically does the job of refactoring those error details with those basic details like types, title, code, detail.
    - So in real world scenario , we may not just return those defaults, we can customize or add our need .
    - To do it , we can use *ProblemDetailsFactory* to create/modify as per our need [ ref from official `DefaultProblemDetailsFactory` class]
    [ASP.NET Core Github](https://github.com/dotnet/aspnetcore)
    - Now what ever defaults we need to add can be added there.
    - Once added , we also need to override the default as we added on our own , so add into program cs file.


### Break : 4 [Reference Link](https://www.youtube.com/watch?v=tZ8gGqiq_IU&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=5)

1. Now here we will be just using the error routing and get appropiate errors. So not using exception middleware or exception filters.
2. Now there may happen exception related to some logic in our application , for that we can create separete exceptions that may happen to our application.
 But in this case if we need to switch between application exceptions then return appropiate error , it may become hectic for later part.
3. Because of there are many components dependent on each other , in any exception occur , we need to catch those exception using try catch in each component and also switch between different types. So it becomes difficult to track error flow.
 Also there can be two types of generic exception one is expected which we know and other is unknown exception. 
 So also to track this may become more heavy when we are following domain driven design.

A.  Now to overcome these problem, a different path we can follow , that *Results pattern*, which basically will return some result if it is good or success other wise will return some error object.

1.  Now also implement this result pattern in many ways using some popular packages .
 - Using *OneOf*
    - To use need to add :
    ```cs
    dotnet add .\DinnerBooking.Application\ package oneof
    ```
    - Next use this like :
    ```cs
    OneOf<AuthenticationResult, DuplicateEmailError> Register(string firstName, string lastName, string email, string password);
    ```

 - Using *FluentResults*
    - To use need to add :
    ```cs
    dotnet add .\DinnerBooking.Application\ package FluentResults
    ```
    - Next use this like :
    ```cs
    Result<AuthenticationResult> Register(string firstName, string lastName, string email, string password);
    ```

 - Using *ErrorOr*
    - To use need to add :
    ```cs
    dotnet add .\DinnerBooking.Domain\ package ErrorOr
    ```
    - Next use this like :
    ```cs
    Result<AuthenticationResult> Register(string firstName, string lastName, string email, string password);
    ```
    - So now will define some error in our domain layer , like which can happen in our application .
    - This approach returns a single error, or list of errors or success result which we can take benefit of.

2. Now creating a base controller which helps to define our errors or adds custom domain error as wanted.
    - Errors controller is ther eto catch exceptions then it goes to the api controller to make proper error responses and then returns.
    - If no errors then just returns success result.

### Break : 5 [Reference Link](https://www.youtube.com/watch?v=MwMVvLBSJa8&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=6)

*CQRS(Command Query Responsibility Segregation) & MediatR*
------------------------------------------------------------
1. Basic idea of CQRS is to segregate the type of request or operation we normally do, which are ->
    i. do something which is *Command* 
    ii. give something which is *Query*.

2. Now once we implemented the CQRS , now there may be many commands req or query req to controller, now to decide whether the req is a command or query it can be little tricky. So we can use *MediatR* which helps and send to appropiate service .
    - To add MediatR :
        - we need to add 
        ```cs
        dotnet add .\DinnerBooking.Application\ package MediatR
        ```
        - After implementing both Command or Query then Command or query handler inherited fro IRequest [to implement] method , we need to add this in our DI in such way they can be recognize in  a way for our application.
        - So to add this we need to add another package of MediatR like --
        ```cs
        dotnet add .\DinnerBooking.Application\ package MediatR.Extensions.Microsoft.DependencyInjection
        ```
        Then add like [ Mediatr starting version 12.0.0]-->
        ```cs
        services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        ```
        - Now we can check, how the flow happens.


### Break : 5 [Reference Link](https://www.youtube.com/watch?v=vBs6naPD6RE&list=PLzYkqgWkHPKBcDIP5gzLfASkQyTdy0t4k&index=7)

