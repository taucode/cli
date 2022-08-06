dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\test\TauCode.Cli.Tests\TauCode.Cli.Tests.csproj
dotnet test -c Release .\test\TauCode.Cli.Tests\TauCode.Cli.Tests.csproj

nuget pack nuget\TauCode.Cli.nuspec