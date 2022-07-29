namespace TauCode.Cli;

public class App : IApp
{
    #region Fields

    private readonly Dictionary<string, IModule> _modules;
    private IModule? _namelessModule;

    #endregion

    #region ctor

    public App(string name)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (!CliHelper.IsValidTerm(name))
        {
            throw new ArgumentException($"'{name}' is not a valid app name.", nameof(name));
        }

        this.Name = name;
        _modules = new Dictionary<string, IModule>();
    }

    #endregion

    #region IApp Members

    public string Name { get; }

    public IReadOnlyList<IModule> Modules
    {
        get
        {
            var list = _modules.Values.ToList();

            if (_namelessModule != null)
            {
                list.Add(_namelessModule);
            }

            return list;
        }
    }

    public bool Contains(IModule module)
    {
        if (module == null)
        {
            throw new ArgumentNullException(nameof(module));
        }

        if (module == _namelessModule)
        {
            return true;
        }

        if (_modules.Values.Contains(module)) // todo: performance
        {
            return true;
        }

        return false;
    }

    public void AddModule(IModule module)
    {
        if (module == null)
        {
            throw new ArgumentNullException(nameof(module));
        }

        if (!(module is Module))
        {
            throw new ArgumentException(
                $"'{nameof(module)}' must be an instance of '{typeof(Module).FullName}'.");
        }


        if (module.Name == null)
        {
            if (_namelessModule == null)
            {
                if (_modules.Count > 0)
                {
                    throw new InvalidOperationException("Cannot add nameless module if a named module was added previously.");
                }

                _namelessModule = module;
            }
            else
            {
                throw new InvalidOperationException("Nameless module already added.");
            }
        }
        else
        {
            if (_namelessModule != null)
            {
                throw new InvalidOperationException("Cannot add named module if a nameless module was added previously.");
            }

            if (_modules.ContainsKey(module.Name))
            {
                throw new ArgumentException($"Module with name '{module.Name}' already added.", nameof(module));
            }

            _modules.Add(module.Name, module);
        }
    }

    public IModule? GetModule(string? moduleName)
    {
        return moduleName == null ? _namelessModule : _modules.GetValueOrDefault(moduleName);
    }

    #endregion

    #region Overridden

    public override string? ToString() => this.Name;

    #endregion
}