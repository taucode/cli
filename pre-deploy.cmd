dotnet restore

dotnet build TauCode.Cli.sln -c Debug
dotnet build TauCode.Cli.sln -c Release

dotnet test TauCode.Cli.sln -c Debug
dotnet test TauCode.Cli.sln -c Release

nuget pack nuget\TauCode.Cli.nuspec