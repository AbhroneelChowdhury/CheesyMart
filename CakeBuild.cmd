cd .\Build
dotnet tool restore
dotnet cake --target=Build
dotnet cake --target=CheesyMartAPI --verbosity=diagnostic
cd ..\
pause