using TauCode.Cli.Executors;

namespace TauCode.Cli;

public abstract class Module : IModule
{
    #region Fields

    private IExecutor? _namelessExecutor;
    private readonly Dictionary<string, IExecutor> _executors;
    private FallbackExecutor? _fallbackExecutor;

    #endregion

    #region ctor

    protected Module(string? name)
    {
        if (name != null)
        {
            if (!CliHelper.IsValidTerm(name))
            {
                throw new ArgumentException($"'{name}' is not a valid module name.", nameof(name));
            }
        }

        this.Name = name;

        _executors = new Dictionary<string, IExecutor>();
    }

    #endregion

    #region Protected

    protected virtual void DisposeImpl()
    {
        // idle
    }

    #endregion

    #region Overridde

    public override string? ToString() => this.Name;

    #endregion

    #region IModule Members

    public string? Name { get; }

    public IReadOnlyList<IExecutor> Executors
    {
        get
        {
            var list = _executors.Values.ToList();
            if (_namelessExecutor != null)
            {
                list.Add(_namelessExecutor);
            }

            if (_fallbackExecutor != null)
            {
                list.Add(_fallbackExecutor);
            }

            return list;
        }
    }

    public bool Contains(IExecutor executor)
    {
        if (executor == null)
        {
            throw new ArgumentNullException(nameof(executor));
        }

        if (_namelessExecutor == executor || _fallbackExecutor == executor)
        {
            return true;
        }

        return _executors.ContainsValue(executor);
    }

    public void AddExecutor(IExecutor executor)
    {
        if (executor == null)
        {
            throw new ArgumentNullException(nameof(executor));
        }

        if (executor is FallbackExecutor fallbackExecutor)
        {
            if (_fallbackExecutor != null)
            {
                throw new ArgumentException("Module already has fallback executor.", nameof(executor));
            }

            _fallbackExecutor = fallbackExecutor;
            return;
        }

        if (executor.Name == null)
        {
            if (_namelessExecutor != null)
            {
                throw new ArgumentException("Module already has nameless executor.", nameof(executor));
            }

            _namelessExecutor = executor;
        }
        else
        {
            if (_namelessExecutor != null)
            {
                throw new InvalidOperationException("Cannot add named executor if a nameless executor was added previously.");
            }

            if (_executors.ContainsKey(executor.Name))
            {
                throw new ArgumentException($"Executor with name '{executor.Name}' already added.", nameof(executor));
            }

            _executors.Add(executor.Name, executor);
        }
    }

    public IExecutor? GetExecutor(string? executorName)
    {
        if (executorName == null)
        {
            return _namelessExecutor;
        }

        var gotNamedExecutor = _executors.TryGetValue(executorName, out var executor);
        if (gotNamedExecutor)
        {
            return executor;
        }

        if (_fallbackExecutor != null)
        {
            if (_fallbackExecutor.AcceptsName(executorName))
            {
                return _fallbackExecutor;
            }
        }

        return null;
    }

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
        _namelessExecutor?.Dispose();

        foreach (var executor in _executors.Values)
        {
            executor.Dispose();
        }

        _fallbackExecutor?.Dispose();

        this.DisposeImpl();
    }

    #endregion
}
