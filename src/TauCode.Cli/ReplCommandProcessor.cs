namespace TauCode.Cli;

public abstract class ReplCommandProcessor
{
    private readonly HashSet<string> _keys;

    protected ReplCommandProcessor(
        IReplHost host,
        string[] keys,
        string description)
    {
        this.Host = host ?? throw new ArgumentNullException(nameof(host));

        if (keys.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException("Keys must be valid", new ArgumentException(nameof(keys)));
        }

        this.Keys = keys;
        _keys = new HashSet<string>(keys);

        this.Description = description;
    }

    public IReplHost Host { get; }

    public IReadOnlyList<string> Keys { get; }

    public string Description { get; }

    public bool IsAcceptableKey(string key)
    {
        return _keys.Contains(key);
    }

    public abstract void Process(ReplContext replContext);
}
