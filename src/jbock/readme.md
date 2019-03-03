# Cross-Platform C# Using .NET Core and WebAssembly - Demos From Jason
This directory contains demos that I (Jason) do during the workshop. Hopefully they work! Here are all my notes on what these demos are and how they work.

## Project Descriptions
Following are descriptions of each project and what they do.

### Collatz.Core

This is a .NET Standard 2.0 project that contains a simple implementation of the [Collatz Conjecture](https://en.wikipedia.org/wiki/Collatz_conjecture). This project is used by all the other projects with different UI flavors.

### Collatz.Core.Tests

This is a .NET Core 2.1 project that has some tests for `Collatz.Core`. [NUnit](http://nunit.org/) is used as the testing framework.

### Collatz.API
This is an ASP.NET Core 2.1 project that hosts a REST API. It uses:
*[Swagger](https://swagger.io/) to document and run the APIs. 
*[Autofac](https://autofac.org/) for dependency injection
*[Serilog](https://serilog.net/) for logging

### Collatz.API.Tests
This contains controller tests for `Collatz.API`.

### Collatz.API.Client
This is a console application that calls the endpoint in `Collatz.API`. It uses [Polly](https://github.com/App-vNext/Polly) for resilience.

### Collatz.WebAssembly
This is an ASP.NET Core app that hosts a `collatz.wasm` file.

### Collatz.BlazorClient.Server
This contains the server-side Blazor support if the client decides to run "server-side".

### Collatz.BlazorClient.App
This contains a Blazor-based client for Collatz. It also uses Bing maps and [ChartJS](https://github.com/mariusmuntean/ChartJs.Blazor) for graphing.

### Collatz.AvaloniaClient
This is an [Avalonia](https://github.com/AvaloniaUI/Avalonia)-based client for Collatz.

### Collatz.GuiClient
This is a [gui.cs](https://github.com/migueldeicaza/gui.cs)-based client for Collatz.

## Demos
Here's a brief run-down of the demos and what they do. Note, if you want a long sequence from a small number, use 27.

### Hello World Console Application
When I do the first demo, it's not in this repository. I just do a simple "hello world" .NET Core application. This is done via "dotnet new console" in a directory off of jbock (probably called "VSLiveLasVegas2019" or something like that). Then "dotnet run" is done to run the app. Sometimes, switching between Ubuntu (via WSL) and Windows requires a "dotnet restore".

I also demo my [EditorConfigGenerator](https://github.com/JasonBock/EditorConfigGenerator) tool. This is done by:
```
dotnet tool install -g EditorConfigGenerator
```
To run it:
```
EditorConfigGenerator {directory goes here}
```
### Running Tests
You should be able to run tests in Visual Studio. You can also go to the root directory (where the Collatz.sln file exists) and run ["dotnet test"](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-test). This will run the two test projects.

### Compatiability
I use my [DynamixProxies](https://github.com/jasonbock/dynamicproxies) library to see what it would take to upgrade it.

### API
`dotnet run` in the Collatz.API directory. You can hit the Swagger page at http://localhost:5000/swagger/index.html and see the log messages in the console window, or run the Collatz.API.Client project.

### WebAssembly
`dotnet run` in the Collatz.WebAssembly directory. Then go into a browser, and visit `http://localhost:64370/collatz.html` or http://localhost:64370/collatzWithCallback.html`.

### Blazor
Run Collatz.BlazorClient.Server. If you want it all in the client, change the blazor JavaScript file reference in index.html, otherwise, use the other one for server-side usage (you'll see the two lines, should be clear which one should be uncommented). This is hosted on `http://localhost:61739/`.

### FlightFinder
This is a demo app from Microsoft, you can find it [here](https://github.com/aspnet/samples/tree/master/samples/aspnetcore/blazor/FlightFinder). Another demo that I don't show but is very good is [Blazing Pizza](https://github.com/dotnet-presentations/blazor-workshop).

### Avalonia
Run 

### gui.cs
This is a fun one. Just run it, and you'll get what looks like an old-school terminal UI.