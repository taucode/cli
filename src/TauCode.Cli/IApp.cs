using System.Collections.Generic;

namespace TauCode.Cli
{
    public interface IApp
    {
        string Name { get; }

        IReadOnlyList<IModule> Modules { get; }

        void AddModule(IModule module);

        IModule GetModule(string moduleName);
    }
}
