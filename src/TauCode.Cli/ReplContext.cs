using TauCode.Data.Text.TextDataExtractors;

namespace TauCode.Cli;

// todo: internal?
public class ReplContext
{
    private readonly KeyExtractor _keyExtractor;
    private readonly TermExtractor _termExtractor;

    public ReplContext(ReplHost replHost)
    {
        this.ReplHost = replHost;
        _keyExtractor = new KeyExtractor(CliHelper.IsCliWhiteSpace);
        _termExtractor = new TermExtractor(CliHelper.IsCliWhiteSpace);
    }

    public ReplHost ReplHost { get; }

    public ReadOnlyMemory<char> Input { get; set; }
    protected int Position { get; private set; }
    public ReadOnlyMemory<char> RemainingInput => this.Input[this.Position..];

    private void SkipWhiteSpace()
    {
        var pos = this.Position;
        var span = this.Input.Span;
        while (true)
        {
            if (pos == this.Input.Length)
            {
                break;
            }

            var isWhiteSpace = CliHelper.IsCliWhiteSpace(span, pos);
            if (isWhiteSpace)
            {
                // go on
            }
            else
            {
                break;
            }

            pos++;
        }

        this.Position = pos;
    }

    public void Reset(string input)
    {
        this.Input = input.AsMemory();
        this.Position = 0;
    }

    public string? TryExtractKey()
    {
        var result = _keyExtractor.TryExtract(this.RemainingInput.Span, out var key);

        if (key == null)
        {
            return null;
        }

        this.Position += result.CharsConsumed;

        this.SkipWhiteSpace();

        return key;
    }

    public string? TryExtractTerm()
    {
        var result = _termExtractor.TryExtract(this.RemainingInput.Span, out var term);

        if (term == null)
        {
            return null;
        }

        this.Position += result.CharsConsumed;

        this.SkipWhiteSpace();

        return term;
    }

    public void Rewind()
    {
        this.Position = 0;
    }

    public bool IsEndOfInput()
    {
        return this.Position == this.Input.Length;
    }
}