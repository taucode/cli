namespace TauCode.Cli;

public interface IApp : IDisposable
{
    string Name { get; }

    IReadOnlyList<IModule> Modules { get; }

    bool Contains(IModule module);

    void AddModule(IModule module);

    IModule? GetModule(string? moduleName);
}