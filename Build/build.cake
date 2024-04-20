#addin nuget:?package=Cake.CodeGen.NSwag&version=1.2.0&loaddependencies=true
#addin nuget:?package=Cake.FileHelpers&version=7.0.0

var target = Argument("target", "Test.Unit");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    DotNetClean($"../CheesyMart.sln");
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("../CheesyMart.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});



Task("CheesyMartAPI")
    .IsDependentOn("CheesyMartAPI.Angular");
    
Task("CheesyMartAPI.Swagger")
    .IsDependentOn("Build")
    .Does(() => 
{ 
    //Generate swagger.json.
    using(var process = StartAndReturnProcess("c:/program files/dotnet/dotnet.exe", 
        new ProcessSettings{
            Arguments = "swagger tofile --serializeasv2 --output ../CheesyMart.API/bin/Debug/net8.0/swagger.json ../CheesyMart.API/bin/Debug/net8.0/CheesyMart.API.dll v1" 
        }))
    {
        process.WaitForExit();
        // This should output 0 as valid arguments supplied
        Information("Exit code: {0}", process.GetExitCode());
    }
});

Task("CheesyMartAPI.Angular")
    .IsDependentOn("CheesyMartAPI.Swagger")
    .Does(() => 
{
    NSwag.FromJsonSpecification("../CheesyMart.API/bin/debug/net8.0/swagger.json")
    .GenerateTypeScriptClient("../CheesyMart.Portal/cheesy-mart/src/app/services/cheesy-client.service.ts", new TypeScriptClientGeneratorSettings() {
       BaseUrlTokenName = "CHEESEYMART_API_BASE_URL",
       ClassName = "{controller}Client",
       Template = TypeScriptTemplate.Angular,
       InjectionTokenType = InjectionTokenType.InjectionToken,
       RxJsVersion = 7.0M
    }); 
});

Task("CheesyMartAPI.Angular.NSwagConfig")
    .IsDependentOn("Build")
    .Does(() =>
{
    StartProcess("nswag", new ProcessSettings {
        WorkingDirectory = "../ClientGeneration/CheesyMartAPI",
		Arguments = "run"
    });
});


Task("Test.Unit")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest("../CheesyMart.Test.Unit/CheesyMart.Test.Unit.csproj", new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});    
   

   
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);